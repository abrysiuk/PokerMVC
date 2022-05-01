Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports PokerMVC.DAL
Imports PokerMVC.Models

Namespace Controllers
    Public Class PlayersController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Players
        Function Index(ByVal sortOrder As String, ByVal Desc As Boolean?) As ActionResult


            Dim result = From s In db.Scores.Include("Game.Night")
                         From tm In db.TeamMembers.Where(Function(x) x.Effective <= s.Game.Night.Scheduled AndAlso x.PlayerID = s.PlayerID).OrderByDescending(Function(x) x.Effective).Take(1).DefaultIfEmpty()
                         Select New With {.Score = s, .Team = tm.Team}

            Dim MaxedGames = result.ToList.Select(Function(x) New With {.Player = x.Score.Player, .Game = x.Score.Game, .Score = Math.Min(x.Score.Game.MaxScore, x.Score.RawScore + x.Score.BonusScore), .Team = x.Team})

            Dim Nights = MaxedGames.GroupBy(Function(mg) New With {Key mg.Player, Key mg.Game.Night}).
                Select(Function(grp) New With {.Player = grp.Key.Player, .Night = grp.Key.Night, .TotalScore = grp.Sum(Function(s) s.Score), grp.FirstOrDefault.Team})

            Dim ts = GetTeamScores()

            Dim joined = From n In Nights
                         From t In ts.Where(Function(t) n.Night.ID = t.Night.ID AndAlso n.Team IsNot Nothing AndAlso n.Team.ID = t.Team.ID)
                         Select New With {n.Night, n.Team, n.Player, n.TotalScore, t.Bonus}

            Dim teamBonus = joined.GroupBy(Function(s) s.Player).Select(Function(s) New With {.Player = s.Key, .TeamBonus = s.Sum(Function(x) x.Bonus)})

            Dim topNights = Nights.GroupBy(Function(grp) grp.Player).SelectMany(Function(p) p.OrderByDescending(Function(x) x.TotalScore).Take(8)).GroupBy(Function(s) s.Player).Select(Function(s) New With {.Player = s.Key, .TopScores = s.Sum(Function(f) Math.Min(f.Night.MaxScore, f.TotalScore))})

            Dim intermed = db.Players.Include("Scores.Game.Night").ToList
            Dim viewResult As IEnumerable(Of PlayerView)

            Dim th = db.TopHands



            viewResult = intermed.Select(Function(p) New PlayerView With {
                                             .Player = p,
                                             .Top8 = topNights.Where(Function(x) x.Player.ID = p.ID).Sum(Function(x) x.TopScores),
                                             .Rank = topNights.Count(Function(i) i.TopScores > .Top8) + 1,
                                             .TeamBonus = teamBonus.Where(Function(x) x.Player.ID = p.ID).Sum(Function(x) x.TeamBonus),
                                             .TopHands = th.Where(Function(x) x.PlayerID = p.ID).Count(),
                                             .Attendance = p.Scores.Select(Function(s) s.Game.Night).Distinct().Count()})

            Select Case sortOrder
                Case "name"
                    viewResult = viewResult.OrderBy(Function(m) m.Player.PublicName)
                Case "gross"
                    viewResult = viewResult.OrderBy(Function(m) m.Top8 + m.TeamBonus + m.TopHands)
                Case "top"
                    viewResult = viewResult.OrderBy(Function(m) m.Top8)
                Case "attend"
                    viewResult = viewResult.OrderBy(Function(m) m.Attendance)
                Case "tophands"
                    viewResult = viewResult.OrderBy(Function(m) m.TopHands)
                Case "teambonus"
                    viewResult = viewResult.OrderBy(Function(m) m.TeamBonus)
                Case Else
                    viewResult = viewResult.OrderBy(Function(m) m.Player.PublicName)
            End Select
            If Desc Then viewResult = viewResult.Reverse
            ViewBag.sortOrder = If(sortOrder, "name")
            ViewBag.Desc = If(Desc, False)
            Return View(viewResult)
        End Function
        Function GetTeamScores() As List(Of TeamTableView)

            Dim Scores = New Dictionary(Of Integer, Double)() From {{3, 5.0F}, {2, 4.0F}, {1, 2.75F}, {0, 1.5F}}

            Dim result = From s In db.Scores.Include("Game.Night")
                         From tm In db.TeamMembers.Where(Function(x) x.Effective <= s.Game.Night.Scheduled AndAlso x.PlayerID = s.PlayerID).OrderByDescending(Function(x) x.Effective).Take(1)
                         Select New With {s, tm.Team}

            Dim teamScores = result.Where(Function(r) r.s.Game.TeamGame AndAlso r.Team IsNot Nothing).GroupBy(Function(r) New With {r.s.Game.Night, r.Team}).ToList()

            Dim teamTable = teamScores.Select(Function(r) New With
                                                  {r.Key.Night, r.Key.Team,
                                                  .attendance = r.Select(Function(s) s.s.PlayerID).Distinct.Count(),
                                                  .score = r.Sum(Function(s) s.s.RawScore + s.s.BonusScore),
                                                  .TeamScore = .score / .attendance + .attendance})

            Dim Ranked = teamTable.OrderBy(Function(r) r.TeamScore).Select(Function(r) New With {
                                                                             r.Night,
                                                                             r.Team,
                                                                             r.TeamScore,
                                                                             r.attendance,
                                                                             .GrossScore = r.score,
                                                                             .bonus = Scores.Where(Function(y) y.Key >= teamTable.Count(Function(x) x.Night.ID = r.Night.ID AndAlso x.TeamScore < r.TeamScore) AndAlso y.Key < teamTable.Count(Function(x) x.Night.ID = r.Night.ID AndAlso x.TeamScore < r.TeamScore) + teamTable.Count(Function(x) x.Night.ID = r.Night.ID AndAlso x.TeamScore = r.TeamScore)).Sum(Function(y) y.Value) / teamTable.Count(Function(x) x.Night.ID = r.Night.ID AndAlso x.TeamScore = r.TeamScore)
                                                                               }).
                                                                             OrderBy(Function(x) x.Night.Scheduled).ThenBy(Function(x) x.bonus)
            Dim output As New List(Of TeamTableView)

            Ranked.ToList.ForEach(Sub(x)
                                      output.Add(New TeamTableView With {.Attendance = x.attendance, .Bonus = x.bonus, .GrossScore = x.GrossScore, .Night = x.Night, .Team = x.Team, .TeamScore = x.TeamScore})
                                  End Sub)


            Return output
        End Function

        ' GET: Players/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim player As Player = db.Players.Find(id)
            If IsNothing(player) Then
                Return HttpNotFound()
            End If
            Return View(player)
        End Function
        ' GET: Players/Create
        <Authorize(Roles:="Admin")>
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Players/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ID,FirstName,LastName,Phone,Email")> ByVal player As Player) As ActionResult
            If ModelState.IsValid Then
                db.Players.Add(player)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(player)
        End Function
        'Get Players/Seating
        <HttpGet()>
        Function Seating() As ActionResult
            Dim players = db.Players.ToList.Where(Function(p) p.Team IsNot Nothing).ToList
            Return View(New SelectionView With {.Players = players.Select(Function(p) New PlayerSelect With {.Player = p, .Selected = 0}).OrderBy(Function(ps) ps.Player.FirstName).ThenBy(Function(ps) ps.Player.LastName).ToList})
        End Function
        'Post: Players/Seating
        <HttpPost>
        <ValidateAntiForgeryToken>
        Function Seating(<Bind(Include:="NoTables,Players,Players.Player.ID,Players.Selected")> Selections As SelectionView) As ActionResult
            Dim Selected = Selections.Players.Where(Function(s) s.Selected).Select(Function(s) s.Player.ID)
            If Selected.Count = 0 Then
                ModelState.AddModelError("SeatGenerator", "You must select 1 or more players")
                Return Seating()
            End If
            Dim keys = ModelState.Keys.Where(Function(k) k.Contains("FirstName")).ToList
            For Each key In keys
                ModelState.Remove(key)
            Next
            If ModelState.IsValid Then

                Dim allplayers = db.Players.ToList.OrderBy(Function(ps) ps.Fullname)
                Dim random = New Random()
                Dim players = allplayers.Where(Function(p) Selected.Contains(p.ID)).OrderBy(Function(g) random.Next).ToList
                Dim playerselect = allplayers.Where(Function(p) p.Team IsNot Nothing).Select(Function(p) New PlayerSelect With {.Player = p, .Selected = players.Contains(p)}).ToList

                Dim teams = players.Select(Function(p) p.Team).Distinct

                Dim tables = players.GroupBy(Function(p) p.Team) _
                .OrderByDescending(Function(g) g.Count) _
                .SelectMany(Function(g) g.ToList) _
                .Select(Function(p, i) New With {.player = p, .Table = (i Mod Selections.NoTables) + 1}) _
                .GroupBy(Function(g) g.Table) _
                .Select(Function(g) New Table With {.Players = g.Select(Function(y) y.player), .Seq = g.Key}) _
                .Select(Function(t) New Table With {.Players = t.Players.OrderBy(Function(p) t.Players.Count(Function(p2) p.Team.ID = p2.Team.ID AndAlso t.Players.ToList.IndexOf(p2) > t.Players.ToList.IndexOf(p))).ThenByDescending(Function(p) t.Players.Count(Function(p2) p2.Team.ID = p.Team.ID)).ThenBy(Function(p) p.Team.ID), .Seq = t.Seq}) _
                .OrderBy(Function(g) g.Seq) _
                .ToList


                Return View(New SelectionView With {.Tables = tables, .Players = playerselect, .NoTables = Selections.NoTables})
            End If
            Selections.Players = db.Players.Where(Function(p) p.Team IsNot Nothing).OrderBy(Function(ps) ps.FirstName).ThenBy(Function(ps) ps.LastName).Select(Function(p) New PlayerSelect With {.Player = p, .Selected = Selected.Contains(p.ID)}).ToList
            Return View(Selections)
        End Function
        ' GET: Players/Edit/5
        <Authorize(Roles:="Admin")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim player As Player = db.Players.Find(id)
            If IsNothing(player) Then
                Return HttpNotFound()
            End If
            Return View(player)
        End Function

        ' POST: Players/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,FirstName,LastName,Phone,Email")> ByVal player As Player) As ActionResult
            If ModelState.IsValid Then
                db.Entry(player).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(player)
        End Function

        ' GET: Players/Delete/5
        <Authorize(Roles:="Admin")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim player As Player = db.Players.Find(id)
            If IsNothing(player) Then
                Return HttpNotFound()
            End If
            Return View(player)
        End Function

        ' POST: Players/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim player As Player = db.Players.Find(id)
            If db.Teams.Any(Function(t) t.CaptainID = player.ID) Then
                ModelState.AddModelError("Captain", "Player is a Captain - please re-assign them before deleting")
                Return View(player)
            End If
            db.Players.Remove(player)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace

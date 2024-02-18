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
            'TODO Cleanup and get rid of anything pulling the team forward. Make better.

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

            Dim intermed = db.Players.Include("Scores.Game.Night").Where(Function(p) p.Scores.Count > 0).ToList
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

            'Select all scores and join it based on a subquery to team members to get the most recent team assignment on or nefore the night of the score - therefore assigning a team to each score.
            Dim results = From score In db.Scores.Include("Game.Night")
                          From tm In db.TeamMembers.Where(Function(x) x.Effective <= score.Game.Night.Scheduled AndAlso x.PlayerID = score.PlayerID).OrderByDescending(Function(x) x.Effective).Take(1)
                          Where tm.Team IsNot Nothing AndAlso score.Game.TeamGame
                          Order By score.Game.Night.Scheduled, score.Player.FirstName
                          Select New With {score, tm.Team}

            'Group by night and team
            Dim teamScores = results.GroupBy(Function(result) New With {result.score.Game.Night, result.Team}).ToList()

            'Cast to an anonymous that sums the gross scores, counts the attendance and calculates net team scores for the night.
            Dim teamTable = teamScores.Select(Function(ResultGroup) New With
                                                  {ResultGroup.Key.Night, ResultGroup.Key.Team,
                                                  .attendance = ResultGroup.Select(Function(s) s.score.PlayerID).Distinct.Count(),
                                                  .score = ResultGroup.Sum(Function(s) s.score.RawScore + s.score.BonusScore),
                                                  .TeamScore = .score / .attendance + .attendance})

            'Draw owl. Case the aforementioned into a TeamTableView, calculating the bonus along the way. Bonus looks up index 0-3 based on how many teams did better.
            Dim Ranked = teamTable.
                Select(Function(thisTeamScore)
                           'Find how many scores are equal to or bigger than my score (include my score) and count how many are tied with me
                           Dim scoresEqualorGreater = teamTable.Count(Function(allTeamScores) allTeamScores.Night.ID = thisTeamScore.Night.ID AndAlso allTeamScores.TeamScore >= thisTeamScore.TeamScore)
                           Dim tiedScores = teamTable.Count(Function(allTeamScores) allTeamScores.Night.ID = thisTeamScore.Night.ID AndAlso allTeamScores.TeamScore = thisTeamScore.TeamScore)
                           'Lookup .bonus against static table - grab every index less than teams that did better than me (includes myself) andalso
                           'is greater than (or usually equal to) the number of teams that did better than me, less those that got the same as me (probably 1. So if no one did better than me, index calculates 0 = 1 (me) - 1 (me))
                           Return New TeamTableView With {
                            .Night = thisTeamScore.Night,
                            .Team = thisTeamScore.Team,
                            .TeamScore = thisTeamScore.TeamScore,
                            .Attendance = thisTeamScore.attendance,
                            .GrossScore = thisTeamScore.score,
                            .Bonus = Scores.
                                    Where(Function(lookup) lookup.Key < scoresEqualorGreater AndAlso lookup.Key >= scoresEqualorGreater - tiedScores).
                                    Sum(Function(lookup) lookup.Value) / tiedScores
                            }
                       End Function)
            Return Ranked.ToList
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

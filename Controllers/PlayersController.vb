Imports System
Imports System.Collections.Generic
Imports System.Data
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
            Dim intermed = db.Players.Include("TopHands").Include("Scores.Game.Night").ToList.OrderByDescending(Function(p) p.GetTopScores(25))
            Dim result As IEnumerable(Of PlayerView)
            result = intermed.Select(Function(p) New PlayerView With {.Player = p, .Rank = intermed.Count(Function(i) i.GetTopScores(25) > p.GetTopScores(25)) + 1, .LastRank = .Rank, .TeamBonus = p.GetTeamBonus})
            'If db.Nights.ToList.Where(Function(n) n.Scheduled < Today).Count > 1 Then
            '    lastnight = db.Nights.ToList.Where(Function(n) n.Scheduled < Today).OrderByDescending(Function(n) n.Scheduled).Take(2).Skip(1).First
            '    result = intermed.Select(Function(p) New PlayerView With {.Player = p, .Rank = intermed.Count(Function(i) i.GetTopScores(25) > p.GetTopScores(25)) + 1, .LastRank = intermed.Count(Function(i) i.GetTopScores(25, lastnight.Scheduled) > p.GetTopScores(25, lastnight.Scheduled)) + 1})
            'Else
            '    result = intermed.Select(Function(p) New PlayerView With {.Player = p, .Rank = intermed.Count(Function(i) i.GetTopScores(25) > p.GetTopScores(25)) + 1, .LastRank = .Rank})
            'End If

            Select Case sortOrder
                Case "name"
                    result = result.OrderBy(Function(m) m.Player.PublicName)
                Case "gross"
                    result = result.OrderBy(Function(m) m.Player.GetTopScores(25) + m.Player.GetTeamBonus + m.Player.GetTopHands)
                Case "top"
                    result = result.OrderBy(Function(m) m.Player.GetTopScores(25))
                Case "attend"
                    result = result.OrderBy(Function(m) m.Player.GetNights.Count)
                Case "tophands"
                    result = result.OrderBy(Function(m) m.Player.GetTopHands)
                Case "teambonus"
                    result = result.OrderBy(Function(m) m.Player.GetTeamBonus)
                Case Else
                    result = result.OrderBy(Function(m) m.Player.PublicName)
            End Select
            If Desc Then result = result.Reverse
            ViewBag.sortOrder = If(sortOrder, "name")
            ViewBag.Desc = If(Desc, False)

            Return View(result)
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

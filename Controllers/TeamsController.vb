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
    Public Class TeamsController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Teams
        Function Index() As ActionResult
            Dim months = db.Nights.GroupBy(Function(n) New Month With {.Year = n.Scheduled.Year, .Month = n.Scheduled.Month})
            Dim vm = New TeamsView

            vm.Teams = db.Teams.Include(Function(x) x.Captain).ToList
            vm.Schedule = months

            Return View(vm)
        End Function

        ' GET: Teams/Details/5
        Function Details(ByVal id As Integer?, ByVal SortOrder As String) As ActionResult
            Dim viewmodel As New TeamView
            Dim months = db.Nights.GroupBy(Function(n) New Month With {.Year = n.Scheduled.Year, .Month = n.Scheduled.Month})

            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim team As Team = db.Teams.Find(id)
            If IsNothing(team) Then
                Return HttpNotFound()
            End If
            viewmodel.Team = team
            If Double.TryParse(SortOrder, New Double) AndAlso months.Count(Function(m) m.Key.Month = SortOrder) > 0 Then
                viewmodel.Members = team.CurrentMembers().OrderByDescending(Function(p) months.FirstOrDefault(Function(m) m.Key.Month = SortOrder).Where(Function(n) n.GetPlayerScore(p) > 0).DefaultIfEmpty(New Models.Night).Average(Function(n) n.GetPlayerScore(p)))
            ElseIf SortOrder = "A" Then
                viewmodel.Members = team.CurrentMembers().OrderBy(Function(m) m.PublicName)
            Else
                viewmodel.Members = team.CurrentMembers().OrderByDescending(Function(m) m.GetTopScores(25))
            End If
            viewmodel.Schedule = months
            Return View(viewmodel)
        End Function

        ' GET: Teams/Create
        <Authorize(Roles:="Admin")>
        Function Create() As ActionResult
            ViewBag.CaptainID = New SelectList(db.Players, "ID", "PublicName").OrderBy(Function(p) p.Text)
            Return View()
        End Function

        ' POST: Teams/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ID,Name,CaptainID")> ByVal team As Team) As ActionResult
            If ModelState.IsValid Then
                db.Teams.Add(team)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.CaptainID = New SelectList(db.Players, "ID", "PublicName", team.CaptainID).OrderBy(Function(p) p.Text)
            Return View(team)
        End Function

        ' GET: Teams/Edit/5
        <Authorize(Roles:="Admin")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim team As Team = db.Teams.Find(id)
            If IsNothing(team) Then
                Return HttpNotFound()
            End If
            ViewBag.CaptainID = New SelectList(db.Players, "ID", "PublicName", team.CaptainID).OrderBy(Function(p) p.Text)
            Return View(team)
        End Function

        ' POST: Teams/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,Name,CaptainID")> ByVal team As Team) As ActionResult
            If ModelState.IsValid Then
                db.Entry(team).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.CaptainID = New SelectList(db.Players, "ID", "PublicName", team.CaptainID).OrderBy(Function(p) p.Text)
            Return View(team)
        End Function

        ' GET: Teams/Delete/5
        <Authorize(Roles:="Admin")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim team As Team = db.Teams.Find(id)
            If IsNothing(team) Then
                Return HttpNotFound()
            End If
            Return View(team)
        End Function

        ' POST: Teams/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim team As Team = db.Teams.Find(id)
            db.Teams.Remove(team)
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

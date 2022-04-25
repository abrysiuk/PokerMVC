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
    Public Class TeamNightOverridesController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: TeamNightOverrides
        Function Index() As ActionResult
            Dim teamNightOverrides = db.TeamNightOverrides.Include(Function(t) t.Night).Include(Function(t) t.Team)
            Return View(teamNightOverrides.ToList())
        End Function

        ' GET: TeamNightOverrides/Details/5
        Function Details(ByVal TeamID As Integer?, ByVal NightID As Integer?) As ActionResult
            If IsNothing(TeamID) Or IsNothing(NightID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim teamNightOverride As TeamNightOverride = db.TeamNightOverrides.Find(TeamID, NightID)
            If IsNothing(teamNightOverride) Then
                Return HttpNotFound()
            End If
            Return View(teamNightOverride)
        End Function

        ' GET: TeamNightOverrides/Create
        Function Create() As ActionResult
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled")
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name")
            Return View()
        End Function

        ' POST: TeamNightOverrides/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="TeamID,NightID,Score")> ByVal teamNightOverride As TeamNightOverride) As ActionResult
            If ModelState.IsValid Then
                db.TeamNightOverrides.Add(teamNightOverride)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", teamNightOverride.NightID)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", teamNightOverride.TeamID)
            Return View(teamNightOverride)
        End Function

        ' GET: TeamNightOverrides/Edit/5
        Function Edit(ByVal TeamID As Integer?, ByVal NightID As Integer?) As ActionResult
            If IsNothing(TeamID) Or IsNothing(NightID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim teamNightOverride As TeamNightOverride = db.TeamNightOverrides.Find(TeamID, NightID)
            If IsNothing(teamNightOverride) Then
                Return HttpNotFound()
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", teamNightOverride.NightID)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", teamNightOverride.TeamID)
            Return View(teamNightOverride)
        End Function

        ' POST: TeamNightOverrides/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="TeamID,NightID,Score")> ByVal teamNightOverride As TeamNightOverride) As ActionResult
            If ModelState.IsValid Then
                db.Entry(teamNightOverride).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", teamNightOverride.NightID)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", teamNightOverride.TeamID)
            Return View(teamNightOverride)
        End Function

        ' GET: TeamNightOverrides/Delete/5
        Function Delete(ByVal TeamID As Integer?, ByVal NightID As Integer?) As ActionResult
            If IsNothing(TeamID) Or IsNothing(NightID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim teamNightOverride As TeamNightOverride = db.TeamNightOverrides.Find(TeamID, NightID)
            If IsNothing(teamNightOverride) Then
                Return HttpNotFound()
            End If
            Return View(teamNightOverride)
        End Function

        ' POST: TeamNightOverrides/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal TeamID As Integer?, ByVal NightID As Integer?) As ActionResult
            Dim teamNightOverride As TeamNightOverride = db.TeamNightOverrides.Find(TeamID, NightID)
            db.TeamNightOverrides.Remove(teamNightOverride)
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

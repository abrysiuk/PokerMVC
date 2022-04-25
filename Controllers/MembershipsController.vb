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
    Public Class MembershipsController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Memberships
        Function Index() As ActionResult
            Dim teamMembers = db.TeamMembers.Include(Function(m) m.Player).Include(Function(m) m.Team)
            Return View(teamMembers.ToList())
        End Function

        ' GET: Memberships/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim membership As Membership = db.TeamMembers.Find(id)
            If IsNothing(membership) Then
                Return HttpNotFound()
            End If
            Return View(membership)
        End Function

        ' GET: Memberships/Create
        <Authorize(Roles:="Admin")>
        Function Create(ByVal PlayerID As Integer?, ByVal TeamID As Integer?, ByVal EffectiveDate As Date?, ByVal Guest As Boolean?) As ActionResult
            If Guest Is Nothing Then Guest = False
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Fullname", PlayerID).OrderBy(Function(s) s.Text)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", TeamID)
            Dim NewMember = New MembershipCreateHelper
            NewMember.Membership = New Membership With {.Effective = If(EffectiveDate, New Date(Today.Year, Today.Month, 1))}
            NewMember.Guest = Guest
            Return View(NewMember)
        End Function

        ' POST: Memberships/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="membership, membership.PlayerID, membership.TeamID, membership.Effective,Guest")> ByVal newMembership As MembershipCreateHelper) As ActionResult
            If ModelState.IsValid Then
                db.TeamMembers.Add(newMembership.Membership)
                If newMembership.Guest Then
                    Dim guestMembership As New Membership
                    guestMembership.TeamID = Nothing
                    guestMembership.Team = Nothing
                    guestMembership.Player = newMembership.Membership.Player
                    guestMembership.PlayerID = newMembership.Membership.PlayerID
                    guestMembership.Effective = newMembership.Membership.Effective.AddDays(1)
                    db.TeamMembers.Add(guestMembership)
                End If
                db.SaveChanges()
                Return RedirectToAction("Create", New With {.TeamID = newMembership.Membership.TeamID, .EffectiveDate = newMembership.Membership.Effective, .Guest = newMembership.Guest})
            End If
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Fullname", newMembership.Membership.PlayerID).OrderBy(Function(s) s.Text)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", newMembership.Membership.TeamID)
            Return View(newMembership)
        End Function

        ' GET: Memberships/Edit/5
        <Authorize(Roles:="Admin")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim membership As Membership = db.TeamMembers.Find(id)
            If IsNothing(membership) Then
                Return HttpNotFound()
            End If
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Fullname", membership.PlayerID).OrderBy(Function(s) s.Text)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", membership.TeamID)
            Return View(membership)
        End Function

        ' POST: Memberships/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,PlayerID,TeamID,Effective")> ByVal membership As Membership) As ActionResult
            If ModelState.IsValid Then
                db.Entry(membership).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Fullname", membership.PlayerID).OrderBy(Function(s) s.Text)
            ViewBag.TeamID = New SelectList(db.Teams, "ID", "Name", membership.TeamID)
            Return View(membership)
        End Function

        ' GET: Memberships/Delete/5
        <Authorize(Roles:="Admin")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim membership As Membership = db.TeamMembers.Find(id)
            If IsNothing(membership) Then
                Return HttpNotFound()
            End If
            Return View(membership)
        End Function

        ' POST: Memberships/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim membership As Membership = db.TeamMembers.Find(id)
            db.TeamMembers.Remove(membership)
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

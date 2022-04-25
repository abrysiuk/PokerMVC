Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports PokerMVC
Imports PokerMVC.DAL

Namespace Controllers
    <Authorize(Roles:="Admin")>
    Public Class ApplicationUsersController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: ApplicationUsers
        Function Index() As ActionResult
            Return View(db.IdentityUsers.ToList())
        End Function

        ' GET: ApplicationUsers/Details/5
        Function Details(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim applicationUser As ApplicationUser = db.IdentityUsers.Find(id)
            If IsNothing(applicationUser) Then
                Return HttpNotFound()
            End If
            Return View(applicationUser)
        End Function

        ' GET: ApplicationUsers/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: ApplicationUsers/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,LastName")> ByVal applicationUser As ApplicationUser) As ActionResult
            If ModelState.IsValid Then
                db.IdentityUsers.Add(applicationUser)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(applicationUser)
        End Function

        ' GET: ApplicationUsers/Edit/5
        Function Edit(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim applicationUser As ApplicationUser = db.IdentityUsers.Find(id)
            If IsNothing(applicationUser) Then
                Return HttpNotFound()
            End If
            Return View(applicationUser)
        End Function

        ' POST: ApplicationUsers/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,LastName")> ByVal applicationUser As ApplicationUser) As ActionResult
            If ModelState.IsValid Then
                db.Entry(applicationUser).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(applicationUser)
        End Function

        ' GET: ApplicationUsers/Delete/5
        Function Delete(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim applicationUser As ApplicationUser = db.IdentityUsers.Find(id)
            If IsNothing(applicationUser) Then
                Return HttpNotFound()
            End If
            Return View(applicationUser)
        End Function

        ' POST: ApplicationUsers/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As String) As ActionResult
            Dim applicationUser As ApplicationUser = db.IdentityUsers.Find(id)
            db.IdentityUsers.Remove(applicationUser)
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

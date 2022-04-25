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
    Public Class RSVPsController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: RSVPs
        <Authorize(Roles:="Admin,PowerUser")>
        Function Index() As ActionResult
            Dim rSVPs = db.RSVPs.Include(Function(r) r.Night).Include(Function(r) r.Player)
            Return View(rSVPs.ToList())
        End Function

        ' GET: RSVPs/Details/5
        <Authorize(Roles:="Admin,PowerUser")>
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim rSVP As RSVP = db.RSVPs.Find(id)
            If IsNothing(rSVP) Then
                Return HttpNotFound()
            End If
            Return View(rSVP)
        End Function

        Function SMS(ByVal PhoneNumber As String, Attending As Boolean?) As ActionResult
            Dim Attend As Boolean
            If String.IsNullOrWhiteSpace(PhoneNumber) OrElse Not Boolean.TryParse(Attending, Attend) Then Return Json(New With {.status = "Error", .message = "Error processing request."}, JsonRequestBehavior.AllowGet)
            Dim night As Night

            Try
                night = db.Nights.First(Function(n) n.Scheduled >= Today AndAlso DbFunctions.AddDays(n.Scheduled, -30) <= Today)
            Catch ex As InvalidOperationException
                If ex.Message = "Sequence contains no elements" Then
                    Return Json(New With {.status = "Error", .message = "There are no nights coming up. Please wait until the confirmation text."}, JsonRequestBehavior.AllowGet)
                Else
                    Return Json(New With {.status = "Error", .message = "An uncaught exception occurred."}, JsonRequestBehavior.AllowGet)
                End If
            End Try

            Dim player As Player

            Try
                player = db.Players.First(Function(p) p.Phone = PhoneNumber)
            Catch ex As InvalidOperationException
                If ex.Message = "Sequence contains no elements" Then
                    Return Json(New With {.status = "Error", .message = "I don't have your phone number on file, please text back with Join to be set-up."}, JsonRequestBehavior.AllowGet)
                Else
                    Return Json(New With {.status = "Error", .message = "An uncaught exception occurred."}, JsonRequestBehavior.AllowGet)
                End If
            End Try

            db.RSVPs.Add(New RSVP With {.PlayerID = player.ID, .NightID = night.ID, .Attending = Attend, .Notified = True})
            db.SaveChanges()
            Return Json(New With {.status = "Success"}, JsonRequestBehavior.AllowGet)
        End Function
        ' GET: RSVPs/Create
        <Authorize(Roles:="Admin,PowerUser")>
        Function Create() As ActionResult
            ViewBag.NightID = New SelectList(db.Nights, "ID", "ID")
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "FirstName")
            Return View()
        End Function

        ' POST: RSVPs/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin,PowerUser")>
        Function Create(<Bind(Include:="ID,PlayerID,NightID,Attending,Notified")> ByVal rSVP As RSVP) As ActionResult
            If ModelState.IsValid Then
                db.RSVPs.Add(rSVP)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "ID", rSVP.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "FirstName", rSVP.PlayerID)
            Return View(rSVP)
        End Function

        ' GET: RSVPs/Edit/5
        <Authorize(Roles:="Admin,PowerUser")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim rSVP As RSVP = db.RSVPs.Find(id)
            If IsNothing(rSVP) Then
                Return HttpNotFound()
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "ID", rSVP.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "FirstName", rSVP.PlayerID)
            Return View(rSVP)
        End Function

        ' POST: RSVPs/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin,PowerUser")>
        Function Edit(<Bind(Include:="ID,PlayerID,NightID,Attending,Notified")> ByVal rSVP As RSVP) As ActionResult
            If ModelState.IsValid Then
                db.Entry(rSVP).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "ID", rSVP.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "FirstName", rSVP.PlayerID)
            Return View(rSVP)
        End Function

        ' GET: RSVPs/Delete/5
        <Authorize(Roles:="Admin,PowerUser")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim rSVP As RSVP = db.RSVPs.Find(id)
            If IsNothing(rSVP) Then
                Return HttpNotFound()
            End If
            Return View(rSVP)
        End Function

        ' POST: RSVPs/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin,PowerUser")>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim rSVP As RSVP = db.RSVPs.Find(id)
            db.RSVPs.Remove(rSVP)
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

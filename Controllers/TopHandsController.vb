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
    Public Class TopHandsController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: TopHands
        Function Index() As ActionResult
            Dim topHands = db.TopHands.Include(Function(t) t.Night).Include(Function(t) t.Player)
            Return View(topHands.ToList())
        End Function

        ' GET: TopHands/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim topHand As TopHand = db.TopHands.Find(id)
            If IsNothing(topHand) Then
                Return HttpNotFound()
            End If
            Return View(topHand)
        End Function

        ' GET: TopHands/Create
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Create(ByVal night As Integer?) As ActionResult
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", night)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Publicname").OrderBy(Function(p) p.Text)
            Return View()
        End Function

        ' POST: TopHands/Create
        'To protect from overposting attacks, enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Create(<Bind(Include:="ID,Hand,PlayerID,NightID")> ByVal topHand As TopHand) As ActionResult
            If ModelState.IsValid Then
                db.TopHands.Add(topHand)
                db.SaveChanges()
                Return RedirectToAction("Create", "TopHands", New With {.night = topHand.NightID})

            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "ID", topHand.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "FirstName", topHand.PlayerID).OrderBy(Function(p) p.Text)
            Return View(topHand)
        End Function

        ' GET: TopHands/Edit/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim topHand As TopHand = db.TopHands.Find(id)
            If IsNothing(topHand) Then
                Return HttpNotFound()
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled")
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Publicname").OrderBy(Function(p) p.Text)
            Return View(topHand)
        End Function

        ' POST: TopHands/Edit/5
        'To protect from overposting attacks, enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <Authorize(Roles:="Admin,Scorekeeper")>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,Hand,PlayerID,NightID")> ByVal topHand As TopHand) As ActionResult
            If ModelState.IsValid Then
                db.Entry(topHand).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", topHand.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Publicname", topHand.PlayerID).OrderBy(Function(p) p.Text)
            Return View(topHand)
        End Function

        ' GET: TopHands/Delete/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim topHand As TopHand = db.TopHands.Find(id)
            If IsNothing(topHand) Then
                Return HttpNotFound()
            End If
            Return View(topHand)
        End Function

        ' POST: TopHands/Delete/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim topHand As TopHand = db.TopHands.Find(id)
            db.TopHands.Remove(topHand)
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

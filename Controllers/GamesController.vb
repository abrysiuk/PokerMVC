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
    Public Class GamesController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Games
        Function Index() As ActionResult
            Return RedirectToAction("Index", "Nights")
        End Function

        ' GET: Games/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return RedirectToAction("Index", "Nights")
            End If
            Dim game As Game = db.Games.Find(id)
            If IsNothing(game) Then
                Return HttpNotFound()
            End If
            Return View(game)
        End Function

        ' GET: Games/Create
        Function Create(night As Integer?) As ActionResult
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", night)
            ViewBag.Night = night
            Return View()
        End Function

        ' POST: Games/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin,Scorekeeper")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ID,TeamGame,Seq,Desc,NightID,PlacePoints,MaxScore")> ByVal game As Game) As ActionResult
            If ModelState.IsValid Then
                db.Games.Add(game)
                db.SaveChanges()
                Return RedirectToAction("Details", "Nights", New With {.id = game.NightID})
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", game.NightID)
            Return View(game)
        End Function

        ' GET: Games/Edit/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim game As Game = db.Games.Find(id)
            If IsNothing(game) Then
                Return HttpNotFound()
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", game.NightID)
            Return View(game)
        End Function

        ' POST: Games/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin,Scorekeeper")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,TeamGame,Seq,Desc,NightID,PlacePoints,MaxScore")> ByVal game As Game) As ActionResult
            If ModelState.IsValid Then
                db.Entry(game).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Details", "Nights", New With {.id = game.NightID})
            End If
            ViewBag.NightID = New SelectList(db.Nights, "ID", "Scheduled", game.NightID)
            Return View(game)
        End Function

        ' GET: Games/Delete/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim game As Game = db.Games.Find(id)
            If IsNothing(game) Then
                Return HttpNotFound()
            End If
            Return View(game)
        End Function

        ' POST: Games/Delete/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim game As Game = db.Games.Find(id)
            db.Games.Remove(game)
            db.SaveChanges()
            Return RedirectToAction("Details", "Nights", New With {.id = game.NightID})
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace

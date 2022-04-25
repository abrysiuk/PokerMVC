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
    Public Class ScoresController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Scores
        Function Index() As ActionResult
            Return RedirectToAction("Index", "Nights", Nothing)
        End Function

        ' GET: Scores/Details/5
        Function Details(ByVal GameID As Integer?, ByVal PlayerID As Integer?) As ActionResult
            If IsNothing(PlayerID) Or IsNothing(GameID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim score As Score = db.Scores.Find(GameID, PlayerID)
            If IsNothing(score) Then
                Return HttpNotFound()
            End If
            Return View(score)
        End Function

        ' GET: Scores/Create
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Create(ByVal GameID As Integer?, ByVal PlayerID As Integer?, ByVal RawScore As Double?) As ActionResult
            ViewBag.GameID = New SelectList(db.Games, "ID", "Identifier", GameID)
            ViewBag.Game = GameID
            Dim Game = db.Games.Find(GameID)
            ViewBag.Night = If(IsNothing(Game), Nothing, Game.NightID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "Publicname", PlayerID).OrderBy(Function(p) p.Text)
            Dim newScore = New Score
            If GameID IsNot Nothing Then newScore.GameID = GameID
            If PlayerID IsNot Nothing Then newScore.PlayerID = PlayerID
            If RawScore IsNot Nothing Then newScore.RawScore = RawScore
            Return View(newScore)
        End Function

        ' POST: Scores/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Create(<Bind(Include:="RawScore,BonusScore,PlayerID,GameID")> ByVal score As Score) As ActionResult
            If ModelState.IsValid Then
                If IsNothing(db.Scores.Find(score.GameID, score.PlayerID)) Then
                    db.Scores.Add(score)
                    db.SaveChanges()
                    Return RedirectToAction("Create", New With {.GameID = score.GameID, .RawScore = score.RawScore - db.Games.First(Function(g) g.ID = score.GameID).PlacePoints})
                Else
                    ModelState.AddModelError("Duplicate", "Score for that user and game already exists")
                End If
            End If
            ViewBag.GameID = New SelectList(db.Games, "ID", "Identifier", score.GameID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "PublicName", score.PlayerID).OrderBy(Function(p) p.Text)
            Return View(score)
        End Function

        ' GET: Scores/Edit/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Edit(ByVal GameID As Integer?, ByVal PlayerID As Integer?) As ActionResult
            If IsNothing(PlayerID) Or IsNothing(GameID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim score As Score = db.Scores.Find(GameID, PlayerID)
            If IsNothing(score) Then
                Return RedirectToAction("Create", "Scores", New With {.GameID = GameID, .PlayerID = PlayerID})
            End If
            ViewBag.GameID = New SelectList(db.Games, "ID", "Identifier", score.GameID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "PublicName", score.PlayerID).OrderBy(Function(p) p.Text)
            Return View(score)
        End Function

        ' POST: Scores/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin,Scorekeeper")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="GameID, PlayerID, RawScore,BonusScore")> ByVal score As Score) As ActionResult
            If ModelState.IsValid Then
                db.Entry(score).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Details", "Nights", New With {.ID = db.Games.Find(score.GameID).NightID})
            End If
            ViewBag.GameID = New SelectList(db.Games, "ID", "Identifier", score.GameID)
            ViewBag.PlayerID = New SelectList(db.Players, "ID", "PublicName", score.PlayerID).OrderBy(Function(p) p.Text)
            Return View(score)
        End Function

        ' GET: Scores/Delete/5
        <Authorize(Roles:="Admin,Scorekeeper")>
        Function Delete(ByVal GameID As Integer?, ByVal PlayerID As Integer?) As ActionResult
            If IsNothing(GameID) Or IsNothing(PlayerID) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim score As Score = db.Scores.Find(GameID, PlayerID)
            If IsNothing(score) Then
                Return HttpNotFound()
            End If
            Return View(score)
        End Function

        ' POST: Scores/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <Authorize(Roles:="Admin,Scorekeeper")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal GameID As Integer?, ByVal PlayerID As Integer?) As ActionResult
            Dim score As Score = db.Scores.Find(GameID, PlayerID)
            db.Scores.Remove(score)
            db.SaveChanges()
            Return RedirectToAction("Details", "Nights", New With {.ID = db.Games.Find(score.GameID).NightID})
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace

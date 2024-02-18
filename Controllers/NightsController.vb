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
    Public Class NightsController
        Inherits System.Web.Mvc.Controller

        Private db As New PokerContext

        ' GET: Nights
        Function Index() As ActionResult
            Return View(db.Nights)
        End Function

        ' GET: Nights/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return RedirectToAction("Index")
            End If
            Dim vm As New NightView
            vm.Night = db.Nights.Where(Function(n) n.ID = id).Include("Games.Scores.Player.Membership").FirstOrDefault()

            If IsNothing(vm.Night) Then
                Return HttpNotFound()
            End If
            Dim rand As New Random(vm.Night.ID)
            Dim players = vm.Night.Games.SelectMany(Function(g) g.Scores.Select(Function(s) s.Player)).Distinct.OrderByDescending(Function(p) p.GetScore(vm.Night)).ThenByDescending(Function(p) p.GetRawScore(vm.Night)).ThenByDescending(Function(p) p.GetNights.Count).ThenBy(Function(x) rand.Next)
            Dim result = players.Select(Function(p) New PlayerView With {.Player = p, .Rank = players.Count(Function(i) i.GetScore(vm.Night) > p.GetScore(vm.Night)) + 1})
            vm.Players = result
            vm.Teams = db.Teams.ToList().Where(Function(t) t.GetTeamBonus(vm.Night) > 0)
            vm.TopHands = db.TopHands.Where(Function(th) th.NightID = id)
            Return View(vm)
        End Function

        ' GET: Nights/Create
        <Authorize(Roles:="Admin")>
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Nights/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="ID,Scheduled, MaxScore")> ByVal night As Night) As ActionResult
            If ModelState.IsValid Then
                db.Nights.Add(night)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(night)
        End Function

        ' GET: Nights/Edit/5
        <Authorize(Roles:="Admin")>
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim night As Night = db.Nights.Find(id)
            If IsNothing(night) Then
                Return HttpNotFound()
            End If
            Return View(night)
        End Function

        ' POST: Nights/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="ID,Scheduled, MaxScore")> ByVal night As Night) As ActionResult
            If ModelState.IsValid Then
                db.Entry(night).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(night)
        End Function

        ' GET: Nights/Delete/5
        <Authorize(Roles:="Admin")>
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim night As Night = db.Nights.Find(id)
            If IsNothing(night) Then
                Return HttpNotFound()
            End If
            Return View(night)
        End Function

        ' POST: Nights/Delete/5
        <HttpPost()>
        <Authorize(Roles:="Admin")>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim night As Night = db.Nights.Find(id)
            db.Nights.Remove(night)
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

Imports PokerMVC.Models
Imports PokerMVC.DAL
Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As New DAL.PokerContext

    Function Index() As ActionResult
        Dim vm As New HomeViewModel

        vm.Teams = db.Teams
        Dim players As IEnumerable(Of Player) = db.Players

        vm.Players = players.OrderByDescending(Function(p) p.GetTopScores(25)).Take(9)
        Dim lastNight = db.Nights.Where(Function(n) n.Scheduled < DateTime.Now And n.Games.Sum(Function(g) g.Scores.Count) > 0).OrderByDescending(Function(n) n.Scheduled).FirstOrDefault
        If lastNight IsNot Nothing Then vm.LastWeek = lastNight

        Dim nextNight = db.Nights.Where(Function(n) n.Scheduled >= DateTime.Today).OrderBy(Function(n) n.Scheduled).FirstOrDefault
        If nextNight IsNot Nothing Then vm.NextNight = nextNight
        Return View(vm)
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function
End Class

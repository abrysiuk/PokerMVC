Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq
Imports PokerMVC.Models

Namespace Migrations

    Friend NotInheritable Class Configuration 
        Inherits DbMigrationsConfiguration(Of DAL.PokerContext)

        Public Sub New()
            AutomaticMigrationsEnabled = False
        End Sub

        Protected Overrides Sub Seed(context As DAL.PokerContext)
            'Dim Andrew = New Player With {.FirstName = "Andrew", .LastName = "Brysiuk"}
            'Dim Devin = New Player With {.FirstName = "Devin", .LastName = "Basky"}
            'Dim Rodney = New Player With {.FirstName = "Rodney"}
            'Dim Aaron = New Player With {.FirstName = "Aaron", .LastName = "Stewart"}
            'Dim Tim = New Player With {.FirstName = "Tim", .LastName = "Berry"}
            'Dim Roger = New Player With {.FirstName = "Roger", .LastName = "Brule"}
            'Dim Scott = New Player With {.FirstName = "Scott", .LastName = "Whitford"}

            'Dim IT = New Team With {.ID = 2, .Name = "IT", .Captain = Andrew}
            'Dim Aspen = New Team With {.ID = 3, .Name = "Aspen", .Captain = Tim}

            'Dim DevinMember = New Models.Membership With {.Player = Devin, .Team = IT, .Effective = "2018/09/01"}
            'Dim ScottMember = New Models.Membership With {.Player = Scott, .Team = Aspen, .Effective = "2018/09/01"}
            'Dim RodneyMember = New Models.Membership With {.Player = Rodney, .Team = IT, .Effective = "2018/09/01"}
            'Dim RodneyMember2 = New Models.Membership With {.Player = Rodney, .Team = Aspen, .Effective = "2018/10/01"}

            'context.Players.AddOrUpdate(Andrew, Devin, Tim, Aaron, Roger, Rodney, Scott)
            'context.Teams.AddOrUpdate(IT, Aspen)
            'context.TeamMembers.AddOrUpdate(DevinMember, RodneyMember, RodneyMember2, ScottMember)
        End Sub

    End Class

End Namespace

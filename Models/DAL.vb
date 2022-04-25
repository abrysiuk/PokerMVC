Imports PokerMVC.Models
Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Entity.ModelConfiguration.Conventions
Imports Microsoft.AspNet.Identity.EntityFramework

Namespace DAL
    Public Class PokerContext
        Inherits IdentityDbContext

        Public Sub New()
            MyBase.New("DefaultConnection")
        End Sub

        Public Property Players As DbSet(Of Player)
        Public Property Teams As DbSet(Of Team)
        Public Property TeamMembers As DbSet(Of Membership)
        Public Property Scores As DbSet(Of Score)
        Public Property Nights As DbSet(Of Night)
        Public Property Games As DbSet(Of Game)
        Public Property RSVPs As DbSet(Of RSVP)
        Public Property TeamNightOverrides As DbSet(Of TeamNightOverride)
        Public Shared Function Create() As PokerContext
            Return New PokerContext()
        End Function

        Public Property IdentityUsers As DbSet(Of ApplicationUser)
        Public Property TopHands As System.Data.Entity.DbSet(Of Models.TopHand)
    End Class
    Public Class PokerInitializer
        Inherits System.Data.Entity.DropCreateDatabaseAlways(Of PokerContext)
    End Class

End Namespace

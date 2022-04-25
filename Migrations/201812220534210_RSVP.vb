Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class RSVP
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.RSVPs",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .PlayerID = c.Int(nullable := False),
                        .NightID = c.Int(nullable := False),
                        .Attending = c.Boolean(nullable := False),
                        .Notified = c.Byte(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Nights", Function(t) t.NightID, cascadeDelete := True) _
                .ForeignKey("dbo.Players", Function(t) t.PlayerID, cascadeDelete := True) _
                .Index(Function(t) t.PlayerID) _
                .Index(Function(t) t.NightID)

            AddColumn("dbo.Players", "Active", Function(c) c.Boolean(nullable:=False, defaultValue:=True))
        End Sub

        Public Overrides Sub Down()
            DropForeignKey("dbo.RSVPs", "PlayerID", "dbo.Players")
            DropForeignKey("dbo.RSVPs", "NightID", "dbo.Nights")
            DropIndex("dbo.RSVPs", New String() {"NightID"})
            DropIndex("dbo.RSVPs", New String() {"PlayerID"})
            DropColumn("dbo.Players", "Active")
            DropTable("dbo.RSVPs")
        End Sub
    End Class
End Namespace

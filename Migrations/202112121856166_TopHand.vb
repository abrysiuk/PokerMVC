Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class TopHand
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.TopHands",
                Function(c) New With
                    {
                        .ID = c.Int(nullable := False, identity := True),
                        .Hand = c.String(),
                        .PlayerID = c.Int(nullable := False),
                        .NightID = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ID) _
                .ForeignKey("dbo.Nights", Function(t) t.NightID, cascadeDelete := True) _
                .ForeignKey("dbo.Players", Function(t) t.PlayerID, cascadeDelete := True) _
                .Index(Function(t) t.PlayerID) _
                .Index(Function(t) t.NightID)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.TopHands", "PlayerID", "dbo.Players")
            DropForeignKey("dbo.TopHands", "NightID", "dbo.Nights")
            DropIndex("dbo.TopHands", New String() { "NightID" })
            DropIndex("dbo.TopHands", New String() { "PlayerID" })
            DropTable("dbo.TopHands")
        End Sub
    End Class
End Namespace

Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class TeamNightOverride
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.TeamNightOverrides",
                Function(c) New With
                    {
                        .TeamID = c.Int(nullable := False),
                        .NightID = c.Int(nullable := False),
                        .Score = c.Byte(nullable := False)
                    }) _
                .PrimaryKey(Function(t) New With { t.TeamID, t.NightID }) _
                .ForeignKey("dbo.Nights", Function(t) t.NightID, cascadeDelete := True) _
                .ForeignKey("dbo.Teams", Function(t) t.TeamID, cascadeDelete := True) _
                .Index(Function(t) t.TeamID) _
                .Index(Function(t) t.NightID)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.TeamNightOverrides", "TeamID", "dbo.Teams")
            DropForeignKey("dbo.TeamNightOverrides", "NightID", "dbo.Nights")
            DropIndex("dbo.TeamNightOverrides", New String() { "NightID" })
            DropIndex("dbo.TeamNightOverrides", New String() { "TeamID" })
            DropTable("dbo.TeamNightOverrides")
        End Sub
    End Class
End Namespace

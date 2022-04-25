Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class NullifyTeam
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropForeignKey("dbo.Memberships", "TeamID", "dbo.Teams")
            DropIndex("dbo.Memberships", New String() { "TeamID" })
            AlterColumn("dbo.Memberships", "TeamID", Function(c) c.Int())
            CreateIndex("dbo.Memberships", "TeamID")
            AddForeignKey("dbo.Memberships", "TeamID", "dbo.Teams", "ID")
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Memberships", "TeamID", "dbo.Teams")
            DropIndex("dbo.Memberships", New String() { "TeamID" })
            AlterColumn("dbo.Memberships", "TeamID", Function(c) c.Int(nullable := False))
            CreateIndex("dbo.Memberships", "TeamID")
            AddForeignKey("dbo.Memberships", "TeamID", "dbo.Teams", "ID", cascadeDelete := True)
        End Sub
    End Class
End Namespace

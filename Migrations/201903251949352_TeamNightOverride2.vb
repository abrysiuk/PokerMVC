Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class TeamNightOverride2
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.TeamNightOverrides", "Score", Function(c) c.Double(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.TeamNightOverrides", "Score", Function(c) c.Byte(nullable := False))
        End Sub
    End Class
End Namespace

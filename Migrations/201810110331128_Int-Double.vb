Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class IntDouble
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Scores", "RawScore", Function(c) c.Double(nullable := False))
            AlterColumn("dbo.Scores", "BonusScore", Function(c) c.Double(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Scores", "BonusScore", Function(c) c.Byte(nullable := False))
            AlterColumn("dbo.Scores", "RawScore", Function(c) c.Byte(nullable := False))
        End Sub
    End Class
End Namespace

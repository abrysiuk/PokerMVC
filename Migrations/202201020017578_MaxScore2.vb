Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class MaxScore2
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Games", "MaxScore", Function(c) c.Double(nullable := False))
            AddColumn("dbo.Nights", "MaxScore", Function(c) c.Double(nullable:=False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Nights", "MaxScore")
            DropColumn("dbo.Games", "MaxScore")
        End Sub
    End Class
End Namespace

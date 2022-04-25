Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class PositionPoint
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Games", "PlacePoints", Function(c) c.Double(nullable:=False, defaultValue:=1))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Games", "PlacePoints")
        End Sub
    End Class
End Namespace

Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations


Namespace Models

#Region "Players and Teams"

    Public Class Player
        Public Property ID As Integer
        Public Property Active As Boolean
        <Required, Display(Name:="First Name")>
        Public Property FirstName As String
        <Display(Name:="Last Name")>
        Public Property LastName As String
        <Display(Name:="Phone Number"), DataType(DataType.PhoneNumber)>
        Public Property Phone As String
        <Display(Name:="Email Address"), DataType(DataType.EmailAddress)>
        Public Property Email As String
        <Display(Name:="Full Name")>
        Public ReadOnly Property Fullname As String
            Get
                Return FirstName & IIf(String.IsNullOrWhiteSpace(LastName), "", " " & LastName)
            End Get
        End Property
        <Display(Name:="Name")>
        Public ReadOnly Property PublicName As String
            Get
                Return FirstName & IIf(String.IsNullOrWhiteSpace(LastName), "", " " & Left(LastName, 1).ToUpper & ".")
            End Get
        End Property
        <Display(Name:="Team")>
        Public Overridable Function Team() As Team
            Return Team(DateTime.Now)
        End Function
        <Display(Name:="Team")>
        Public Overridable Function Team(ByVal AsOf As DateTime) As Team
            If Membership.Count > 0 Then
                Dim rightTeam = Membership.OrderByDescending(Function(x) x.Effective).FirstOrDefault(Function(m) m.Effective <= AsOf)
                If rightTeam Is Nothing OrElse rightTeam.TeamID Is Nothing OrElse rightTeam.Team Is Nothing Then Return Nothing
                Return rightTeam.Team
            Else
                Return Nothing
            End If
        End Function
        Public Overrides Function ToString() As String
            Return PublicName
        End Function
        Public Overridable Property Membership() As ICollection(Of Membership)
        Public Overridable Property Scores() As ICollection(Of Score)
        Public Overridable Property TopHands() As ICollection(Of TopHand)
        Public Overridable Function GetNights() As ICollection(Of Night)
            Return Scores.Select(Function(s) s.Game.Night).Distinct().OrderBy(Function(n) n.Scheduled).ToList()
        End Function
        Public Overridable Function GetRawScore(ByVal Game As Game) As Double
            Dim score = Scores.FirstOrDefault(Function(s) s.GameID = Game.ID)
            If score Is Nothing Then
                Return 0
            Else
                Return score.TotalScore
            End If
        End Function
        Public Overridable Function GetRawScore(ByVal Night As Night) As Double
            Dim playerScores = Scores.Where(Function(s) s.Game.NightID = Night.ID)

            If playerScores.Count = 0 Then
                Return 0
            End If

            Return playerScores.Sum(Function(ps) ps.TotalScore)
        End Function
        Public Overridable Function GetScore(ByVal Game As Game) As Double
            Dim score = Scores.FirstOrDefault(Function(s) s.GameID = Game.ID)
            If score Is Nothing Then
                Return 0
            Else
                Return Math.Min(score.TotalScore, Game.MaxScore)
            End If
        End Function
        Public Overridable Function GetScore(ByVal Night As Night) As Double
            Dim playerScores = Scores.Where(Function(s) s.Game.NightID = Night.ID)

            If playerScores.Count = 0 Then
                Return 0
            End If

            Return Math.Min(playerScores.Sum(Function(ps) Math.Min(ps.TotalScore, ps.Game.MaxScore)), Night.MaxScore)
        End Function
        Public Overridable Function GetTotalGross(ByVal Max As Integer?) As Double
            Dim Nights = Scores.GroupBy(Function(s) s.Game.NightID).Select(Function(g) g.Sum(Function(g2) g2.TotalScore))
            Dim mScores = Nights.Select(Function(s) Math.Min(25, s)).Sum(Function(s) s)
            Return mScores
        End Function
        Public Overridable Function GetTopScores(ByVal Max As Integer?) As Double
            Dim Nights = Scores.GroupBy(Function(s) s.Game.NightID).Select(Function(g) g.Sum(Function(g2) g2.TotalScore)).OrderByDescending(Function(i) i).Take(8)
            Dim mScores = Nights.Select(Function(s) Math.Min(25, s)).Sum(Function(s) s)
            Return mScores
        End Function
        Public Overridable Function GetTopScores(ByVal Max As Integer?, ByVal AsOf As Date) As Double
            Dim Nights = Scores.Where(Function(s) s.Game.Night.Scheduled <= AsOf).GroupBy(Function(s) s.Game.NightID).Select(Function(g) g.Sum(Function(g2) g2.TotalScore)).OrderByDescending(Function(i) i).Take(8)
            Dim mScores = Nights.Select(Function(s) Math.Min(25, s)).Sum(Function(s) s)
            Return mScores
        End Function
        Public Overridable Function GetTeamBonus() As Double
            Return Scores.Select(Function(s) s.Game.Night).Distinct.Sum(Function(n)
                                                                            Dim t = Team(n.Scheduled)
                                                                            If t Is Nothing Then Return 0
                                                                            Return Team(n.Scheduled).GetTeamBonus(n)
                                                                        End Function)
        End Function
        Public Overridable Function GetTopHands() As Double
            Return TopHands.Count
        End Function
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not Me.GetType().Equals(obj.GetType) Then Return False
            Dim objTeam = CType(obj, Player)
            Return objTeam.ID = ID
        End Function

    End Class

    Public Class Team
        <DisplayFormat(NullDisplayText:="0")>
        Public Property ID As Integer
        <DisplayFormat(NullDisplayText:="Guest")>
        Public Property Name As String
        Public Property CaptainID As Integer?
        Public Overridable Property Captain As Player
        Public Overridable Property Members() As ICollection(Of Membership)
        Public Overridable Property NightOverRides() As ICollection(Of TeamNightOverride)
        Public Overridable Function CurrentMembers() As ICollection(Of Player)
            Return CurrentMembers(DateTime.Now)
        End Function
        Public Overridable Function CurrentMembers(ByVal AsOf As DateTime) As ICollection(Of Player)
            Dim cMembers = Members.Where(Function(m) m.Effective <= AsOf AndAlso m.Player.Team(AsOf) IsNot Nothing AndAlso m.Player.Team(AsOf).ID = m.TeamID).Select(Function(m) m.Player).Distinct.ToList
            Return cMembers
        End Function
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Public Overridable Function GetScore(ByVal Game As Game) As Double
            If Not Game.TeamGame Then
                Return 0
            End If

            Return Game.Scores.Where(Function(s) s.Player.Team(Game.Night.Scheduled).ID = ID).Sum(Function(s) s.TotalScore)
        End Function
        Public Overridable Function GetScore(ByVal Night As Night) As Double
            Dim cMembers = CurrentMembers(Night.Scheduled)
            If cMembers.Count = 0 Then Return 0
            Dim nScores = cMembers.SelectMany(Function(m) m.Scores.Where(Function(s) s.Game.TeamGame AndAlso s.Game.NightID = Night.ID)).ToList
            If nScores.Count = 0 Then Return 0

            Return nScores.Sum(Function(s) s.TotalScore)
        End Function

        Public Overridable Function GetAttendance(ByVal Night As Night) As Integer
            Return CurrentMembers(Night.Scheduled).Where(Function(m) m.Scores.Where(Function(s) s.Game.TeamGame AndAlso s.Game.NightID = Night.ID).Count > 0).Distinct.Count
        End Function
        Public Overridable Function GetTeamScore() As Double
            Dim mNights = Members.SelectMany(Function(m) m.Player.Scores.Select(Function(s) s.Game.Night).Distinct).Distinct
            If mNights.Count = 0 Then Return 0
            Return mNights.Sum(Function(n) GetTeamScore(n))
        End Function

        Public Overridable Function GetTeamScore(ByVal Night As Night) As Double
            If Me.NightOverRides.Where(Function(n) n.NightID = Night.ID).Count > 0 Then
                Return Me.NightOverRides.First(Function(n) n.NightID = Night.ID).Score
            Else
                Dim attend = GetAttendance(Night)
                If attend = 0 Then Return 0
                Return (GetScore(Night) / attend) + attend
            End If
        End Function

        Public Overridable Function GetTeamBonus() As Double
            Dim mNights = Members.SelectMany(Function(m) m.Player.Scores.Select(Function(s) s.Game.Night).Distinct).Distinct
            If mNights.Count = 0 Then Return 0
            Return mNights.Sum(Function(n) GetTeamBonus(n))
        End Function

        Public Overridable Function GetTeamBonus(ByVal Night As Night) As Double
            Dim nTeams = Night.GetTeams()
            If nTeams.Count = 0 OrElse Not nTeams.Contains(Me) Then Return 0

            Dim scoresEqualorGreater = nTeams.Count(Function(allTeamScores) allTeamScores.GetTeamScore(Night) >= Me.GetTeamScore(Night))
            Dim tiedScores = nTeams.Count(Function(allTeamScores) allTeamScores.GetTeamScore(Night) = Me.GetTeamScore(Night))

            Dim tDec As Double = 0

            tDec = Scores.Where(
                Function(value) value.Key < scoresEqualorGreater AndAlso value.Key >= scoresEqualorGreater - tiedScores).
                Sum(Function(y) y.Value) / tiedScores

            Return tDec
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not Me.GetType().Equals(obj.GetType) Then Return False
            Dim objTeam = CType(obj, Team)
            Return objTeam.ID = ID AndAlso objTeam.Name = Name
        End Function

    End Class
    Public Class Membership
        Public Property ID As Integer
        <Required>
        <Display(Name:="Player")>
        Public Property PlayerID As Integer
        Public Overridable Property Player As Player
        <Display(Name:="Team")>
        Public Property TeamID As Integer?
        Public Overridable Property Team As Team
        <DataType(DataType.Date)>
        <DisplayFormat(ApplyFormatInEditMode:=True, DataFormatString:="{0:yyyy-MM-dd}")>
        <Display(Name:="Effective Date")>
        Public Property Effective As Date
        Public Overrides Function ToString() As String
            Return Player.PublicName & ":" & Team.Name & "@" & Effective.ToShortDateString
        End Function
    End Class
#End Region
#Region "Games and Scoring"
    Public Class Game
        Public Property ID As Integer
        <Display(Name:="Team Game")>
        Public Property TeamGame As Boolean
        <Display(Name:="Game #")>
        Public Property Seq As Byte
        <Display(Name:="Description")>
        Public Property Desc As String
        <Display(Name:="Max Score")>
        <DefaultSettingValue("20")>
        <Required>
        Public Property MaxScore As Double
        <Required>
        <Display(Name:="Night")>
        Public Property NightID As Integer
        Public Overridable Property Night As Night
        Public Overridable Property Scores() As ICollection(Of Score)
        <DefaultSettingValue("1")>
        <Display(Name:="Points per Position")>
        Public Property PlacePoints As Double
        Public ReadOnly Property Identifier As String
            Get
                Return Night.Scheduled.ToShortDateString & ": Game " & Seq.ToString
            End Get
        End Property
        Public Overrides Function ToString() As String
            Return Identifier
        End Function
        Public Overridable Function GetRawScore(ByVal Player As Player) As Double
            Dim iScore = Scores.Where(Function(s) s.PlayerID = Player.ID).FirstOrDefault
            If iScore Is Nothing Then Return 0
            Return iScore.RawTotal
        End Function
        Public Overridable Function GetScore(ByVal Player As Player) As Double
            Dim iScore = Scores.Where(Function(s) s.PlayerID = Player.ID).FirstOrDefault
            If iScore Is Nothing Then Return 0
            Return Math.Min(iScore.TotalScore, MaxScore)
        End Function
    End Class
    Public Class TeamNightOverride
        Public Property Score As Double
        <Required, Display(Name:="Team"), Key, Schema.Column(Order:=1)>
        Public Property TeamID As Integer
        Public Overridable Property Team As Team
        <Required, Display(Name:="Night"), Key, Schema.Column(Order:=2)>
        Public Property NightID As Integer
        Public Overridable Property Night As Night
    End Class
    Public Class Night
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not Me.GetType().Equals(obj.GetType) Then Return False
            Dim objNight = CType(obj, Night)
            Return objNight.ID = ID AndAlso objNight.Scheduled = Scheduled
        End Function
        Public Property ID As Integer
        <DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode:=False, DataFormatString:="{0:MMM dd, yyyy}")>
        Public Property Scheduled As Date
        Public Property MaxScore As Double
        Public Overridable Property TopHands As ICollection(Of TopHand)
        Public Overridable Property Games() As ICollection(Of Game)
        Public Overrides Function ToString() As String
            Return Scheduled.ToShortDateString
        End Function
        Public Overridable Function GetTeamScore(ByVal Team As Team) As Double
            Return Team.GetTeamScore(Me)
        End Function
        Public Overridable Function GetPlayerScore(ByVal Player As Player) As Double
            Return Math.Min(Player.Scores.Where(Function(s) s.Game.NightID = ID).Sum(Function(s) s.TotalScore), MaxScore)
        End Function
        Public Overridable Function GetTeams() As IEnumerable(Of Team)
            Return Games.SelectMany(Function(g) g.Scores.Select(Function(s) s.Player.Team(Scheduled))).Distinct.Where(Function(t) t IsNot Nothing).ToList
        End Function
    End Class
    Public Class Month
        Public Property Month As Integer
        Public Property Year As Integer
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not Me.GetType().Equals(obj.GetType) Then Return False
            Dim objTyped = CType(obj, Month)
            Return objTyped.Month = Me.Month AndAlso objTyped.Year = Me.Year
        End Function
    End Class

    Public Class Score
        <Display(Name:="Natural Score")>
        <DisplayFormat(NullDisplayText:="0")>
        Public Property RawScore As Double
        <Display(Name:="Bonus Points")>
        <DisplayFormat(NullDisplayText:="0")>
        Public Property BonusScore As Double
        <Required, Display(Name:="Game"), Key, Schema.Column(Order:=1)>
        Public Property GameID As Integer
        Public Overridable Property Game As Game
        <Required, Display(Name:="Player"), Key, Schema.Column(Order:=2)>
        Public Property PlayerID As Integer
        Public Overridable Property Player As Player
        <Display(Name:="Total Score")>
        <DisplayFormat(NullDisplayText:="0")>
        Public ReadOnly Property TotalScore As Double
            Get
                'Return Math.Min(RawScore + BonusScore, Game.MaxScore)
                Return RawScore + BonusScore
            End Get
        End Property
        Public ReadOnly Property RawTotal As Double
            Get
                Return RawScore + BonusScore
            End Get
        End Property
        Public Overrides Function ToString() As String
            Return String.Format("{0}, {1:d} - {2:N}", Player.ToString, Game.Night.Scheduled, RawScore + BonusScore)
        End Function

    End Class
    Public Class TopHand
        Public Property ID As Integer
        <Display(Name:="Hand")>
        Public Property Hand As String
        Public Property PlayerID As Integer
        Public Overridable Property Player As Player
        Public Property NightID As Integer
        Public Overridable Property Night As Night
    End Class
    Public Module AppDefs
        Public Function Scores() As IEnumerable(Of KeyValuePair(Of Integer, Double))
            Return _scores
        End Function
        Public ReadOnly _scores = New Dictionary(Of Integer, Double)() From {{0, 7.0F}, {1, 5.0F}, {2, 3.0F}, {3, 1.0F}}
    End Module
#End Region
#Region "Notifications"
    Public Class RSVP
        Public Property ID As Integer
        Public Property PlayerID As Integer
        Public Overridable Property Player As Player
        Public Property NightID As Integer
        Public Overridable Property Night As Night
        Public Property Attending As Boolean
        Public Property Notified As Byte
    End Class

#End Region
#Region "View Models"
    Public Class TeamView
        Public Property Team As Team
        Public Property Members As IEnumerable(Of Player)
        Public Property Schedule As IEnumerable(Of IGrouping(Of Month, Night))
    End Class
    Public Class NightView
        Public Property Night As Night
        Public Property Players As IEnumerable(Of PlayerView)
        Public Property Teams As IEnumerable(Of Team)
        Public Property TopHands As IEnumerable(Of TopHand)
    End Class
    Public Class HomeViewModel
        Public Property Teams As IEnumerable(Of Team)
        Public Property Players As IEnumerable(Of Player)
        Public Property LastWeek As Night
        Public Property NextNight As Night
    End Class
    Public Class MembershipCreateHelper
        Public Property Membership As Membership
        Public Property Guest As Boolean
    End Class
    Public Class TeamsView
        Public Property Teams As IEnumerable(Of Team)
        Public Property Schedule As IEnumerable(Of IGrouping(Of Month, Night))
    End Class
    Public Class PlayerView
        Public Property Player As Player
        Public Property Rank As Integer
        Public Property TeamBonus As Double
        Public Property Top8 As Double
        Public Property TopHands As Integer
        Public Property Attendance As Integer

    End Class
    Public Class PlayerSelect
        Public Property Player As Player
        Public Property Selected As Boolean
    End Class
    Public Class SelectionView
        Public Property Players As List(Of PlayerSelect)
        <Display(Name:="Number of Tables")>
        <Range(1, Byte.MaxValue, ErrorMessage:="You must choose between 1 and 255 tables")>
        <Required>
        Public Property NoTables As Byte
        Public Property Tables As List(Of Table)
    End Class
    Public Class Table
        Public Property Seq As Integer
        Public Property Players As IEnumerable(Of Player)
    End Class
    Public Class TeamTableView
        Public Property Night As Night
        Public Property Team As Team
        Public Property TeamScore As Double
        Public Property Attendance As Integer
        Public Property GrossScore As Double
        Public Property Bonus As Double

        Public Overrides Function ToString() As String
            Return Night.Scheduled.ToShortDateString() & " - " & Team.Name
        End Function

    End Class

    Public Class TeamBonusView
        Public Property Player As Player

        Public Property Bonus As Double
    End Class
#End Region
End Namespace
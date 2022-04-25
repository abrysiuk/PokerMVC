@ModelType PokerMVC.Models.HomeViewModel
@Code
    ViewData("Title") = "Home"
End Code

<div class="jumbotron">
    <h1>Ministry Poker</h1>
    <p class="lead">Taking the gambling out of poker to encourage clean fellowship between men.</p>
    <p>
        Next Scheduled Game: <strong>@(If(Model.NextNight IsNot Nothing, Model.NextNight.Scheduled.ToLongDateString(), "Unscheduled"))</strong>
    </p>
</div>

<div Class="row">
    <div Class="col-md-4">
        <h2> Teams</h2>
        <Table Class="table">
            <tr>
                <th> Team</th>
                <th> Score</th>
            </tr>
            @For Each team In Model.Teams.OrderByDescending(Function(t) t.GetTeamScore)
                @<tr>
                    <td>@team.Name</td>
                    <td>@String.Format("{0:0.00}", team.GetTeamScore())</td>
                </tr>
            Next
        </Table>
    </div>
    <div Class="col-md-4">
        <h2>Top 9 Overall</h2>
        <table class="table">
            <tr>
                <th>Player</th>
                <th>Score</th>
            </tr>
            @For Each player In Model.Players
                @<tr>
                    <td>
                        @player.PublicName
                </td>
                <td>@player.GetTopScores(25)</td>
            </tr>
        Next
        </table>
    </div>
    @If Model.LastWeek IsNot Nothing Then
        @<div Class="col-md-4">
            <h2> Top 9 Last Week</h2>
            <table class="table">
                <tr>
                    <th>Player</th>
                    <th>Team</th>
                    <th>Total</th>
                </tr>
                @For Each player In Model.LastWeek.Games.SelectMany(Function(g) g.Scores.Select(Function(s) s.Player)).Distinct.OrderByDescending(Function(p) p.GetScore(Model.LastWeek)).Take(9)
                    @<tr>
                        <td>
                            @Html.DisplayFor(Function(modelItem) player.PublicName)
                        </td>
                        <td>
                            @Html.DisplayFor(Function(modelItem) player.Team(Model.LastWeek.Scheduled).Name)
                        </td>
                        <td>
                            @IIf(Model.LastWeek.GetPlayerScore(player) > 25, "25 (" & Model.LastWeek.GetPlayerScore(player).ToString & ")", Model.LastWeek.GetPlayerScore(player).ToString)
                        </td>
                    </tr>
                Next
            </table>
        </div>
    End If
</div>

@ModelType PokerMVC.Models.NightView
@Code
    ViewData("Title") = "Details"
End Code

<h2>@Html.DisplayFor(Function(model) model.Night.Scheduled)</h2>

<div>
    <table class="table">
        <tr>
            <th>Rank</th>
            <th>Player</th>
            <th>Team</th>
            @For Each game In Model.Night.Games.OrderBy(Function(g) g.Seq)
                @<th>
                    @Html.DisplayFor(Function(model) game.Seq)
                </th>
            Next
            <th>Total</th>
        </tr>
        @For Each player In Model.Players
            @<tr>
                <td>@player.Rank.ToString("0") </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) player.Player.PublicName)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) player.Player.Team(Model.Night.Scheduled).Name)
                </td>
                @For Each game In Model.Night.Games.OrderBy(Function(g) g.Seq)
                    @<td>
                        @if Request.IsAuthenticated AndAlso (User.IsInRole("Admin") Or User.IsInRole("ScoreKeeper")) Then
                            If game.GetScore(player.Player) = game.GetRawScore(player.Player) Then
                                @Html.ActionLink(game.GetScore(player.Player), "Edit", "Scores", New With {.GameID = game.ID, .PlayerID = player.Player.ID}, Nothing)
                            Else
                                @Html.ActionLink(String.Format("{0} ({1})", game.GetScore(player.Player), game.GetRawScore(player.Player)), "Edit", "Scores", New With {.GameID = game.ID, .PlayerID = player.Player.ID}, Nothing)
                            End If

                        Else
                            If game.GetScore(player.Player) = game.GetRawScore(player.Player) Then
                                @game.GetScore(player.Player)
                            Else
                                @String.Format("{0} ({1})", game.GetScore(player.Player), game.GetRawScore(player.Player))
                            End If
                        End If
                    </td>
                Next
                <td>
                    @IIf(player.Player.GetRawScore(Model.Night) > player.Player.GetScore(Model.Night), player.Player.GetScore(Model.Night) & " (" & player.Player.GetRawScore(Model.Night) & ")", Model.Night.GetPlayerScore(player.Player).ToString)
                </td>
            </tr>
        Next
        @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") Or User.IsInRole("ScoreKeeper")) Then
            @<tr>
                <td></td>
                <td></td>
                <td></td>
                @For Each game In Model.Night.Games.OrderBy(Function(g) g.Seq)
                    @<td>
                        @Html.ActionLink("+", "Create", "Scores", New With {.GameID = game.ID}, New With {.class = "glyphicon"})
                    </td>
                Next
                <td>
                </td>
            </tr>

        End If
    </table>
    <table class="table">
        <tr>
            <th>Team</th>
            <th>Gross Scores</th>
            <th>Players</th>
            <th>Team Score</th>
            <th>Bonus to Player</th>
        </tr>
        @For Each team In Model.Teams
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) team.Name)
                </td>
                <td>
                    @team.GetScore(Model.Night)
                </td>
                <td>
                    @team.GetAttendance(Model.Night)
                </td>
                <td>
                    @String.Format("{0:0.00}", team.GetTeamScore(Model.Night))
                </td>
                <td>
                    @String.Format("{0:0.00}", team.GetTeamBonus(Model.Night))
                </td>
            </tr>
        Next
    </table>
    <Table Class="table">
        <tr>
            <th>
                Top Hands
            </th>
            <th>
                Player
            </th>
        </tr>
        @For Each item In Model.TopHands.OrderBy(Function(g) g.Player.PublicName)
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Hand)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Player.PublicName)
                </td>
            </tr>
        Next
    </Table>

    <Table Class="table">
        <tr>
            <th>
                Game
            </th>
            <th>
                Description
            </th>
            <th>
                Team Game
            </th>
            <th></th>
        </tr>
        @For Each item In Model.Night.Games.OrderBy(Function(g) g.Seq)
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Seq)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Desc)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.TeamGame)
                </td>
                <td>
                    @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")) Then
                        @Html.ActionLink("Edit", "Edit", "Games", New With {.id = item.ID}, Nothing)
                    End If

                </td>
            </tr>
        Next
    </Table>
</div>
<p>
    @If Request.IsAuthenticated AndAlso User.IsInRole("Admin") Then
        @Html.ActionLink("Add Game", "Create", "Games", New With {.night = Model.Night.ID}, Nothing)
        @<span> | </span>
        @Html.ActionLink("Add Top Hand", "Create", "TopHands", New With {.night = Model.Night.ID}, Nothing)
        @<span> | </span>
        @Html.ActionLink("Edit Night", "Edit", New With {.id = Model.Night.ID})
        @<span> | </span>
    End If
    @Html.ActionLink("Back to List", "Index")
</p>

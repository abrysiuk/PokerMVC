@ModelType PokerMVC.Models.Game
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Game</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Night.ID)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Night.ID)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.TeamGame)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.TeamGame)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Seq)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Seq)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Desc)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Desc)
        </dd>

    </dl>
    <table class="table">
        <tr>
            <th>
                Player
            </th>
            <th>
                Score
            </th>
            <th>
                Team
            </th>
            <th></th>
        </tr>
        @For Each item In Model.Scores.OrderByDescending(Function(s) s.TotalScore)
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Player.PublicName)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.TotalScore)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Player.Membership.Where(Function(m) m.Effective <= modelItem.Night.Scheduled).OrderByDescending(Function(m) m.Effective).FirstOrDefault.Team.Name)
                </td>
                <td>
                    @IIf(User.Identity.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")), Html.ActionLink("Edit", "Edit", "Scores", New With {.GameID = item.GameID, .playerID = item.PlayerID}, Nothing), "")
                </td>
            </tr>
        Next
    </table>
</div>
<p>
    @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")) Then
        @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID})
        @<span> | </span>
    End If
    @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = Model.NightID}, Nothing)
</p>

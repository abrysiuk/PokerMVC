@ModelType PokerMVC.Models.Player
@Code
    ViewData("Title") = "Details"
End Code

<h2>@Html.DisplayFor(Function(model) model.PublicName)</h2>

<div>
    <h4>Player</h4>
    <hr />
    <dl Class="dl-horizontal">
        @If User.Identity.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("Power")) Then
            @<dt>
                @Html.DisplayNameFor(Function(model) model.FirstName)
            </dt>

            @<dd>
                @Html.DisplayFor(Function(model) model.FirstName)
            </dd>
            @<dt>
                @Html.DisplayNameFor(Function(model) model.LastName)
            </dt>

            @<dd>
                @Html.DisplayFor(Function(model) model.LastName)
            </dd>

            @<dt>
                @Html.DisplayNameFor(Function(model) model.Phone)
            </dt>

            @<dd>
                @Html.DisplayFor(Function(model) model.Phone)
            </dd>

            @<dt>
                @Html.DisplayNameFor(Function(model) model.Email)
            </dt>

            @<dd>
                @Html.DisplayFor(Function(model) model.Email)
            </dd>
        End If
        <dt>
            Gross Score
        </dt>
        <dd>
            @Model.GetTotalGross(25)
        </dd>
        <dt>Top 8 Scores</dt>
        <dd>@Model.GetTopScores(25)</dd>
        <dt>Top Hands</dt>
        <dd>@Model.GetTopHands</dd>
        <dt>Team Bonus</dt>
        <dd>@Model.GetTeamBonus</dd>
        <dt>Nights Attended</dt>
        <dd>@Model.GetNights.Count</dd>
    </dl>
    <hr/>
    <table class="table">
        <tr>
            <th>Night</th>
            <th>Score</th>
            <th>Team Bonus</th>
        </tr>
        @For Each night In Model.GetNights
            @<tr>
                <td>@Html.DisplayFor(Function(model) night.Scheduled)</td>
                <td>@IIf(Model.GetRawScore(night) > Model.GetScore(night), Model.GetScore(night) & " (" & Model.GetRawScore(night).ToString & ")", Model.GetScore(night).ToString)</td>
                <td>
                    @CODE
                        Dim tBonus As Double = 0
                        Dim t = Model.Team(night.Scheduled)
                        If t IsNot Nothing Then tBonus = t.GetTeamBonus(night)
                        END CODE
                            @tBonus</td>
            </tr>
                        Next
    </table>
</div>
<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}, New With {.class = "link-body-emphasis link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover"}), "") 
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), " | ", "")
    @Html.ActionLink("Back to List", "Index", Nothing, New With {.class = "link-body-emphasis link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover"})
</p>

@ModelType PokerMVC.Models.TeamView
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Team</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Team.Name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Team.Name)
        </dd>
        <dt>
            Captain
        </dt>
        <dd>
            @Html.DisplayFor(Function(model) model.Team.Captain.PublicName)
        </dd>
    </dl>
</div>

<table class="table">
    <tr>
        <th></th>
        <th></th>
        <th colspan="@Model.Schedule.Count">Average Monthly Score</th>
    </tr>
    <tr>
        <th>
            @Html.ActionLink("Name", "Details", New With {.id = Model.Team.ID, .SortOrder = "A"})
        </th>
        <th>
            @Html.ActionLink("Total Score", "Details", New With {.id = Model.Team.ID, .SortOrder = "0"})
        </th>
        @For Each item In Model.Schedule
            @<th>
                @Html.ActionLink(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(item.Key.Month), "Details", New With {.id = Model.Team.ID, .SortOrder = item.Key.Month})
            </th>
        Next
    </tr>
    @For Each item In Model.Members
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) item.PublicName)
            </td>
            <td>
                @item.GetTopScores(25)
            </td>
            @For Each dMonth In Model.Schedule
            @<td>
                @String.Format("{0:0.00}", dMonth.Where(Function(n) n.GetPlayerScore(item) > 0).DefaultIfEmpty(New Models.Night).Average(Function(n) n.GetPlayerScore(item)))
            </td>   Next
        </tr>
    Next
</table>

<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Edit", "Edit", New With {.id = Model.Team.ID}), "")
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), " | ", "")
    @Html.ActionLink("Back to List", "Index")
</p>

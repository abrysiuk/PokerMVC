@ModelType PokerMVC.Models.TeamsView
@Code
    ViewData("Title") = "Teams"
End Code

<h2>Index</h2>

<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Create New", "Create"), "")
</p>
<Table Class="table">
    <tr>
        <th>
        Team
        </th>
        <th>
        Captain
        </th>
        @For Each item In Model.Schedule
            @<th>
            @String.Format("{0}", System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(item.Key.Month))
        </th>
        Next
        <th>Gross Team Score</th>
        <th>Total Bonus Points</th>
        <th></th>
    </tr>

@For Each item In Model.Teams
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Captain.PublicName)
        </td>
    @For Each dMonth In Model.Schedule
        @<td>
            @String.Format("{0:0.00}", dMonth.Sum(Function(n) n.GetTeamScore(item)))
        </td>   Next
    <td>
            @String.Format("{0:0.00}", item.GetTeamScore)
    </td>
    <td>
            @String.Format("{0:0.00}", item.GetTeamBonus)
    </td>
        <td>
            @Html.ActionLink("Details", "Details", New With {.id = item.ID})
        </td>
    </tr>
Next

</table>

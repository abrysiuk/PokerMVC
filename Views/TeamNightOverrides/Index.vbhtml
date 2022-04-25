@ModelType IEnumerable(Of PokerMVC.Models.TeamNightOverride)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Night.ID)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Team.Name)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Score)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Night.ID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Team.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Score)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.NightID = item.NightID, .TeamID = item.TeamID}) |
            @Html.ActionLink("Details", "Details", New With {.NightID = item.NightID, .TeamID = item.TeamID})|
            @Html.ActionLink("Delete", "Delete", New With {.NightID = item.NightID, .TeamID = item.TeamID})
        </td>
    </tr>
Next

</table>

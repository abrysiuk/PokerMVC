@ModelType IEnumerable(Of PokerMVC.Models.Membership)
@Code
ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Create New", "Create"), "")
</p>
<Table Class="table">
    <tr>
        <th>
        Player
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Team.Name)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Effective)
        </th>
        <th></th>
    </tr>

@For Each item In Model.OrderBy(Function(m) m.Player.PublicName)
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Player.PublicName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Team.Name)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Effective)
        </td>
        <td>
            @Html.ActionLink("Details", "Details", New With {.id = item.ID})
        </td>
    </tr>
Next

</table>

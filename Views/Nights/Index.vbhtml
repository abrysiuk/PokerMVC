@ModelType IEnumerable(Of PokerMVC.Models.Night)
@Code
ViewData("Title") = "Index"
End Code

<h2>Nights</h2>

<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Create New", "Create"), "")
</p>
<Table Class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.Scheduled)
        </th>
        @*<th></th>*@
    </tr>

@For Each item In Model.OrderBy(Function(n) n.Scheduled)
    @<tr>
         <td>
             @*@Html.DisplayFor(Function(modelItem) item.Scheduled)*@
             @Html.ActionLink(item.Scheduled, "Details", New With {.id = item.ID})
         </td>
        @*<td>
            @Html.ActionLink("Details", "Details", New With {.id = item.ID})
        </td>*@
    </tr>
Next

</table>

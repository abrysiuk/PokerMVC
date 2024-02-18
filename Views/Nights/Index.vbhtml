@ModelType IEnumerable(Of PokerMVC.Models.Night)
@Code
ViewData("Title") = "Index"
End Code

<h2>Nights</h2>

<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Create New", "Create", Nothing, New With {.class = "link-body-emphasis link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover"}), "")
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
             @Html.ActionLink(item.Scheduled.ToString("MMMM d, yyyy"), "Details", New With {.id = item.ID}, New With {.class = "link-body-emphasis link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover"})
         </td>
        @*<td>
            @Html.ActionLink("Details", "Details", New With {.id = item.ID})
        </td>*@
    </tr>
Next

</table>

@ModelType IEnumerable(Of PokerMVC.Models.RSVP)
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
            @Html.DisplayNameFor(Function(model) model.Player.FirstName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Attending)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Notified)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Night.ID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Player.FirstName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Attending)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Notified)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.ID }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.ID }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.ID })
        </td>
    </tr>
Next

</table>

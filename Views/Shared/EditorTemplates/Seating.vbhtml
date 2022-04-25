@ModelType PokerMVC.Models.PlayerSelect
<tr>
    <td>
        @Html.DisplayFor(Function(model) model.Player.PublicName)
    </td>
    <td>
        @Html.DisplayFor(Function(model) model.Player.Team)
    </td>
    <td>
        @Html.CheckBoxFor(Function(model) model.Selected)
    </td>
</tr>

@ModelType PokerMVC.Models.SelectionView
@Code
    ViewData("Title") = "Seat Players"
End Code

<h2>Table Generator</h2>
@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    @<div>
         <div>@Html.ValidationMessage("SeatGenerator", New With {.class = "text-danger"})</div>
         <div>@Html.ValidationMessageFor(Function(model) model.NoTables, "", New With {.class = "text-danger"})</div>
    @If Model.Tables IsNot Nothing AndAlso Model.Tables.Count > 0 Then
        @<Table Class="table">
                <tr>
                    <th>
                        Player
                    </th>
                    <th>
                        Team
                    </th>
                </tr>
    @For Each table In Model.Tables
                @<tr>
                    <th colspan = "2" >Table @table.Seq (@table.Players.Count)</th>
                </tr>
        @For Each item In table.Players
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.PublicName)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Team.Name)
                </td>
            </tr>   Next

    Next
</Table> End If
            <p>
                @Html.LabelFor(Function(model) model.NoTables)
                @Html.EditorFor(Function(model) model.NoTables) 
                <input type = "submit" value="Generate" Class="btn btn-secondary mt-2" />
            </p>
            <Table Class="table">
                <tr>
    <th>
    Name
                    </th>
                    <th>
    Team
                    </th>
                    <th></th>
                </tr>

                @For i = 0 To Model.Players.Count - 1
        @<tr>
            <td>
                @Html.HiddenFor(Function(Item) Item.Players(i).Player.ID)
                @Html.DisplayFor(Function(Item) Item.Players(i).Player.PublicName)
            </td>
            <td>
                @Html.DisplayFor(Function(Item) Item.Players(i).Player.Team.Name)
            </td>
            <td>
                @Html.CheckBoxFor(Function(item) item.Players(i).Selected)
            </td>
        </tr>
                    Next

            </Table>
            <input type = "submit" value="Generate" Class="btn btn-secondary mt-2" />
</div>
End Using
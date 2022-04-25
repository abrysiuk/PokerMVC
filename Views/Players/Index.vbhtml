@ModelType IEnumerable(Of PokerMVC.Models.PlayerView)
@Code
    ViewData("Title") = "Index"
End Code

<h2>Players</h2>

<p>
    @Html.ActionLink("Seating Generator", "Seating")
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), " | ", "")
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Create New", "Create"), "")
</p>
<Table Class="table">
    <tr>
        <th>
            @Html.ActionLink("Rank", "Index", New With {.sortOrder = "top", .desc = (ViewBag.sortOrder <> "top") OrElse Not ViewBag.Desc})
        </th>
        <th>
            @Html.ActionLink("Name", "Index", New With {.sortOrder = "name", .desc = ViewBag.sortOrder = "name" AndAlso Not ViewBag.Desc})
            @If ViewBag.sortOrder = "name" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-alphabet-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-alphabet" />
                End If
            End If
        </th>
        <th>
            @Html.ActionLink("Top 8", "Index", New With {.sortOrder = "top", .desc = (ViewBag.sortOrder <> "top") OrElse Not ViewBag.Desc})
            @If ViewBag.sortOrder = "top" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-attributes-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-attributes" />
                End If
            End If
        </th>
        <th>
            @*@Html.ActionLink("Top Hands", "Index", New With {.sortOrder = "tophands", .desc = (ViewBag.sortOrder <> "tophands") OrElse Not ViewBag.Desc})*@
            Top Hands
            @If ViewBag.sortOrder = "tophands" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-attributes-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-attributes" />
                End If
            End If
        </th>
        <th>
            @*@Html.ActionLink("Top Hands", "Index", New With {.sortOrder = "teambonus", .desc = (ViewBag.sortOrder <> "teambonus") OrElse Not ViewBag.Desc})*@
            Team Bonus
            @If ViewBag.sortOrder = "teambonus" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-attributes-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-attributes" />
                End If
            End If
        </th>
        <th>
            @Html.ActionLink("Total", "Index", New With {.sortOrder = "gross", .desc = (ViewBag.sortOrder <> "gross") OrElse Not ViewBag.Desc})
            @If ViewBag.sortOrder = "gross" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-attributes-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-attributes" />
                End If
            End If
        </th>
        <th>
            @Html.ActionLink("Nights", "Index", New With {.sortOrder = "attend", .desc = (ViewBag.sortOrder <> "attend") OrElse Not ViewBag.Desc})
            @If ViewBag.sortOrder = "attend" Then
                @if ViewBag.Desc Then
                    @<span class="glyphicon glyphicon-sort-by-attributes-alt" />
                Else
                    @<span class="glyphicon glyphicon-sort-by-attributes" />
                End If
            End If
        </th>
        <th>
            Team
        </th>
        @*<th></th>*@
    </tr>
    @For Each item In Model
        @<tr>
    @*@If item.Rank < item.LastRank Then
            @<td>@String.Format("{0:0}", item.Rank) <span class="glyphicon glyphicon-arrow-up" style="color:green;"></span>@String.Format("{0:#;#;0}", item.LastRank - item.Rank)</td>
        ElseIf item.Rank > item.LastRank Then
            @<td>@String.Format("{0:0}", item.Rank) <span class="glyphicon glyphicon-arrow-down" style="color:red;"></span>@String.Format("{0:#;#;0}", item.LastRank - item.Rank)</td>
        Else*@
    <td>@String.Format("{0:0}", item.Rank)</td>
    @*End If*@
    <td>
        @Html.ActionLink(item.Player.PublicName, "Details", New With {.id = item.Player.ID})

    </td>
    <td>
        @item.Player.GetTopScores(25)

    </td>
    <td>
        @item.Player.GetTopHands()

    </td>
    <td>

        @item.TeamBonus

    </td>
    <td>
        @(item.Player.GetTopScores(25) + item.TeamBonus + item.Player.GetTopHands)

    </td>
    <td>
        @item.Player.GetNights.Count

    </td>
    <td>
        @Html.DisplayFor(Function(modelItem) item.Player.Team.Name)

    </td>
    @*<td>
            @Html.ActionLink("Details", "Details", New With {.id = item.Player.ID})
        </td>*@
</tr>
    Next

</Table>

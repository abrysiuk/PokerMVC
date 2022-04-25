@ModelType PokerMVC.Models.TeamNightOverride
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>TeamNightOverride</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Night.ID)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Night.ID)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Team.Name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Team.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Score)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Score)
        </dd>

    </dl>
</div>
<p>
    @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
    @Html.ActionLink("Back to List", "Index")
</p>

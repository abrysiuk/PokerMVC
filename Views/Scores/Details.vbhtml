@ModelType PokerMVC.Models.Score
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>Score</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Game.Desc)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Game.Desc)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Player.FirstName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Player.FirstName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.RawScore)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.RawScore)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.BonusScore)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.BonusScore)
        </dd>

    </dl>
</div>
<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Edit", "Edit", New With {.GameID = Model.GameID, .playerID = Model.PlayerID}), "")
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), " | ", "")
    @Html.ActionLink("Back to List", "Details", "Games", New With {.id = Model.GameID}, Nothing)
</p>

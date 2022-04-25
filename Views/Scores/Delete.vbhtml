@ModelType PokerMVC.Models.Score
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
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
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = Model.Game.NightID}, Nothing)
        </div>
    End Using
</div>

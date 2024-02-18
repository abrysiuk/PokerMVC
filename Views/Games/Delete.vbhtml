@ModelType PokerMVC.Models.Game
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Game</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Night.ID)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Night.ID)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.TeamGame)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.TeamGame)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Seq)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Seq)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Desc)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Desc)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-secondary mt-2" /> |
            @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = Model.NightID}, Nothing)
        </div>
    End Using
</div>

@ModelType PokerMVC.Models.Membership
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Membership</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Player.FirstName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Player.FirstName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Team.Name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Team.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Effective)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Effective)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>

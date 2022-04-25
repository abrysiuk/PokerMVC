@ModelType PokerMVC.Models.RSVP
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>RSVP</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Night.ID)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Night.ID)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Player.FirstName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Player.FirstName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Attending)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Attending)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Notified)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Notified)
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

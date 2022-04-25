@ModelType PokerMVC.Models.RSVP
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.ID }) |
    @Html.ActionLink("Back to List", "Index")
</p>

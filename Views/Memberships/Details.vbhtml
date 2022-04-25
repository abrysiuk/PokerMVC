@ModelType PokerMVC.Models.Membership
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
</div>
<p>
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), Html.ActionLink("Edit", "Edit", New With {.id = Model.ID}), "")
    @IIf(User.Identity.IsAuthenticated AndAlso User.IsInRole("Admin"), " | ", "")
    @Html.ActionLink("Back to List", "Index")
</p>

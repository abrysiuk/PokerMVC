@ModelType PokerMVC.ApplicationUser
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>ApplicationUser</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Email)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Email)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.EmailConfirmed)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.EmailConfirmed)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PasswordHash)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PasswordHash)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.SecurityStamp)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.SecurityStamp)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PhoneNumber)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PhoneNumber)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PhoneNumberConfirmed)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PhoneNumberConfirmed)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.TwoFactorEnabled)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.TwoFactorEnabled)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.LockoutEndDateUtc)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LockoutEndDateUtc)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.LockoutEnabled)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LockoutEnabled)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.AccessFailedCount)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.AccessFailedCount)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.UserName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.UserName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.FirstName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.FirstName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.LastName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.LastName)
        </dd>

    </dl>
</div>
<p>
    @Html.ActionLink("Edit", "Edit", New With { .id = Model.Id }) |
    @Html.ActionLink("Back to List", "Index")
</p>

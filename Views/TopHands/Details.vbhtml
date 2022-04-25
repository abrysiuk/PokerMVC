@ModelType PokerMVC.Models.TopHand
@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<div>
    <h4>TopHand</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Night.Scheduled)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Night.Scheduled)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Player.PublicName)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Player.PublicName)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Hand)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Hand)
        </dd>

    </dl>
</div>
<p>
    @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")) Then
        @Html.ActionLink("Edit", "Edit", New With {.id = Model.ID})
        @<span> | </span>
    End If|
    @Html.ActionLink("Back to List", "Index")
</p>

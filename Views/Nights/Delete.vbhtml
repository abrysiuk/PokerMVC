@ModelType PokerMVC.Models.Night
@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Night</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Scheduled)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.Scheduled)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-secondary mt-2" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>

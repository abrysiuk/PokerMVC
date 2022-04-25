@ModelType PokerMVC.Models.Game
@Code
    ViewData("Title") = "Edit"
End Code

<h2>Edit</h2>

@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    
    @<div class="form-horizontal">
    <h4>Game</h4>
    <hr />
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    @Html.HiddenFor(Function(model) model.ID)

    <div class="form-group">
        @Html.LabelFor(Function(model) model.TeamGame, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            <div class="checkbox">
                @Html.EditorFor(Function(model) model.TeamGame)
                @Html.ValidationMessageFor(Function(model) model.TeamGame, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Seq, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Seq, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Seq, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.Desc, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.Desc, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.Desc, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.PlacePoints, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.PlacePoints, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.PlacePoints, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.MaxScore, htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.EditorFor(Function(model) model.MaxScore, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.MaxScore, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        @Html.LabelFor(Function(model) model.NightID, "NightID", htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div class="col-md-10">
            @Html.DropDownList("NightID", Nothing, htmlAttributes:=New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(model) model.NightID, "", New With {.class = "text-danger"})
        </div>
    </div>

    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <input type="submit" value="Save" class="btn btn-default" />
        </div>
    </div>
</div>
End Using

<div>
    @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = Model.NightID}, Nothing) |
    @Html.ActionLink("Delete", "Delete", New With {.id = Model.ID})
</div>

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section

@ModelType PokerMVC.Models.Score
@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

@Using (Html.BeginForm()) 
    @Html.AntiForgeryToken()

    @<div class="form-horizontal">
        <h4>Score</h4>
        <hr />
        @Html.ValidationSummary(False, "", New With {.class = "text-danger"})
         <div class="form-group">
             @Html.LabelFor(Function(model) model.PlayerID, "Player", htmlAttributes:=New With {.class = "control-label col-md-2"})
             <div class="col-md-10">
                 @Html.DropDownList("PlayerID", Nothing, "-Select Player-", htmlAttributes:=New With {.class = "form-control", .autofocus = "autofocus"})
                 @Html.ValidationMessageFor(Function(model) model.PlayerID, "", New With {.class = "text-danger"})
             </div>
         </div>

         <div class="form-group">
             @Html.LabelFor(Function(model) model.GameID, "Game", htmlAttributes:=New With {.class = "control-label col-md-2"})
             <div class="col-md-10">
                 @Html.DropDownList("GameID", Nothing, htmlAttributes:=New With {.class = "form-control"})
                 @Html.ValidationMessageFor(Function(model) model.GameID, "", New With {.class = "text-danger"})
             </div>
         </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.RawScore, htmlAttributes:= New With { .class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.RawScore, New With { .htmlAttributes = New With { .class = "form-control" } })
                @Html.ValidationMessageFor(Function(model) model.RawScore, "", New With { .class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.BonusScore, htmlAttributes:= New With { .class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.BonusScore, New With { .htmlAttributes = New With { .class = "form-control" } })
                @Html.ValidationMessageFor(Function(model) model.BonusScore, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
End Using

<div>
    @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = ViewBag.Night}, Nothing)
</div>

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section

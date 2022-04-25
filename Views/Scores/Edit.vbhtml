@ModelType PokerMVC.Models.Score
@Code
    ViewData("Title") = "Edit"
End Code

<h2>Edit</h2>

@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    
    @<div class="form-horizontal">
        <h4>Score</h4>
        <hr />
        @Html.ValidationSummary(True, "", New With { .class = "text-danger" })

         <div class="form-group">
             @Html.LabelFor(Function(model) model.GameID, "GameID", htmlAttributes:=New With {.class = "control-label col-md-2"})
             <div class="col-md-10">
                 @Html.DisplayFor(Function(m) m.Game.Identifier)
             </div>
         </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.PlayerID, "PlayerID", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DisplayFor(Function(m) m.Player.PublicName)
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.RawScore, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.RawScore, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.RawScore, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.BonusScore, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.BonusScore, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.BonusScore, "", New With {.class = "text-danger"})
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
    @Html.ActionLink("Back to List", "Details", "Nights", New With {.id = Model.Game.NightID}, Nothing) |
    @Html.ActionLink("Delete", "Delete", "Scores", New With {.GameID = Model.GameID, .PlayerID = Model.PlayerID}, Nothing)
</div>

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section

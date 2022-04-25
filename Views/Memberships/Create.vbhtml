@ModelType PokerMVC.Models.MembershipCreateHelper
@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

@Using (Html.BeginForm()) 
    @Html.AntiForgeryToken()
    
    @<div class="form-horizontal">
        <h4>Membership</h4>
        <hr />
        @Html.ValidationSummary(True, "", New With { .class = "text-danger" })
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Membership.PlayerID, "Player", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.Membership.PlayerID, CType(ViewBag.PlayerID, IEnumerable(Of SelectListItem)), New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.Membership.PlayerID, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Membership.TeamID, "Team", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.Membership.TeamID, CType(ViewBag.TeamID, IEnumerable(Of SelectListItem)), "Guest", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.Membership.TeamID, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Membership.Effective, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Membership.Effective, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Membership.Effective, "", New With {.class = "text-danger"})
            </div>
        </div>
         <div class="form-group">
             @Html.LabelFor(Function(model) model.Guest, htmlAttributes:=New With {.class = "control-label col-md-2"})
             <div class="col-md-10">
                 @Html.EditorFor(Function(model) model.Guest)
                 @Html.ValidationMessageFor(Function(model) model.Guest, "", New With {.class = "text-danger"})
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
    @Html.ActionLink("Back to List", "Index")
</div>

@Section Scripts 
    @Scripts.Render("~/bundles/jqueryval")
End Section

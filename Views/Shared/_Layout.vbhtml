<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Ministry Poker</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")

</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                @Html.ActionLink("Ministry Poker", "Index", "Home", New With {.area = ""}, New With {.class = "navbar-brand"})
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    @*<li>@Html.ActionLink("Home", "Index", "Home")</li>*@
                    <li>@Html.ActionLink("Scoreboard", "Index", "Nights")</li>
                    @*<li>@Html.ActionLink("About", "About", "Home")</li>*@
                    <li>@Html.ActionLink("Players", "Index", "Players")</li>
                    <li>@Html.ActionLink("Teams", "Index", "Teams")</li>
                    @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")) Then
                        @<li>@Html.ActionLink("Memberships", "Index", "Memberships")</li>
                        @<li>@Html.ActionLink("Team Override", "Index", "TeamNightOverrides")</li>
                    End If
                </ul>
                @Html.Partial("_LoginPartial")
            </div>
        </div>
    </div>
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - Andrew Brysiuk</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>

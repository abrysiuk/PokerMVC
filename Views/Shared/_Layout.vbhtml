@Imports Microsoft.AspNet.Identity
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Ministry Poker</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">

</head>
<body>
    <nav class="navbar navbar-expand-lg">
        <div class="container-fluid">
            @Html.ActionLink("Ministry Poker", "Index", "Home", Nothing, New With {.class = "navbar-brand"})
            <button class="navbar-toggler d-print-none" type="button" , data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-print-none" id="navbarNavAltMarkup">
                <div class="nav navbar-nav me-auto">
                    @*@Html.ActionLink("Home", "Index", "Home")*@
                    @Html.ActionLink("Scoreboard", "Index", "Nights", Nothing, htmlAttributes:=New With {.class = "nav-link"})
                    @*@Html.ActionLink("About", "About", "Home")*@
                    @Html.ActionLink("Players", "Index", "Players", Nothing, New With {.Class = "nav-link"})
                    @Html.ActionLink("Teams", "Index", "Teams", Nothing, New With {.Class = "nav-link"})
                    @If Request.IsAuthenticated AndAlso (User.IsInRole("Admin") OrElse User.IsInRole("ScoreKeeper")) Then
                        @Html.ActionLink("Memberships", "Index", "Memberships", Nothing, New With {.class = "nav-link"})
                        @Html.ActionLink("Team Override", "Index", "TeamNightOverrides", Nothing, New With {.class = "nav-link"})
                    End If
                </div>
                <div class="nav navbar-nav ms-auto">
                    <div class="nav-item dropdown">
                        <button class="nav-link dropdown-toggle" id="bd-theme" type="button" aria-expanded="false" data-bs-toggle="dropdown" data-bs-display="static" aria-label="Toggle theme (auto)">
                            <i id="theme-icon" class="bi bi-circle-half"></i>
                            <span class="d-lg-none ms-2" id="bd-theme-text">Toggle theme</span>
                        </button>
                        <ul class="dropdown-menu " aria-labelledby="bd-theme-text">
                            <li>
                                <button type="button" class="dropdown-item " data-bs-theme-value="light" aria-pressed="false">
                                    <i class="bi bi-sun-fill"></i>
                                    Light
                                    <svg class="bi ms-auto d-none"><use href="#check2"></use></svg>
                                </button>
                            </li>
                            <li>
                                <button type="button" class="dropdown-item " data-bs-theme-value="dark" aria-pressed="false">
                                    <i class="bi bi-moon-stars-fill"></i>
                                    Dark
                                    <svg class="bi ms-auto d-none"><use href="#check2"></use></svg>
                                </button>
                            </li>
                            <li>
                                <button type="button" class="dropdown-item active" data-bs-theme-value="auto" aria-pressed="true">
                                    <i class="bi bi-circle-half"></i>
                                    Auto
                                    <svg class="bi ms-auto d-none"><use href="#check2"></use></svg>
                                </button>
                            </li>
                        </ul>
                    </div>
                    @If Request.IsAuthenticated Then
                        @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, htmlAttributes:=New With {.id = "logoutForm", .class = "nav navbar-nav"})
                            @Html.AntiForgeryToken()
                            @Html.ActionLink(User.Identity.GetUserName(), "Index", "Manage", Nothing, New With {.title = "Manage", .class = "nav-link"})
                            @<a class="nav-link" href = "javascript:document.getElementById('logoutForm').submit()">Log off</a>
                        End Using
                    Else
                        @Html.ActionLink("Log in", "Login", "Account", Nothing, New With {.id = "loginLink", .class = "nav-link"})
                    End If
                </div>
            </div>
        </div>
    </nav>
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

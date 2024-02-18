
@Imports Microsoft.AspNet.Identity

@If Request.IsAuthenticated Then
    @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm", .class = "navbar-right"})
        @Html.AntiForgeryToken()
        @<ul class="nav navbar-nav navbar-right">
            <li class="nav-item">
                @Html.ActionLink(User.Identity.GetUserName(), "Index", "Manage", routeValues:=Nothing, htmlAttributes:=New With {.title = "Manage"})
            </li>
            <li class="nav-item"><a href="javascript:document.getElementById('logoutForm').submit()">Log off</a></li>
        </ul>   End Using
Else
    @<ul class="nav navbar-nav navbar-right">
        <li class="nav-item">@Html.ActionLink("Log in", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {.id = "loginLink"})</li>
    </ul>
End If


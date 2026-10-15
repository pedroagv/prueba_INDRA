@Code
    Dim ctrl As String = CStr(ViewContext.RouteData.Values("controller"))
    Dim accion As String = CStr(ViewContext.RouteData.Values("action"))
End Code
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewBag.Title - INDRA</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-body-tertiary d-flex flex-column min-vh-100">
    <nav class="navbar navbar-expand-lg bg-body border-bottom sticky-top shadow-sm">
        <div class="container">
            <a class="navbar-brand fw-bold text-primary" href="@Url.Action("Index", "Home")">INDRA</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#menuPrincipal"
                    aria-controls="menuPrincipal" aria-expanded="false" aria-label="Mostrar menú">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="menuPrincipal">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link @(If(ctrl = "Home", "active", ""))" href="@Url.Action("Index", "Home")">Inicio</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link @(If(ctrl = "Estudiantes", "active", ""))" href="@Url.Action("Index", "Estudiantes")">Estudiantes</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link @(If(ctrl = "Calificaciones" AndAlso accion = "Index", "active", ""))" href="@Url.Action("Index", "Calificaciones")">Calificaciones</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link @(If(ctrl = "Calificaciones" AndAlso accion = "Consulta", "active", ""))" href="@Url.Action("Consulta", "Calificaciones")">Consulta por nota</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <main class="container py-4 flex-grow-1">
        @RenderBody()
    </main>

    <footer class="border-top bg-body py-3">
        <div class="container text-body-secondary small">
            &copy; @DateTime.Now.Year - Prueba técnica INDRA
        </div>
    </footer>

    <div class="toast-container position-fixed bottom-0 end-0 p-3" id="toasts"></div>

    @* Token anti-forgery: el JS lo lee y lo manda en cada POST *@
    @Html.AntiForgeryToken()

    <script src="~/Scripts/jquery-3.4.1.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
    <script src="~/Scripts/app/common.js"></script>
    @RenderSection("scripts", required:=False)
</body>
</html>

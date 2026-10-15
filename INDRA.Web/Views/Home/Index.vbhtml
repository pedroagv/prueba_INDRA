@Code
    ViewData("Title") = "Inicio"
End Code

<div class="p-4 p-md-5 mb-4 bg-body border rounded-3 shadow-sm">
    <span class="badge rounded-pill bg-primary-subtle text-primary-emphasis mb-3">Prueba técnica INDRA</span>
    <h1 class="display-5 fw-bold">Estudiantes y calificaciones</h1>
    <p class="lead text-body-secondary mb-4">
        Administra los estudiantes, registra sus notas por materia y periodo,
        y consulta quiénes alcanzan una nota mínima.
    </p>
    <a href="@Url.Action("Index", "Estudiantes")" class="btn btn-primary btn-lg me-2">Estudiantes</a>
    <a href="@Url.Action("Index", "Calificaciones")" class="btn btn-outline-primary btn-lg">Calificaciones</a>
</div>

<div class="row g-4">
    <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm">
            <div class="card-header bg-body fw-semibold text-primary">Estudiantes</div>
            <div class="card-body">
                <p class="card-text text-body-secondary mb-0">Crea, edita y elimina estudiantes. El listado es paginado.</p>
            </div>
            <div class="card-footer bg-body border-0 pt-0">
                <a href="@Url.Action("Index", "Estudiantes")" class="btn btn-sm btn-primary">Ir a estudiantes</a>
            </div>
        </div>
    </div>
    <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm">
            <div class="card-header bg-body fw-semibold text-primary">Calificaciones</div>
            <div class="card-body">
                <p class="card-text text-body-secondary mb-0">Registra notas de 0 a 10 por estudiante, materia y periodo.</p>
            </div>
            <div class="card-footer bg-body border-0 pt-0">
                <a href="@Url.Action("Index", "Calificaciones")" class="btn btn-sm btn-primary">Ir a calificaciones</a>
            </div>
        </div>
    </div>
    <div class="col-md-4">
        <div class="card h-100 border-0 shadow-sm">
            <div class="card-header bg-body fw-semibold text-primary">Consulta por nota</div>
            <div class="card-body">
                <p class="card-text text-body-secondary mb-0">Estudiantes con una nota mayor o igual a la que indiques.</p>
            </div>
            <div class="card-footer bg-body border-0 pt-0">
                <a href="@Url.Action("Consulta", "Calificaciones")" class="btn btn-sm btn-primary">Ir a la consulta</a>
            </div>
        </div>
    </div>
</div>

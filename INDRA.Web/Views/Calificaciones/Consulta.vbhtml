@Code
    ViewData("Title") = "Consulta por nota"
End Code

<div id="app-consulta" data-url-consultar="@Url.Action("PorNota", "Calificaciones")">
    <div class="card border-0 shadow-sm">
        <div class="card-header bg-body d-flex flex-wrap justify-content-between align-items-center gap-2 py-3">
            <h1 class="h4 mb-0">Estudiantes con nota mayor o igual a…</h1>
            <a href="@Url.Action("Index", "Calificaciones")" class="btn btn-outline-secondary">Volver a calificaciones</a>
        </div>
        <div class="card-body border-bottom py-3">
            <form id="formConsulta" class="row g-2 align-items-start" novalidate>
                <div class="col-sm-6 col-md-4">
                    <div class="input-group has-validation">
                        <span class="input-group-text">Nota mínima</span>
                        <input type="number" class="form-control" id="notaMinima" min="0" max="10" step="0.01" value="5" required />
                        <button type="submit" class="btn btn-primary">Consultar</button>
                        <div class="invalid-feedback">Ingresa un valor entre 0 y 10.</div>
                    </div>
                </div>
            </form>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover align-middle mb-0">
                <thead class="table-light">
                    <tr>
                        <th>Documento</th>
                        <th>Estudiante</th>
                        <th>Curso</th>
                        <th>Materia</th>
                        <th>Periodo</th>
                        <th class="text-end">Nota</th>
                    </tr>
                </thead>
                <tbody id="tbodyConsulta"></tbody>
            </table>
        </div>
        <div class="card-footer bg-body small text-body-secondary" id="resumenConsulta">&nbsp;</div>
    </div>
</div>

@Section scripts
    <script src="~/Scripts/app/consulta.js"></script>
End Section

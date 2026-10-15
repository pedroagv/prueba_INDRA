@Code
    ViewData("Title") = "Calificaciones"
End Code

<div id="app-calificaciones"
     data-url-listar="@Url.Action("Listar", "Calificaciones")"
     data-url-guardar="@Url.Action("Guardar", "Calificaciones")"
     data-url-eliminar="@Url.Action("Eliminar", "Calificaciones")"
     data-url-estudiantes="@Url.Action("Listar", "Estudiantes")">

    <div class="card border-0 shadow-sm">
        <div class="card-header bg-body d-flex flex-wrap justify-content-between align-items-center gap-2 py-3">
            <h1 class="h4 mb-0">Calificaciones</h1>
            <div class="d-flex flex-wrap align-items-center gap-2">
                <a href="@Url.Action("Consulta", "Calificaciones")" class="btn btn-outline-secondary">Consulta por nota</a>
                <button type="button" class="btn btn-primary" id="btnNuevo">Nueva calificación</button>
            </div>
        </div>
        <div class="card-body border-bottom py-3">
            <div class="row g-2 align-items-center">
                <div class="col-auto">
                    <label for="filtroEstudiante" class="col-form-label">Filtrar por estudiante</label>
                </div>
                <div class="col-sm-6 col-md-4">
                    <select id="filtroEstudiante" class="form-select">
                        <option value="">Todos</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover align-middle mb-0">
                <thead class="table-light">
                    <tr>
                        <th>Estudiante</th>
                        <th>Materia</th>
                        <th>Periodo</th>
                        <th class="text-end">Nota</th>
                        <th>Observación</th>
                        <th class="text-end">Acciones</th>
                    </tr>
                </thead>
                <tbody id="tbodyCalificaciones"></tbody>
            </table>
        </div>
        <div class="card-footer bg-body d-flex flex-wrap justify-content-between align-items-center gap-2">
            <div id="paginador" class="d-flex flex-wrap align-items-center gap-3"></div>
            <div class="d-flex align-items-center gap-2">
                <label for="tamanoPagina" class="small text-body-secondary">Filas por página</label>
                <select id="tamanoPagina" class="form-select form-select-sm w-auto">
                    <option value="5" selected>5</option>
                    <option value="10">10</option>
                    <option value="15">15</option>
                </select>
            </div>
        </div>
    </div>
</div>

@* Formulario crear / editar *@
<div class="modal fade" id="modalCalificacion" tabindex="-1" aria-labelledby="tituloModalCalificacion" aria-hidden="true">
    <div class="modal-dialog modal-dialog-scrollable">
        <form class="modal-content" id="formCalificacion" novalidate>
            <div class="modal-header bg-primary-subtle">
                <h2 class="modal-title h5" id="tituloModalCalificacion">Nueva calificación</h2>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <input type="hidden" id="cal-id" value="0" />
                <div class="row g-3">
                    <div class="col-12">
                        <label for="cal-estudiante" class="form-label">Estudiante</label>
                        <select class="form-select" id="cal-estudiante" required>
                            <option value="" selected disabled>Selecciona…</option>
                        </select>
                        <div class="invalid-feedback">Selecciona un estudiante.</div>
                    </div>
                    <div class="col-md-7">
                        <label for="cal-materia" class="form-label">Materia</label>
                        <input type="text" class="form-control" id="cal-materia" maxlength="100" required />
                        <div class="invalid-feedback">La materia es obligatoria.</div>
                    </div>
                    <div class="col-md-5">
                        <label for="cal-periodo" class="form-label">Periodo</label>
                        <input type="text" class="form-control" id="cal-periodo" maxlength="30" placeholder="2026-P1" required />
                        <div class="invalid-feedback">El periodo es obligatorio.</div>
                    </div>
                    <div class="col-md-4">
                        <label for="cal-nota" class="form-label">Nota (0 a 10)</label>
                        <input type="number" class="form-control" id="cal-nota" min="0" max="10" step="0.01" required />
                        <div class="invalid-feedback">Ingresa una nota entre 0 y 10.</div>
                    </div>
                    <div class="col-12">
                        <label for="cal-observacion" class="form-label">Observación <span class="text-body-secondary">(opcional)</span></label>
                        <input type="text" class="form-control" id="cal-observacion" maxlength="200" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button type="submit" class="btn btn-primary" id="btnGuardar">Guardar</button>
            </div>
        </form>
    </div>
</div>

@* Confirmacion de eliminacion *@
<div class="modal fade" id="modalEliminar" tabindex="-1" aria-labelledby="tituloModalEliminar" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header bg-danger-subtle">
                <h2 class="modal-title h5" id="tituloModalEliminar">Eliminar calificación</h2>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <p class="mb-0">¿Seguro que quieres eliminar la nota de <strong id="textoEliminar"></strong>?</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-danger" id="btnConfirmarEliminar">Eliminar</button>
            </div>
        </div>
    </div>
</div>

@Section scripts
    <script src="~/Scripts/app/calificaciones.js"></script>
End Section

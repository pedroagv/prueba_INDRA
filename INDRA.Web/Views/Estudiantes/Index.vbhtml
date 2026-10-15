@Code
    ViewData("Title") = "Estudiantes"
End Code

<div id="app-estudiantes"
     data-url-listar="@Url.Action("Listar", "Estudiantes")"
     data-url-guardar="@Url.Action("Guardar", "Estudiantes")"
     data-url-eliminar="@Url.Action("Eliminar", "Estudiantes")">

    <div class="card border-0 shadow-sm">
        <div class="card-header bg-body d-flex flex-wrap justify-content-between align-items-center gap-2 py-3">
            <h1 class="h4 mb-0">Estudiantes</h1>
            <button type="button" class="btn btn-primary" id="btnNuevo">Nuevo estudiante</button>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover align-middle mb-0">
                <thead class="table-light">
                    <tr>
                        <th>Documento</th>
                        <th>Nombres</th>
                        <th>Apellidos</th>
                        <th class="text-end">Edad</th>
                        <th>Sexo</th>
                        <th>Curso</th>
                        <th>Email</th>
                        <th>Estado</th>
                        <th class="text-end">Acciones</th>
                    </tr>
                </thead>
                <tbody id="tbodyEstudiantes"></tbody>
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
<div class="modal fade" id="modalEstudiante" tabindex="-1" aria-labelledby="tituloModalEstudiante" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable">
        <form class="modal-content" id="formEstudiante" novalidate>
            <div class="modal-header bg-primary-subtle">
                <h2 class="modal-title h5" id="tituloModalEstudiante">Nuevo estudiante</h2>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <input type="hidden" id="est-id" value="0" />
                <div class="row g-3">
                    <div class="col-md-4">
                        <label for="est-documento" class="form-label">Documento</label>
                        <input type="text" class="form-control" id="est-documento" maxlength="20" required />
                        <div class="invalid-feedback">El documento es obligatorio.</div>
                    </div>
                    <div class="col-md-4">
                        <label for="est-nombres" class="form-label">Nombres</label>
                        <input type="text" class="form-control" id="est-nombres" maxlength="100" required />
                        <div class="invalid-feedback">Los nombres son obligatorios.</div>
                    </div>
                    <div class="col-md-4">
                        <label for="est-apellidos" class="form-label">Apellidos</label>
                        <input type="text" class="form-control" id="est-apellidos" maxlength="100" required />
                        <div class="invalid-feedback">Los apellidos son obligatorios.</div>
                    </div>
                    <div class="col-md-3">
                        <label for="est-edad" class="form-label">Edad</label>
                        <input type="number" class="form-control" id="est-edad" min="3" max="100" step="1" required />
                        <div class="invalid-feedback">Ingresa una edad entre 3 y 100.</div>
                    </div>
                    <div class="col-md-3">
                        <label for="est-sexo" class="form-label">Sexo</label>
                        <select class="form-select" id="est-sexo" required>
                            <option value="" selected disabled>Selecciona…</option>
                            <option value="F">Femenino</option>
                            <option value="M">Masculino</option>
                        </select>
                        <div class="invalid-feedback">Selecciona el sexo.</div>
                    </div>
                    <div class="col-md-6">
                        <label for="est-curso" class="form-label">Curso</label>
                        <input type="text" class="form-control" id="est-curso" maxlength="50" placeholder="Sin asignar" />
                        <div class="form-text">Si lo dejas vacío se guarda como «Sin asignar».</div>
                    </div>
                    <div class="col-md-8">
                        <label for="est-email" class="form-label">Email <span class="text-body-secondary">(opcional)</span></label>
                        <input type="email" class="form-control" id="est-email" maxlength="150" />
                        <div class="invalid-feedback">El email no tiene un formato válido.</div>
                    </div>
                    <div class="col-md-4 d-flex align-items-end">
                        <div class="form-check form-switch mb-2">
                            <input class="form-check-input" type="checkbox" role="switch" id="est-activo" checked />
                            <label class="form-check-label" for="est-activo">Activo</label>
                        </div>
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
                <h2 class="modal-title h5" id="tituloModalEliminar">Eliminar estudiante</h2>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <p class="mb-1">¿Seguro que quieres eliminar a <strong id="nombreEliminar"></strong>?</p>
                <p class="small text-body-secondary mb-0">Si tiene calificaciones registradas, primero debes eliminarlas.</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-danger" id="btnConfirmarEliminar">Eliminar</button>
            </div>
        </div>
    </div>
</div>

@Section scripts
    <script src="~/Scripts/app/estudiantes.js"></script>
End Section

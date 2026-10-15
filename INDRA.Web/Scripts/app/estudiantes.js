$(function () {
    var urls = $('#app-estudiantes').data();   // urlListar, urlGuardar, urlEliminar
    var pagina = 1, tamano = 5, items = [], idEliminar = 0;

    var $tbody = $('#tbodyEstudiantes');
    var $form = $('#formEstudiante');
    var modalForm = bootstrap.Modal.getOrCreateInstance($('#modalEstudiante')[0]);
    var modalEliminar = bootstrap.Modal.getOrCreateInstance($('#modalEliminar')[0]);

    function cargar() {
        $tbody.empty().append(filaMensaje(9, 'Cargando…', 'cargando'));

        $.getJSON(urls.urlListar, { pagina: pagina, tamano: tamano })
            .done(function (r) {
                // Si borre el ultimo registro de la pagina, vuelvo a la anterior
                if (r.items.length === 0 && r.total > 0 && pagina > 1) {
                    pagina--;
                    return cargar();
                }
                items = r.items;
                dibujar();
                paginador($('#paginador'), pagina, tamano, r.total, function (p) { pagina = p; cargar(); });
            })
            .fail(function (xhr) {
                $tbody.empty().append(filaMensaje(9, mensajeError(xhr), 'error'));
                $('#paginador').empty();
            });
    }

    function dibujar() {
        $tbody.empty();
        if (items.length === 0) {
            $tbody.append(filaMensaje(9, 'No hay estudiantes registrados.'));
            return;
        }
        $.each(items, function (i, e) {
            var estado = e.activo
                ? '<span class="badge rounded-pill bg-success-subtle text-success-emphasis">Activo</span>'
                : '<span class="badge rounded-pill bg-secondary-subtle text-secondary-emphasis">Inactivo</span>';

            $('<tr>').append(
                $('<td>').text(e.documento),
                $('<td>').text(e.nombres),
                $('<td>').text(e.apellidos),
                $('<td class="text-end">').text(e.edad),
                $('<td>').text(e.sexo === 'F' ? 'Femenino' : 'Masculino'),
                $('<td>').text(e.curso),
                $('<td>').text(e.email || '—'),
                $('<td>').html(estado),
                $('<td class="text-end text-nowrap">').html(
                    '<div class="btn-group btn-group-sm">' +
                    '<button type="button" class="btn btn-outline-primary" data-editar="' + i + '">Editar</button>' +
                    '<button type="button" class="btn btn-outline-danger" data-eliminar="' + i + '">Eliminar</button>' +
                    '</div>')
            ).appendTo($tbody);
        });
    }

    // Abro el formulario vacio (nuevo) o con los datos del estudiante (editar)
    function abrirFormulario(e) {
        $form.removeClass('was-validated')[0].reset();
        $('#tituloModalEstudiante').text(e ? 'Editar estudiante' : 'Nuevo estudiante');
        $('#est-id').val(e ? e.id_estudiante : 0);
        if (e) {
            $('#est-documento').val(e.documento);
            $('#est-nombres').val(e.nombres);
            $('#est-apellidos').val(e.apellidos);
            $('#est-edad').val(e.edad);
            $('#est-sexo').val(e.sexo);
            $('#est-curso').val(e.curso === 'Sin asignar' ? '' : e.curso);
            $('#est-email').val(e.email);
            $('#est-activo').prop('checked', e.activo);
        }
        modalForm.show();
    }

    $('#btnNuevo').on('click', function () { abrirFormulario(null); });

    $tbody.on('click', '[data-editar]', function () {
        abrirFormulario(items[$(this).data('editar')]);
    });

    $tbody.on('click', '[data-eliminar]', function () {
        var e = items[$(this).data('eliminar')];
        idEliminar = e.id_estudiante;
        $('#nombreEliminar').text(e.nombres + ' ' + e.apellidos);
        modalEliminar.show();
    });

    $('#tamanoPagina').on('change', function () {
        tamano = Number($(this).val());
        pagina = 1;
        cargar();
    });

    $form.on('submit', function (ev) {
        ev.preventDefault();
        $form.addClass('was-validated');
        if (!this.checkValidity()) { return; }

        var datos = {
            id_estudiante: Number($('#est-id').val()),
            documento: $('#est-documento').val().trim(),
            nombres: $('#est-nombres').val().trim(),
            apellidos: $('#est-apellidos').val().trim(),
            edad: Number($('#est-edad').val()),
            sexo: $('#est-sexo').val(),
            curso: $('#est-curso').val().trim() || null,
            email: $('#est-email').val().trim() || null,
            activo: $('#est-activo').is(':checked')
        };

        var $boton = $('#btnGuardar').prop('disabled', true);
        enviarJson(urls.urlGuardar, datos)
            .done(function () {
                modalForm.hide();
                aviso(datos.id_estudiante ? 'Estudiante actualizado.' : 'Estudiante creado.');
                cargar();
            })
            .fail(function (xhr) { aviso(mensajeError(xhr), 'danger'); })
            .always(function () { $boton.prop('disabled', false); });
    });

    $('#btnConfirmarEliminar').on('click', function () {
        var $boton = $(this).prop('disabled', true);
        enviarJson(urls.urlEliminar, { id_estudiante: idEliminar })
            .done(function () {
                aviso('Estudiante eliminado.');
                cargar();
            })
            .fail(function (xhr) { aviso(mensajeError(xhr), 'danger'); })
            .always(function () {
                modalEliminar.hide();
                $boton.prop('disabled', false);
            });
    });

    cargar();
});

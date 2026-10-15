$(function () {
    var urls = $('#app-calificaciones').data();   // urlListar, urlGuardar, urlEliminar, urlEstudiantes
    var pagina = 1, tamano = 5, idEstudiante = '', items = [], idEliminar = 0;

    var $tbody = $('#tbodyCalificaciones');
    var $form = $('#formCalificacion');
    var modalForm = bootstrap.Modal.getOrCreateInstance($('#modalCalificacion')[0]);
    var modalEliminar = bootstrap.Modal.getOrCreateInstance($('#modalEliminar')[0]);

    // Lleno los dos selects de estudiante: el del filtro y el del formulario
    function cargarEstudiantes() {
        return $.getJSON(urls.urlEstudiantes, { pagina: 1, tamano: 1000 })
            .done(function (r) {
                $.each(r.items, function (i, e) {
                    var nombre = e.nombres + ' ' + e.apellidos;
                    $('#filtroEstudiante, #cal-estudiante').append($('<option>').val(e.id_estudiante).text(nombre));
                });
            })
            .fail(function (xhr) { aviso(mensajeError(xhr), 'danger'); });
    }

    function cargar() {
        $tbody.empty().append(filaMensaje(6, 'Cargando…', 'cargando'));

        $.getJSON(urls.urlListar, { pagina: pagina, tamano: tamano, idEstudiante: idEstudiante })
            .done(function (r) {
                if (r.items.length === 0 && r.total > 0 && pagina > 1) {
                    pagina--;
                    return cargar();
                }
                items = r.items;
                dibujar();
                paginador($('#paginador'), pagina, tamano, r.total, function (p) { pagina = p; cargar(); });
            })
            .fail(function (xhr) {
                $tbody.empty().append(filaMensaje(6, mensajeError(xhr), 'error'));
                $('#paginador').empty();
            });
    }

    function dibujar() {
        $tbody.empty();
        if (items.length === 0) {
            $tbody.append(filaMensaje(6, 'No hay calificaciones registradas.'));
            return;
        }
        $.each(items, function (i, c) {
            $('<tr>').append(
                $('<td>').text(c.estudiante),
                $('<td>').text(c.materia),
                $('<td>').text(c.periodo),
                $('<td class="text-end">').append(badgeNota(c.nota)),
                $('<td>').text(c.observacion || '—'),
                $('<td class="text-end text-nowrap">').html(
                    '<div class="btn-group btn-group-sm">' +
                    '<button type="button" class="btn btn-outline-primary" data-editar="' + i + '">Editar</button>' +
                    '<button type="button" class="btn btn-outline-danger" data-eliminar="' + i + '">Eliminar</button>' +
                    '</div>')
            ).appendTo($tbody);
        });
    }

    // Abro el formulario vacio (nueva) o con los datos de la calificacion (editar)
    function abrirFormulario(c) {
        $form.removeClass('was-validated')[0].reset();
        $('#tituloModalCalificacion').text(c ? 'Editar calificación' : 'Nueva calificación');
        $('#cal-id').val(c ? c.id_calificacion : 0);
        $('#cal-estudiante').val(c ? c.id_estudiante : idEstudiante);
        if (c) {
            $('#cal-materia').val(c.materia);
            $('#cal-periodo').val(c.periodo);
            $('#cal-nota').val(Number(c.nota).toFixed(2));
            $('#cal-observacion').val(c.observacion);
        }
        modalForm.show();
    }

    $('#btnNuevo').on('click', function () { abrirFormulario(null); });

    $tbody.on('click', '[data-editar]', function () {
        abrirFormulario(items[$(this).data('editar')]);
    });

    $tbody.on('click', '[data-eliminar]', function () {
        var c = items[$(this).data('eliminar')];
        idEliminar = c.id_calificacion;
        $('#textoEliminar').text(c.estudiante + ' en ' + c.materia + ' (' + c.periodo + ')');
        modalEliminar.show();
    });

    $('#tamanoPagina').on('change', function () {
        tamano = Number($(this).val());
        pagina = 1;
        cargar();
    });

    $('#filtroEstudiante').on('change', function () {
        idEstudiante = $(this).val();
        pagina = 1;
        cargar();
    });

    $form.on('submit', function (ev) {
        ev.preventDefault();
        $form.addClass('was-validated');
        if (!this.checkValidity()) { return; }

        var datos = {
            id_calificacion: Number($('#cal-id').val()),
            id_estudiante: Number($('#cal-estudiante').val()),
            materia: $('#cal-materia').val().trim(),
            periodo: $('#cal-periodo').val().trim(),
            nota: Number($('#cal-nota').val()),
            observacion: $('#cal-observacion').val().trim() || null
        };

        var $boton = $('#btnGuardar').prop('disabled', true);
        enviarJson(urls.urlGuardar, datos)
            .done(function () {
                modalForm.hide();
                aviso(datos.id_calificacion ? 'Calificación actualizada.' : 'Calificación creada.');
                cargar();
            })
            .fail(function (xhr) { aviso(mensajeError(xhr), 'danger'); })
            .always(function () { $boton.prop('disabled', false); });
    });

    $('#btnConfirmarEliminar').on('click', function () {
        var $boton = $(this).prop('disabled', true);
        enviarJson(urls.urlEliminar, { id_calificacion: idEliminar })
            .done(function () {
                aviso('Calificación eliminada.');
                cargar();
            })
            .fail(function (xhr) { aviso(mensajeError(xhr), 'danger'); })
            .always(function () {
                modalEliminar.hide();
                $boton.prop('disabled', false);
            });
    });

    cargarEstudiantes().always(cargar);
});

// Funciones que uso en todas las pantallas (jQuery para el ajax y Bootstrap 5 para los avisos).

// No quiero cache en los GET
$.ajaxSetup({ cache: false });

// Pantalla de carga: se muestra mientras haya alguna peticion ajax en curso.
// Se queda visible al menos 500 ms para que se alcance a ver aunque el servidor responda rapido.
(function () {
    var pendientes = 0, inicio = 0, temporizador = null;
    var $loading = $(
        '<div class="position-fixed top-0 start-0 w-100 h-100 d-none align-items-center justify-content-center bg-body bg-opacity-75" style="z-index:2000">' +
        '<div class="text-center"><div class="spinner-border text-primary" style="width:3rem;height:3rem" role="status"></div>' +
        '<div class="mt-2 text-body-secondary">Procesando…</div></div></div>');

    $(function () { $loading.appendTo('body'); });

    $(document).on('ajaxSend', function () {
        if (pendientes++ === 0) {
            clearTimeout(temporizador);
            inicio = Date.now();
            $loading.removeClass('d-none').addClass('d-flex');
        }
    });

    $(document).on('ajaxComplete', function () {
        if (--pendientes > 0) { return; }
        pendientes = 0;
        temporizador = setTimeout(function () {
            $loading.removeClass('d-flex').addClass('d-none');
        }, Math.max(0, 500 - (Date.now() - inicio)));
    });
})();

// Mando los datos como JSON por POST (guardar y eliminar), con el token anti-forgery en el header
function enviarJson(url, datos) {
    return $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
        data: JSON.stringify(datos),
        dataType: 'json'
    });
}

// El mensaje de error: el que manda el servidor ({ ok: false, mensaje }) o uno generico
function mensajeError(xhr) {
    return (xhr.responseJSON && xhr.responseJSON.mensaje) || 'No se pudo conectar con el servidor.';
}

// Aviso emergente (toast de Bootstrap). tipo: success o danger
function aviso(texto, tipo) {
    var $toast = $(
        '<div class="toast align-items-center border-0 text-bg-' + (tipo || 'success') + '" role="alert">' +
        '<div class="d-flex"><div class="toast-body"></div>' +
        '<button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>' +
        '</div></div>');
    $toast.find('.toast-body').text(texto);
    $('#toasts').append($toast);
    $toast.on('hidden.bs.toast', function () { $toast.remove(); });
    new bootstrap.Toast($toast[0], { delay: 4000 }).show();
}

// Fila de la tabla con un mensaje (cargando, sin datos o error)
function filaMensaje(columnas, texto, tipo) {
    var $td = $('<td class="text-center py-4">').attr('colspan', columnas);
    if (tipo === 'cargando') {
        $td.append('<span class="spinner-border spinner-border-sm me-2"></span>');
    } else {
        $td.addClass(tipo === 'error' ? 'text-danger' : 'text-body-secondary');
    }
    return $('<tr>').append($td.append(document.createTextNode(texto)));
}

// La nota con color: verde si es 5 o mas, roja si es menor
function badgeNota(nota) {
    var clase = Number(nota) >= 5 ? 'bg-success-subtle text-success-emphasis' : 'bg-danger-subtle text-danger-emphasis';
    return $('<span class="badge rounded-pill">').addClass(clase).text(Number(nota).toFixed(2));
}

// Paginador simple: Anterior / Siguiente y el texto "Pagina X de Y"
function paginador($contenedor, pagina, tamano, total, alCambiar) {
    var paginas = Math.max(1, Math.ceil(total / tamano));

    $contenedor.html(
        '<small class="text-body-secondary">Página ' + pagina + ' de ' + paginas + ' (' + total + ' registros)</small>' +
        '<div class="btn-group btn-group-sm">' +
        '<button type="button" class="btn btn-outline-secondary" data-ir="-1">Anterior</button>' +
        '<button type="button" class="btn btn-outline-secondary" data-ir="1">Siguiente</button>' +
        '</div>');

    $contenedor.find('[data-ir="-1"]').prop('disabled', pagina <= 1);
    $contenedor.find('[data-ir="1"]').prop('disabled', pagina >= paginas);
    $contenedor.find('[data-ir]').on('click', function () {
        alCambiar(pagina + Number($(this).data('ir')));
    });
}

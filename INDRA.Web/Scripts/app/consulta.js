$(function () {
    var url = $('#app-consulta').data('urlConsultar');
    var $tbody = $('#tbodyConsulta');
    var $form = $('#formConsulta');

    function consultar() {
        $tbody.empty().append(filaMensaje(6, 'Consultando…', 'cargando'));

        $.getJSON(url, { notaMinima: $('#notaMinima').val() })
            .done(function (r) {
                $tbody.empty();
                $('#resumenConsulta').text(r.items.length + (r.items.length === 1 ? ' resultado' : ' resultados'));

                if (r.items.length === 0) {
                    $tbody.append(filaMensaje(6, 'Ningún estudiante alcanza esa nota.'));
                    return;
                }
                $.each(r.items, function (i, f) {
                    $('<tr>').append(
                        $('<td>').text(f.documento),
                        $('<td>').text(f.nombres + ' ' + f.apellidos),
                        $('<td>').text(f.curso),
                        $('<td>').text(f.materia),
                        $('<td>').text(f.periodo),
                        $('<td class="text-end">').append(badgeNota(f.nota))
                    ).appendTo($tbody);
                });
            })
            .fail(function (xhr) {
                $tbody.empty().append(filaMensaje(6, mensajeError(xhr), 'error'));
                $('#resumenConsulta').text('');
            });
    }

    $form.on('submit', function (ev) {
        ev.preventDefault();
        $form.addClass('was-validated');
        if (this.checkValidity()) { consultar(); }
    });

    consultar();
});

$(document).ready(function () {
    $(PrecioTotal).val($(PrecioConSimbolo).val())    
    $(RecompensaPuntos).val('+' + $(PuntosEvento).text());
});


$('#miembros').on('change', function () {
    var puntosEvento = $(PuntosEvento).text();
    var precioEvento = $(PrecioPorPersona).val();
    var miembrosSeleccionado = $(this).find(":selected").val();
    var precioTotal = precioEvento * miembrosSeleccionado
    var puntosTotal = puntosEvento * miembrosSeleccionado

    $(PrecioTotal).val(precioTotal + '€');    
    $(RecompensaPuntos).val('+' + puntosTotal);
    if ($(this).find(":selected").val() == 0) {
        $(PrecioTotal).val($(PrecioConSimbolo).val())
        $(RecompensaPuntos).val('+' + $(PuntosEvento).text());
    }
    //Asignamos valor al modelo EventoPago
    $(Miembros).val($(this).find(":selected").val());
    $(PrecioTotalUsuario).val(precioTotal);
    $(PuntosAsignados).val(puntosTotal);
});

$('#fechasEvento').on('change', function () {
    var fechaSeleccionada = $(this).find(":selected").text();
    $(FechaAsistencia).val(fechaSeleccionada);
});
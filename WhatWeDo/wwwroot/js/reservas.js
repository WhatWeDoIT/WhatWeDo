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

$(document).ready(function () {

    var categoria = $("#CategoriaRecibida").text();

    switch (categoria) {
        case '1':
            // Código a ejecutar para la opción 1
            var valorASeleccionar = "ocio";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            break;

        case "2":
            // Código a ejecutar para la opción 2
            var valorASeleccionar = "gastronomia";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            break;

        case "3":
            // Código a ejecutar para la opción 3
            var valorASeleccionar = "deportes";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            break;

        case "4":
            var valorASeleccionar = "aAireLibre";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            break;

        case "5":
            // Código a ejecutar para la opción 5
            var valorASeleccionar = "arteycultura";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            break;

        case "6":
            var valorASeleccionar = "tegyciencia";
            $('input[name="filtro"][value="' + valorASeleccionar + '"]').prop('checked', true);
            // Código a ejecutar para la opción 6
            break;
    }

    $('input[name="filtro"]').click(function () {
        var selectedValue = $('input[name="filtro"]:checked').val();
        if (selectedValue === "ocio") {
            window.location.href = 'eventosporcategoria?categoria=1';
        } else if (selectedValue === "gastronomia") {
            window.location.href = 'eventosporcategoria?categoria=2';
        } else if (selectedValue === "aAireLibre") {
            window.location.href = 'eventosporcategoria?categoria=4';
        } else if (selectedValue === "arteycultura") {
            window.location.href = 'eventosporcategoria?categoria=5';
        } else if (selectedValue === "tegyciencia") {
            window.location.href = 'eventosporcategoria?categoria=6';
        } else if (selectedValue === "deportes") {
            window.location.href = 'eventosporcategoria?categoria=3';
        }
    });
});
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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
        }else if (selectedValue === "arteycultura") {
            window.location.href = 'eventosporcategoria?categoria=5';
        }else if (selectedValue === "tegyciencia") {
            window.location.href = 'eventosporcategoria?categoria=6';
        } else if (selectedValue === "deportes") {
            window.location.href = 'eventosporcategoria?categoria=3';
        }
    });
});
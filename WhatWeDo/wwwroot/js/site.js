// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
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
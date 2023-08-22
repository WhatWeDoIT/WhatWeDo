//Script para verificar el titulo del evento introducido por la empresa
$(document).ready(function () {
    var checkIconTitulo = $("#checkTitulo");
    var exclamationIconTitulo = $("#exclamationTitulo");
    var tituloInput = $("#Titulo");

    var validCharacters = /^[a-zA-Z0-9\s]*$/;

    tituloInput.on("input", function () {
        var tituloValue = tituloInput.val();
        var tituloLength = tituloValue.length;

        // Si el primer carácter es un espacio, eliminarlo
        if (tituloLength > 0 && tituloValue.charAt(0) === " ") {
            tituloInput.val(tituloValue.trim());
            return; // Evitar procesar el evento si se eliminó el espacio
        }

        if (tituloLength <= 0) {
            checkIconTitulo.hide();
            exclamationIconTitulo.hide();
        }
        else if (tituloLength >= 10 && tituloLength <= 50 && validCharacters.test(tituloValue)) {
            checkIconTitulo.show();
            exclamationIconTitulo.hide();
        }
        else {
            checkIconTitulo.hide();
            exclamationIconTitulo.show();
        }
    });

    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    tituloInput.trigger("input");
});

//Script para verificar la dirección introducido por el Usuario
$(document).ready(function () {
    var checkIconDireccion = $("#checkDireccion");
    var exclamationIconDireccion = $("#exclamationDireccion");
    var direccionInput = $("#Direccion");

    direccionInput.on("input", function () {
        var direccionValue = direccionInput.val();

        if (direccionValue.charAt(0) === " ") {
            direccionInput.val(direccionValue.trim());
            return; // Evitar procesar el evento si se eliminó el espacio
        }

        direccionValue = direccionValue.trim(); // Eliminar espacios al principio y al final

        if (direccionValue.length <= 0) {
            checkIconDireccion.hide();
            exclamationIconDireccion.hide();
        } else if (direccionValue.length >= 10 && direccionValue.length <= 50) {
            checkIconDireccion.show();
            exclamationIconDireccion.hide();
        } else {
            checkIconDireccion.hide();
            exclamationIconDireccion.show();
        }
    });


    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    direccionInput.trigger("input");
});


//Script para verificar el email introducido por el Usuario
$(document).ready(function () {//Se ejecuta una vez ya se haya cargado la pagina 
    var checkIconEmail = $("#checkEmail");
    var exclamationIconEmail = $("#exclamationEmail");
    var emailInput = $("#Mail");

    var validEmail = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

    emailInput.on("keydown", function (event) {
        if (event.which === 32 && emailInput.val().length === 0) {
            event.preventDefault(); // Evitar que se ingrese el espacio al principio
        }
    });

    emailInput.on("input", function () {
        var emailValue = emailInput.val().trim(); // Eliminar espacios al principio y al final

        if (emailValue.length === 0) {
            checkIconEmail.hide();
            exclamationIconEmail.hide();
        }
        else if (validEmail.test(emailValue)) {
            checkIconEmail.show();
            exclamationIconEmail.hide();
        } else {
            checkIconEmail.hide();
            exclamationIconEmail.show();
        }
    });

    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    emailInput.trigger("input");
});


//Script para verificar la contraseña introducida por el Usuario
$(document).ready(function () {
    var checkIconPass = $("#checkPass");
    var exclamationIconPass = $("#exclamationPass");
    var passInput = $("#Pass");

    var validPassword = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]{8,}$/;

    passInput.on("keydown", function (event) {
        if (event.which === 32 && passInput.val().length === 0) {
            event.preventDefault(); // Evitar que se ingrese el espacio al principio
        }
    });

    passInput.on("input", function () {
        var passValue = passInput.val().trim(); // Eliminar espacios al principio y al final

        if (passValue.length === 0) {
            checkIconPass.hide();
            exclamationIconPass.hide();
        }
        else if (validPassword.test(passValue)) {
            checkIconPass.show();
            exclamationIconPass.hide();
        } else {
            checkIconPass.hide();
            exclamationIconPass.show();
        }

        // Actualizar el valor del campo de contraseña con los espacios eliminados
        passInput.val(passValue);
    });

    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    passInput.trigger("input");
});

//Script para verificar la confirmación de contraseña introducida por el Usuario
$(document).ready(function () {
    var checkIconConfirmPass = $("#checkConfirmPass");
    var exclamationIconConfirmPass = $("#exclamationConfirmPass");
    var confirmPassInput = $("#ConfirmPass");
    var originalPassInput = $("#Pass");

    confirmPassInput.on("input", function () {
        var confirmPassValue = confirmPassInput.val().trim(); // Eliminar espacios al principio y al final
        var originalPassValue = originalPassInput.val().trim(); // Obtener el valor del campo de contraseña original

        if (confirmPassValue.length === 0) {
            checkIconConfirmPass.hide();
            exclamationIconConfirmPass.hide();
        }
        else if (confirmPassValue === originalPassValue) {
            checkIconConfirmPass.show();
            exclamationIconConfirmPass.hide();
        } else {
            checkIconConfirmPass.hide();
            exclamationIconConfirmPass.show();
        }
    });

    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    confirmPassInput.trigger("input");
});

$("#switch").click(function() {
    MostrarDireccion();
});

$(document).ready(function () {
    MostrarDireccion();
});

function MostrarDireccion() {
    if ($('#switch').prop('checked')) {
        $("#divDireccion").removeClass("d-none");
    }
    else {
        $("#divDireccion").addClass("d-none");
    }

}

//Preview imagen
const defaultFile ='https://wpdirecto.com/wp-content/uploads/2017/08/alt-de-una-imagen.png'
const file = document.getElementById('subirImg');
const imagen = document.getElementById('img');

file.addEventListener('change', e => {
    if (e.target.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            imagen.src = e.target.result;
        }
        reader.readAsDataURL(e.target.files[0])
        
    } else
    {
        imagen.src = defaultFile;
    }

    console.log();
});
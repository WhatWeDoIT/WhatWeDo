//Script para verificar el nombre introducido por el usaurio
$(document).ready(function () {
    var checkIconNombre = $("#checkNombre");
    var exclamationIconNombre = $("#exclamationNombre");
    var nombreInput = $("#Nombre");

    var validCharacters = /^[a-zA-Z0-9\s]*$/;

    nombreInput.on("input", function () {
        var nombreValue = nombreInput.val();
        var nombreLength = nombreValue.length;

        // Si el primer carácter es un espacio, eliminarlo
        if (nombreLength > 0 && nombreValue.charAt(0) === " ") {
            nombreInput.val(nombreValue.trim());
            return; // Evitar procesar el evento si se eliminó el espacio
        }

        if (nombreLength <= 0) {
            checkIconNombre.hide();
            exclamationIconNombre.hide();
        }
        else if (nombreLength >= 3 && nombreLength <= 20 && validCharacters.test(nombreValue)) {
            checkIconNombre.show();
            exclamationIconNombre.hide();
        }
        else {
            checkIconNombre.hide();
            exclamationIconNombre.show();
        }
    });

    // Inicializar tooltips de Bootstrap
    $('[data-toggle="tooltip"]').tooltip();

    // Establecer la visibilidad inicial según el valor de entrada en la carga de la página
    nombreInput.trigger("input");
});

//Script para verificar la dirección introducido por el usaurio
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


//Script para verificar el email introducido por el usaurio
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


//Script para verificar la contraseña introducida por el usaurio
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

//Script para verificar la confirmación de contraseña introducida por el usaurio
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


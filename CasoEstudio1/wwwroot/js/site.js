/**
 * @param {string} title 
 * @param {string} text 
 */
function showSuccessAlert(title, text) {
    Swal.fire({
        icon: 'success',
        title: title || 'Éxito',
        text: text || 'Operación realizada correctamente.',
        timer: 3000,
        showConfirmButton: false
    });
}

/**
 * @param {string} title 
 * @param {string} text 
 */
function showErrorAlert(title, text) {
    Swal.fire({
        icon: 'error',
        title: title || 'Error',
        text: text || 'Hubo un error al procesar la solicitud. Intente de nuevo.',
        confirmButtonText: 'Aceptar'
    });
}

/**
 * @param {string} title 
 * @param {string} text 
 * @param {function} callback 
 */
function showConfirmationAlert(title, text, callback) {
    Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, ¡confirmar!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}
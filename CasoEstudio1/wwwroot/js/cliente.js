var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $('#tblClientes').DataTable({
        "ajax": {
            "url": "/Cliente/ObtenerTodos",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "identificacion", "width": "15%" },
            { "data": "nombre", "width": "20%" },
            { "data": "apellido", "width": "20%" },
            { "data": "edad", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a onclick="abrirModal(${data})" class='btn btn-warning btn-sm text-white' style='cursor:pointer;'>
                                    <i class="fas fa-edit"></i> Editar
                                </a>
                                <a onclick="eliminarCliente(${data})" class='btn btn-danger btn-sm text-white' style='cursor:pointer;'>
                                    <i class="fas fa-trash-alt"></i> Eliminar
                                </a>
                            </div>`;
                }, "width": "25%"
            }
        ],
        "language": {
            "emptyTable": "No hay clientes registrados."
        },
        "width": "100%"
    });
}

function abrirModal(id) {
    limpiarModal();
    if (id === 0) {
        $('#tituloModal').text('Registrar Nuevo Cliente');
    } else {
        $('#tituloModal').text('Editar Cliente');

        var data = dataTable.rows().data().toArray().find(r => r.id === id);

        if (data) {
            $('#clienteId').val(data.id);
            $('#identificacion').val(data.identificacion);
            $('#nombre').val(data.nombre);
            $('#apellido').val(data.apellido);
            $('#edad').val(data.edad);
        }
    }
    $('#clienteModal').modal('show');
}

function limpiarModal() {
    $('#clienteId').val(0);
    $('#identificacion').val('');
    $('#nombre').val('');
    $('#apellido').val('');
    $('#edad').val('');
    $('#clienteForm').find('.text-danger').text('');
}

function guardarCliente() {

    var cliente = {
        Id: parseInt($('#clienteId').val()),
        Identificacion: $('#identificacion').val(),
        Nombre: $('#nombre').val(),
        Apellido: $('#apellido').val(),
        Edad: parseInt($('#edad').val())
    };

    $.ajax({
        url: '/Cliente/Guardar',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(cliente),
        success: function (response) {
            showSuccessAlert('Operación Exitosa', response.message);
            $('#clienteModal').modal('hide');
            dataTable.ajax.reload();
        },
        error: function (xhr) {
            var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : "Error desconocido.";
            showErrorAlert('Error de Negocio', errorMessage);
        }
    });
}

function eliminarCliente(id) {
    showConfirmationAlert(
        '¿Está seguro de eliminar?',
        'No podrá recuperar este registro.',
        function () {
            $.ajax({
                url: '/Cliente/Eliminar/' + id,
                type: 'DELETE',
                success: function (response) {
                    showSuccessAlert('Eliminado', response.message);
                    dataTable.ajax.reload();
                },
                error: function (xhr) {
                    var response = xhr.responseJSON;
                    showErrorAlert('Error al Eliminar', response.message);
                }
            });
        }
    );
}
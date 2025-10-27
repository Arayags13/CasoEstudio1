var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $('#tblVehiculos').DataTable({
        "ajax": {
            "url": "/Vehiculo/ObtenerTodos",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "placa", "width": "15%" },
            { "data": "marca", "width": "20%" },
            { "data": "modelo", "width": "20%" },
            { "data": "clienteId", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a onclick="abrirModal(${data})" class='btn btn-warning btn-sm text-white' style='cursor:pointer;'>
                                    <i class="fas fa-edit"></i> Editar
                                </a>
                                <a onclick="eliminarVehiculo(${data})" class='btn btn-danger btn-sm text-white' style='cursor:pointer;'>
                                    <i class="fas fa-trash-alt"></i> Eliminar
                                </a>
                            </div>`;
                }, "width": "25%"
            }
        ],
        "language": {
            "emptyTable": "No hay vehículos registrados."
        },
        "width": "100%"
    });
}

function abrirModal(id) {
    limpiarModal();
    if (id === 0) {
        $('#tituloModal').text('Registrar Nuevo Vehículo');
    } else {
        $('#tituloModal').text('Editar Vehículo');
        var data = dataTable.rows().data().toArray().find(r => r.id === id);

        if (data) {
            $('#vehiculoId').val(data.id);
            $('#placa').val(data.placa);
            $('#marca').val(data.marca);
            $('#modelo').val(data.modelo);
            $('#clienteId').val(data.clienteId);
        }
    }
    $('#vehiculoModal').modal('show');
}

function limpiarModal() {
    $('#vehiculoId').val(0);
    $('#placa').val('');
    $('#marca').val('');
    $('#modelo').val('');
    $('#clienteId').val('');
    $('#vehiculoForm').find('.text-danger').text('');
}

function guardarVehiculo() {
    var vehiculo = {
        Id: parseInt($('#vehiculoId').val()),
        Placa: $('#placa').val(),
        Marca: $('#marca').val(),
        Modelo: $('#modelo').val(),
        ClienteId: parseInt($('#clienteId').val())
    };

    if (isNaN(vehiculo.ClienteId) || vehiculo.ClienteId <= 0) {
        showErrorAlert('Error de Validación', 'Debe seleccionar un cliente.');
        return;
    }

    $.ajax({
        url: '/Vehiculo/Guardar',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(vehiculo),
        success: function (response) {
            showSuccessAlert('Operación Exitosa', response.message);
            $('#vehiculoModal').modal('hide');
            dataTable.ajax.reload();
        },
        error: function (xhr) {
            var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : "Error desconocido.";
            showErrorAlert('Error de Negocio', errorMessage);
        }
    });
}

function eliminarVehiculo(id) {
    showConfirmationAlert(
        '¿Está seguro de eliminar este vehículo?',
        'Si el vehículo tiene citas asociadas, podría fallar la eliminación.',
        function () {
            $.ajax({
                url: '/Vehiculo/Eliminar/' + id,
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
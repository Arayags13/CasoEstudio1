var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $('#tblCitas').DataTable({
        "ajax": {
            "url": "/Cita/ObtenerTodos",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "clienteNombreCompleto", "width": "20%" },
            { "data": "vehiculoPlaca", "width": "15%" },
            {
                "data": "fechaCita",
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY hh:mm A');
                },
                "width": "20%"
            },
            {
                "data": "estado",
                "render": function (data) {
                    var badgeClass = '';
                    if (data === 'Ingresada') {
                        badgeClass = 'bg-primary';
                    } else if (data === 'Concluida') {
                        badgeClass = 'bg-success';
                    } else if (data === 'Cancelada') {
                        badgeClass = 'bg-danger';
                    }
                    return `<span class="badge ${badgeClass}">${data}</span>`;
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `<div class="text-center">
                                <a onclick="abrirModal(${data})" class='btn btn-warning btn-sm text-white me-2' style='cursor:pointer;'>
                                    <i class="fas fa-edit"></i> Editar
                                </a>
                                <a onclick="cambiarEstado(${data}, '${row.estado}')" class='btn btn-info btn-sm text-white me-2' style='cursor:pointer;'>
                                    <i class="fas fa-sync"></i> Estado
                                </a>
                                <a onclick="eliminarCita(${data})" class='btn btn-danger btn-sm text-white' style='cursor:pointer;'>
                                    <i class="fas fa-trash-alt"></i> Eliminar
                                </a>
                            </div>`;
                }, "width": "35%"
            }
        ],
        "language": {
            "emptyTable": "No hay citas de lavado registradas."
        },
        "order": [[2, "asc"]],
        "width": "100%"
    });
}

function abrirModal(id) {
    limpiarModal();
    if (id === 0) {
        $('#tituloModal').text('Registrar Nueva Cita');
    } else {
        $('#tituloModal').text('Editar Cita');
        var data = dataTable.rows().data().toArray().find(r => r.id === id);

        if (data) {
            $('#citaId').val(data.id);
            $('#clienteId').val(data.clienteId);
            $('#vehiculoId').val(data.vehiculoId);
            $('#estado').val(data.estado);

            var fecha = moment(data.fechaCita).format('YYYY-MM-DDTHH:mm');
            $('#fechaCita').val(fecha);
        }
    }
    $('#citaModal').modal('show');
}

function limpiarModal() {
    $('#citaId').val(0);
    $('#clienteId').val('');
    $('#vehiculoId').val('');
    $('#fechaCita').val('');
    $('#estado').val('Ingresada');
    $('#citaForm').find('.text-danger').text('');
}

function guardarCita() {
    var cita = {
        Id: parseInt($('#citaId').val()),
        ClienteId: parseInt($('#clienteId').val()),
        VehiculoId: parseInt($('#vehiculoId').val()),
        FechaCita: $('#fechaCita').val(),
        Estado: $('#estado').val()
    };

    if (isNaN(cita.ClienteId) || isNaN(cita.VehiculoId) || !cita.FechaCita) {
        showErrorAlert('Error de Validación', 'Debe completar el cliente, vehículo y fecha.');
        return;
    }

    $.ajax({
        url: '/Cita/Guardar',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(cita),
        success: function (response) {
            showSuccessAlert('Operación Exitosa', response.message);
            $('#citaModal').modal('hide');
            dataTable.ajax.reload();
        },
        error: function (xhr) {
            var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : "Error desconocido. Revise la fecha (debe ser futura).";
            showErrorAlert('Error de Negocio', errorMessage);
        }
    });
}

function eliminarCita(id) {
    showConfirmationAlert(
        '¿Está seguro de eliminar esta cita?',
        'Esta acción es irreversible.',
        function () {
            $.ajax({
                url: '/Cita/Eliminar/' + id,
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

function cambiarEstado(id, estadoActual) {
    Swal.fire({
        title: 'Cambiar Estado de Cita',
        text: `Cita #${id}. Estado actual: ${estadoActual}`,
        icon: 'info',
        input: 'select',
        inputOptions: {
            'Ingresada': 'Ingresada',
            'Cancelada': 'Cancelada',
            'Concluida': 'Concluida'
        },
        inputValue: estadoActual,
        showCancelButton: true,
        confirmButtonText: 'Guardar Estado',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var nuevoEstado = result.value;
            $.ajax({
                url: `/Cita/ActualizarEstado/${id}?nuevoEstado=${nuevoEstado}`,
                type: 'POST',
                success: function (response) {
                    showSuccessAlert('Estado Actualizado', `El estado de la cita #${id} ha cambiado a ${nuevoEstado}.`);
                    dataTable.ajax.reload();
                },
                error: function (xhr) {
                    var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : "Error al cambiar el estado.";
                    showErrorAlert('Error al Actualizar', errorMessage);
                }
            });
        }
    });
}
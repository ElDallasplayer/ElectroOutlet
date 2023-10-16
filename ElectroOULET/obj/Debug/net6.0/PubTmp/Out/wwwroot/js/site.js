
$("#SaveButton").on("click", function (event) {
    event.preventDefault(); //prevent default action 

    var post_url = $("#EditForm").attr("action"); //get form action url
    var form_data = $("#EditForm").serialize(); //Encode form elements for submission
    var array = post_url.split("/");

    $.post(array[2].toString(), form_data, function (response) {

        if (response.result == "OK") {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response.message,
                showConfirmButton: false,
                timer: 2000
            }).then((result) => {
                if (response.message == "Registro agregado correctamente") {
                    $("#ReparacionRealizada").val("");
                    $("#Trazabilidad").val("");
                    $("#Repuesto").val("");
                    $("#Repuesto").focus();
                }
            })
        } else {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: response.message,
                showConfirmButton: false,
                timer: 2000
            })
        }

    });
});

$("#CancelButton").on("click", function () {
    $("#ObjectEdit").remove();
    $("#EditBlur").remove();
    location.reload();
})

function StartLoader() {
    document.getElementById('loader-container-p').classList.remove('notVisible');
};

function StopLoader() {
    document.getElementById('loader-container-p').classList.add('notVisible');
};

function GuardadoCorrectamente(mensajeAMostrar) {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: mensajeAMostrar,
        showConfirmButton: false,
        timer: 2000
    })
}

function ButtonEliminar(e, url, userId) {
    StartLoader();
    e.preventDefault();
    e.stopImmediatePropagation();
    let id = e.target.getAttribute('id');

    Swal.fire({
        title: 'Seguro que deseas eliminar el registro?',
        showDenyButton: true,
        confirmButtonText: 'Eliminar',
        denyButtonText: `Cancelar`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            $.ajax({
                url: url + '?id=' + id + '&userId=' + userId,
                dataType: 'html',
                async: true,
                success: function (data) {
                    location.reload();
                }
            });
        }
    })
    StopLoader();
}
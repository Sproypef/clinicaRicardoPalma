//=== Valida fecha Inicio / Fin

$('#form_generar_servicio_salud').on('submit', function (e) {

    var fi = $("#fecha_inicio").val();//$.datepicker.formatDate("yyyy/mm/dd", $("#fecha_inicio").val());//$("#fecha_inicio").val();
    var ff = $("#fecha_fin").val();//$.datepicker.formatDate("yyyy/mm/dd", $("#fecha_fin").val());//$("#fecha_fin").val();

});


$('#submit_retirar').on('click', function (e) {
    $('#accion').attr('value', 'retirar');
    var min = 0;
    $("#table_retirar").find("input[type='checkbox']").each(function () {
        if ($(this).is(":checked")) {
            min++;
        }
    });
    if (min > 0) {
        $("#form_generar_accion").submit();
    }
    else {
        alert("Debe seleccionar almenos una fila");
        e.preventDefault();
    } 
});

$("#submit_asignar").on("click", function (e) {
    $("#accion").attr("value", "asignar");
    var min = 0;
    $("#table_asignar").find("input[type='checkbox']").each(function () {
        if ($(this).is(":checked")) {
            min++;
        }
    });
    if (min > 0) {
        $("#form_generar_accion").submit();
    }     
    else {
        alert("Debe asignar almenos una fila");
        e.preventDefault();
    }
        
});
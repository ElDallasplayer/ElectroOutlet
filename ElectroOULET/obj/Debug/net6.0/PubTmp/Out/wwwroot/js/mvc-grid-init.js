var getSelectedURL = null;
function mvcGridInit(_getSelectedURL) {
    getSelectedURL = _getSelectedURL;
    if ($('.mvc-grid'))
        setSelected($($('.mvc-grid').find("tbody").find("tr").first()), $($('.mvc-grid').find("tbody").find("tr").first()).find("td.hidden").html());
}

function setSelected(row, data, cb) {
    $(".seleccionado").removeClass("seleccionado");
    $(row).addClass("seleccionado");
    if (getSelectedURL) {
        $.getJSON(getSelectedURL + "?Id=" + data, function (item) {
            itemSeleccionado = item;
            setCalendar();
            loaded = true;
            if (cb)
                cb(item);
        });
    }
}

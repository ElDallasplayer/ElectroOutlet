// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#CancelButton").on("click", function () {
    $("#ObjectEdit").remove();
    $("#EditBlur").remove();
})

$(".LogOutButton").on("click", (e) => {
    e.stopPropagation();
    e.stopImmediatePropagation();

    let id = e.target.getAttribute("id");
    $.ajax({
        url: '@Url.Action("LogOut","Account")?id=' + id,
        type: "POST",
        success: function (e) {
            window.location.replace('@Url.Action("Login","Account")')
        }
    })
})

async function StartLoader() {
    if ($("#loader-container").hasClass("notVisible")) {
        $("#loader-container").removeClass("notVisible");
    };
}

async function StopLoader() {
    if ($("#loader-container").hasClass("notVisible")) {
        //NOTHING
    } else {
        $("#loader-container").addClass("notVisible");
    };
}

window.onclick = function (event) {
    var background = document.getElementById("popupBackground");

    if (event.target == background) {
        togglePopup("popupBackground", "seedInfoPopup");
    }
}

function togglePopup(popupBackgroundId, popupId, id) {
    $.get("/Seed/GetSeedInfo?seedId=" + id,
        function (data) {
            $('#seedInfo').html(data);
        });

    var background = document.getElementById(popupBackgroundId);
    background.classList.toggle("block");

    var popup = document.getElementById(popupId);
    popup.classList.toggle("visible");
}
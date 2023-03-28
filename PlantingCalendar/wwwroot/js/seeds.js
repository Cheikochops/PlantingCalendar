window.onclick = function (event) {
    var background = document.getElementById("popupBackground");

    if (event.target == background) {
        togglePopup("popupBackground", "seedInfoPopup");
    }
}
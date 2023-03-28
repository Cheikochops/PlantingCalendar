function togglePopup(popupBackgroundId, popupId) {
    var background = document.getElementById(popupBackgroundId);
    background.classList.toggle("block");

    var popup = document.getElementById(popupId);
    popup.classList.toggle("visible");
}

function calendarDropdown(calendarId) {

    if (calendarId == "new") {

    }
    else {
        window.location.href = "/Calendar?calendarId=" + calendarId;
    }
}
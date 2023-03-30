function calendarDropdown(calendarId) {

    if (calendarId == "new") {

    }
    else {
        window.location.href = "/Calendar?calendarId=" + calendarId;
    }
}
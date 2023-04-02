function calendarDropdown(calendarId) {

    if (calendarId == "new") {
        window.locaation.href = "/NewCalendar"
    }
    else {
        window.location.href = "/Calendar?calendarId=" + calendarId;
    }
}
angular.module('calendarApp', []);

function calendarDropdown(calendarId) {

    window.location.href = "/Calendar?calendarId=" + calendarId;
}
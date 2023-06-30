angular.module('calendarApp', []);
angular.module('newCalendarApp', []);

function calendarDropdown(calendarId) {

    window.location.href = "/Calendar?calendarId=" + calendarId;
}
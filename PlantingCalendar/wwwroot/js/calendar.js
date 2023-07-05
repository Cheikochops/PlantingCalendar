const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const calendarId = urlParams.get('calendarId')

angular.module('seedApp').controller('calendar', function ($scope, $http) {
    calendarUrl = "api/calendar"

    if (calendarId != null) {
        calendarUrl += "?id=" + calendarId;
    }

    $scope.displaySeedId = null;

    $scope.getCalendar = function () {
        $http.get(calendarUrl).then(
            function (response) {
                $scope.calendar = response.data;
                $scope.months = response.data.months;

                $scope.showSingleMonth = false;

                console.log($scope.calendar)
                console.log($scope.months)
            });
    }

    $scope.showMonth = function (month) {
        $scope.chosenMonth = $scope.months[month - 1]

        $scope.seedsByMonth = [];

        $scope.calendar.seeds.forEach(function (s) {

            days = []

            currentTasks = s.tasks[month];

            $scope.chosenMonth.days.forEach(function (d) {
                tasks = []

                currentTasks.forEach(function (x) {
                    if (x.taskDate != null && x.taskDate != undefined) {
                        if (new Date(x.taskDate).getUTCDate() == d.day) {
                            tasks.push({
                                isSet: true,
                                taskId: x.id,
                                taskName: x.taskName,
                                taskDescription: x.taskDescription,
                                isComplete: x.isComplete,
                                displayColour: x.displayColour,
                                dispayChar: x.dispayChar,
                                taskSetDate: new Date(x.taskDate)
                            })
                        }
                    }
                    else {
                        startDate = new Date(x.taskStartDate)
                        endDate = new Date(x.taskEndDate)
                        dayDate = new Date($scope.calendar.year, month-1, d.day)

                        if (startDate <= dayDate && endDate >= dayDate) {
                            tasks.push({
                                isSet: false,
                                taskId: x.id,
                                taskName: x.taskName,
                                taskDescription: x.taskDescription,
                                taskStartDate: startDate,
                                taskEndDate: endDate,
                                isComplete: x.isComplete,
                                displayColour: x.displayColour,
                                dispayChar: x.dispayChar
                            })
                        }
                    }
                })

                days.push({
                    day: d.day,
                    tasks: tasks
                })
            })

            $scope.seedsByMonth.push({
                plantId: s.id,
                plantBreed: s.plantBreed,
                plantTypeName: s.plantTypeName,
                dayTasks: days
            })
        });

        $scope.showSingleMonth = true;        
    }

    $scope.showCalendar = function () {
        $scope.showSingleMonth = false;
    }

    $scope.loadSeedInfoPopup = function (popupBackgroundId, popupId, id) {
        $scope.displaySeedId = id;
        $scope.togglePopup(popupBackgroundId, popupId);
    }

    $scope.loadNewTaskPopup = function (popupBackgroundId, popupId) {
        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.loadNewSeedPopup = function (popupBackgroundId, popupId) {
        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.loadTaskInfoPopup = function (popupBackgroundId, popupId, task) {
        $scope.editPopupTask = task;

        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.togglePopup = function(popupBackgroundId, popupId) {
        var background = document.getElementById(popupBackgroundId);
        background.classList.toggle("block");

        var popup = document.getElementById(popupId);
        popup.classList.toggle("visible");
    }

    $scope.refresh = function () {
        $scope.getCalendar();
    }

    window.onclick = function (event) {
        var background = document.getElementById("popupBackground");
        var newTaskPopupBackground = document.getElementById("newTaskPopupBackground");
        var editTaskPopupBackground = document.getElementById("editTaskPopupBackground");
        var newSeedPopupBackground = document.getElementById("newSeedPopupBackground");

        if (event.target == background) {
            $scope.togglePopup("popupBackground", "seedInfoPopup");
        }
        else if (event.target == newTaskPopupBackground) {
            $scope.togglePopup("newTaskPopupBackground", "newTaskPopup");
        }
        else if (event.target == editTaskPopupBackground) {
            $scope.togglePopup("editTaskPopupBackground", "editTaskPopup");
        }
        else if (event.target == newSeedPopupBackground) {
            $scope.togglePopup("newSeedPopupBackground", "newSeedPopup");
        }
    }

    $scope.getCalendar();
});
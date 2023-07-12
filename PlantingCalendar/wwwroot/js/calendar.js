const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const calendarId = urlParams.get('calendarId')

angular.module('seedApp').controller('calendar', function ($scope, $http) {
    calendarUrl = "api/calendar"

    if (calendarId != null) {
        calendarUrl += "?id=" + calendarId;
    }

    $scope.displaySeedId = null;
    $scope.refreshTrigger = false;

    $scope.getCalendar = function () {
        $http.get(calendarUrl).then(
            function (response) {
                $scope.calendar = response.data;
                $scope.months = response.data.months;

                $scope.showSingleMonth = false;

                console.log($scope.calendar)
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
                                taskTypeId: x.taskTypeId,
                                isRanged: x.isRanged,
                                taskId: x.id,
                                taskName: x.taskName,
                                taskDescription: x.taskDescription,
                                isComplete: x.isComplete,
                                displayColour: x.displayColour,
                                displayChar: x.displayChar,
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
                                taskTypeId: x.taskTypeId,
                                isRanged: x.isRanged,
                                taskId: x.id,
                                taskName: x.taskName,
                                taskDescription: x.taskDescription,
                                taskStartDate: startDate,
                                taskEndDate: endDate,
                                isComplete: x.isComplete,
                                displayColour: x.displayColour,
                                displayChar: x.displayChar
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

    $scope.loadSeedInfoPopup = function (id) {
        var popupBackgroundId = 'popupBackground';
        var popupId = 'seedInfoPopup';

        $scope.displaySeedId = id;
        $scope.togglePopup(popupBackgroundId, popupId);
    }

    $scope.loadNewTaskPopup = function () {
        var popupBackgroundId = 'newTaskPopupBackground';
        var popupId = 'newTaskPopup';

        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.refreshNewTask = function () {
        $scope.loadNewTaskPopup();
        $scope.refresh();
    }

    $scope.loadNewSeedPopup = function (popupBackgroundId, popupId) {
        var popupBackgroundId = 'newSeedPopupBackground';
        var popupId = 'newSeedPopup';

        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.refreshNewSeed = function () {
        $scope.loadNewSeedPopup();
        $scope.refresh();
    }

    $scope.loadTaskInfoPopup = function (task, seed) {
        var popupBackgroundId = 'editTaskPopupBackground';
        var popupId = 'editTaskPopup';

        $scope.editPopupTask = task;
        $scope.editPopupSeed = seed;
        $scope.togglePopup(popupBackgroundId, popupId)
    }

    $scope.refreshTaskInfo = function () {
        $scope.loadTaskInfoPopup();
        $scope.refresh();
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
            $scope.refreshTrigger = !$scope.refreshTrigger;
        }
        else if (event.target == newTaskPopupBackground) {
            $scope.togglePopup("newTaskPopupBackground", "newTaskPopup");
            $scope.refreshTrigger = !$scope.refreshTrigger;
        }
        else if (event.target == editTaskPopupBackground) {
            $scope.togglePopup("editTaskPopupBackground", "editTaskPopup");
            $scope.refreshTrigger = !$scope.refreshTrigger;
        }
        else if (event.target == newSeedPopupBackground) {
            $scope.togglePopup("newSeedPopupBackground", "newSeedPopup");
            $scope.refreshTrigger = !$scope.refreshTrigger;
        }
    }

    $scope.getCalendar();
});
//Searching features
var wto;

function debounceLoadSeedList(filter) {
    clearTimeout(wto);
    wto = setTimeout(function () {
        loadSeedList(filter)
    }, 750);
};

//

window.onclick = function (event) {
    var background = document.getElementById("popupBackground");

    if (event.target == background) {
        togglePopup("popupBackground", "seedInfoPopup");
    }
}

window.onload = function () {
    loadSeedList()
}

function loadPopup(popupBackgroundId, popupId, id) {

    var url = "/Seed/SeedInfo";

    if (id != null) {
        url += "?seedId=" + id;
    }

    $.get(url,
        function (data) {
            $('#seedInfo').html(data);
        });

    togglePopup(popupBackgroundId, popupId)
}

function togglePopup(popupBackgroundId, popupId) {
    var background = document.getElementById(popupBackgroundId);
    background.classList.toggle("block");

    var popup = document.getElementById(popupId);
    popup.classList.toggle("visible");
}

function orderBy(orderBy) {
    var filter = $("#searchBar").val()
    loadSeedList(filter, orderBy);
}

function loadSeedList(filter, orderBy) {

    var url = "/Seed/SeedsList";

    if (filter != null) {
        url += "?filter=" + filter;

        if (orderBy != null) {
            url += "&orderBy=" + orderBy;
        }
    }
    else if (orderBy != null) {
        url += "?orderBy=" + orderBy;
    }

    console.log(url)

    $.get(url,
        function (data) {
            $('#plantList').html(data);
        });
}
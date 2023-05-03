function deleteSeed(id) {
    $.ajax({
        url: "/Seed/SeedInfo?seedId=" + id,
        type: 'DELETE',
        success: function (result) {
            loadSeedList($("#searchBar").val())
            togglePopup("popupBackground", "seedInfoPopup")
        }
    });
} 
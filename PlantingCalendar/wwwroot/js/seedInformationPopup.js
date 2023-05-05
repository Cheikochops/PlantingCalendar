var rows = document.getElementById("actionTable").rows.length;
console.log(rows);

if (rows <= 2) {
    $("#noAction").show()
}
else {
    $("#noAction").hide()
}

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

function addRow() {
    $("#noAction").hide()

    $("#actionTable tr:first").after(
        '<tr class="actionItem"><td class="width4"><input id="actionType" class="width12"></td><td class="width1"><input id="displayChar" class="width12"></td><td class="width2"><input id="displayColour" class="width12"></td><td class="width2"><input id="startDate" class="width12"></td><td class="width2"><input id="endDate" class="width12"></td><td><button onclick="removeRow(this)" type="button" class="width12">X</button></td></tr>')
}

function removeRow(btn) {
    var row = btn.parentNode.parentNode;
    row.parentNode.removeChild(row);

    var rows = document.getElementById("actionTable").rows.length;
    console.log(rows);
    if (rows <= 2) {
        $("#noAction").show()
    }
}

function saveSeed(modelId) {
    var data = {
        Id: modelId,
        PlantType: $("#plantType").val(),
        Breed: $("#breed").val(),
        Description: $("#description").val(),
        SunRequirement: $("#sunRequirement").val(),
        WaterRequirement: $("#wateringRequirement").val(),
        ExpiryDate: $("#expiryDate").val(),
        Actions: []
    }

    $("#actionTable > tbody > tr.actionItem").each(function () {
        var currentRow = $(this); //Do not search the whole HTML tree twice, use a subtree instead
        console.log(currentRow.find("#actionType").val())
        data.Actions.push({
            ActionId: currentRow.find("#actionId").val(),
            ActionType: currentRow.find("#actionType").val(),
            DisplayChar: currentRow.find("#displayChar").val(),
            DisplayColour: currentRow.find("#displayColour").val(),
            StartDate: currentRow.find("#startDate").val(),
            EndDate: currentRow.find("#endDate").val()
        });
    });

    console.log(data);

    $.ajax({
        url: "/Seed/SeedInfo",
        type: 'POST',
        data: data,
        success: function (result) {
            console.log("saved!")
        }
    });

}
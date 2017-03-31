function confirmDelete(urlToCall, modalElement) {
    $(modalElement).find('#deleteButton').on("click", function () {
        location.href = urlToCall;
    });
    $(modalElement).modal("show");
    return false;
}

function editApp(appId, editButton) {
    var viewClass = ".view-id-" + appId;
    var editClass = ".edit-id-" + appId;
    $(viewClass).css('visibility','collapse').hide();
    $(editClass).css('visibility','visible').show();
}

function cancelEditApp(appId) {
    var viewClass = ".view-id-" + appId;
    var editClass = ".edit-id-" + appId;
    $(viewClass).css('visibility', 'initial').show();
    $(editClass).css('visibility', 'initial').hide();
}

function SaveApp(appId) {
    //warning!! this relies heavily on the format of the page and any change to the structue of the html can break this.
    var editClass = ".edit-id-" + appId;
    var inputFields = $(editClass + '> input');
    var appObj = new Object();
    inputFields.each(function () {
        var name = $(this).attr('name');
        var val = $(this).attr('value');
        appObj[name] = val;
    });
    $.post("/Settings/SaveApp", appObj, function (result) {

    });
}
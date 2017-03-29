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
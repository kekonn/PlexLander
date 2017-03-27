function confirmDelete(urlToCall, modalElement) {
    $(modalElement).find('#deleteButton').on("click", function () {
        location.href = urlToCall;
    });
    $(modalElement).modal("show");
    return false;
};

function editApp(appId, editButton) {
    var viewClass = ".view-app-" + appId;
    var editClass = ".edit-app-" + appId;
    $(viewClass).fadeOut("fast");
    $(editClass).fadeIn("fast");
};

function cancelEditApp(appId) {
    var viewClass = ".view-app-" + appId;
    var editClass = ".edit-app-" + appId;
    $(editClass).fadeOut("fast");
    //TODO clean up input fields
    $(viewClass).fadeIn("fast");
};
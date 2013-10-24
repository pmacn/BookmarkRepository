window.bookmarkApp = window.bookmarkApp || {};

window.bookmarkApp.datacontext = (function () {
    var datacontext = {
        getBookmarks: getBookmarks,
        deleteBookmark: deleteBookmark,
        getUserToken: getUserToken
    };

    return datacontext;

    function getUserToken(observableToken) {
        ajaxRequest('get', 'account/token')
            .done(function (data) {
                observableToken(data);
            });
    }

    function getBookmarks(observableBookmarks, observableError) {
        return ajaxRequest('get', 'api/bookmarks')
            .done(function (data) {
                var mappedBookmarks = $.map(data, function (item) { return new datacontext.Bookmark(item); });
                observableBookmarks(mappedBookmarks);
            })
            .fail(function () {
                observableError('Unable to load bookmarks: ' + e);
            });
    }

    function deleteBookmark(observableBookmark) {
        return ajaxRequest('delete', '/api/bookmarks/' + observableBookmark().id);
    }

    function ajaxRequest(type, url, data, dataType) {
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            };
        }

        return $.ajax(url, options);
    }
})();
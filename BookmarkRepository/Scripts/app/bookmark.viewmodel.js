
window.bookmarkApp.viewModel = (function bookmarksViewmodel(ko, datacontext) {
    var bookmarks = ko.observableArray([]),
        userToken = ko.observable(),
        baseUrl = window.location.href,
        bookmarkletLink = ko.computed(function() {
            if (!userToken()) return null;
            
            return "javascript:(function(){var d=document,s=d.createElement('script');window.markingUrl='" + baseUrl + "api/bookmarks/?token=" + userToken() + "';s.setAttribute('src','" + baseUrl + "Scripts/app/save.js');s.setAttribute('type','text/javascript');s.setAttribute('charset','UTF-8');d.body.appendChild(s);})();"
        }),
        selectedBookmarkIndex = ko.observable(0),
        selectedBookmark = ko.computed(function() {
            if (bookmarks() && bookmarks().length > 0) {
                var index = selectedBookmarkIndex();
                return bookmarks()[index];
            }

            return null;
        }),
        deleteBookmark = function(observableBookmark) {
            datacontext.deleteBookmark(observableBookmark)
                .done(function () {
                    bookmarks.remove(observableBookmark());
                });
        },
        selectNextBookmark = function() {
            var index = selectedBookmarkIndex();
            if (bookmarks() && bookmarks().length > index + 1) {
                selectedBookmarkIndex(index + 1);
            }
        },
        selectPreviousBookmark = function() {
            var index = selectedBookmarkIndex();
            if (bookmarks() && bookmarks().length > 0 && index > 0) {
                selectedBookmarkIndex(index - 1);
            }
        },
        deleteSelectedBookmark = function () {
            if (selectedBookmark())
                deleteBookmark(selectedBookmark);
        },
        openSelectedBookmark = function () {
            if (selectedBookmark()) {
                window.open(selectedBookmark().url(), '_blank');
            }
        };

    datacontext.getBookmarks(bookmarks);
    datacontext.getUserToken(userToken);

    return {
        bookmarks: bookmarks,
        selectedBookmark: selectedBookmark,
        selectNextBookmark: selectNextBookmark,
        selectPreviousBookmark: selectPreviousBookmark,
        deleteSelectedBookmark: deleteSelectedBookmark,
        bookmarkletLink: bookmarkletLink,
        openSelectedBookmark: openSelectedBookmark
    };
})(ko, bookmarkApp.datacontext);

ko.applyBindings(window.bookmarkApp.viewModel);
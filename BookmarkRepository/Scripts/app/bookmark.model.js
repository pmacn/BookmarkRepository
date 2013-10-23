(function (ko, datacontext) {
    datacontext.Bookmark = bookmark;

    function bookmark(data) {
        var self = this;
        data = data || {};

        self.id = data.id || 0;
        self.url = ko.observable(data.url || '');
        self.name = ko.observable(data.name || '');
        self.errorMessage = ko.observable();
        self.tags = ko.observableArray(data.tags || []);

        self.toJson = function () { return ko.toJSON(self); };
    }
})(ko, bookmarkApp.datacontext);
(function (ko, viewModel) {
    Mousetrap.bind('down', function () {
        viewModel.selectNextBookmark();
        return false;
    });
    Mousetrap.bind('up', function () {
        viewModel.selectPreviousBookmark();
        return false;
    });
    Mousetrap.bind('del', function () {
        if (confirm('do you really want to delete this bookmark?\nthere\'s no turning back!')) {
            viewModel.deleteSelectedBookmark();
        }
        return false;
    });
    Mousetrap.bind('enter', function () {
        viewModel.openSelectedBookmark();
        return false;
    });
})(ko, window.bookmarkApp.viewModel);
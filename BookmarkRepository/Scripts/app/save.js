(function () {
    var HTTP_CREATED = 201;

    if (!window.markingUrl) {
        alert('Dude, your bookmarklet is broken.');
        return;
    }

    if (window.XDomainRequest) {
        openBookmarkWindow();
    } else {
        createBookmark(window.markingUrl, window.location, document.title);
    }

    function createBookmark(markingUrl, bookmarkUrl, bookmarkName) {
        createDialog('Saving your bookmark');

        var request = new XMLHttpRequest();
        request.open('post', markingUrl, true);
        request.setRequestHeader('Content-Type', 'application/json');
        request.onreadystatechange = handleReadyStateChange;
        request.send('{url:"' + bookmarkUrl + '", name:"' + bookmarkName + '"}');
    }

    function handleReadyStateChange(change) {
        var request = change.target;
        if (request.readyState == 4 && request.status == HTTP_CREATED) {
            setDialogMessage('Bookmark saved');
            window.setTimeout(function () {
                deleteDialog();
            }, 3000);
        }
    }

    function createDialog(message) {
        dialog = document.createElement('div');
        dialog.id = 'bookmarkletdialog';
        dialog.setAttribute('style', 'position:fixed;top:20px;left:20px;width:14em;line-height:4em;height:4.5em;background-color:#D3D3D3;border:solid 2px gray;font-weight:bold;font-family:"Segoe UI",Verdana,Helvetica,Sans-Serif;border-radius:0.5em;color:white;text-align:center;');
        dialog.textContent = dialog.innerText = message;
        document.body.appendChild(dialog);
    }

    function setDialogMessage(message) {
        var dialog = document.getElementById('bookmarkletdialog');
        dialog.textContent = dialog.innerText = message;
    }

    function deleteDialog() {
        var dialog = document.getElementById('bookmarkletdialog');
        dialog.parentNode.removeChild(dialog);
    }

    function openBookmarkWindow() {
        // todo: need to open a window and do the bookmark through that
        alert('While waiting for me to fix this, you might want to use a newer browser.');
    }
})();
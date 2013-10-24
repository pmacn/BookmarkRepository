(function () {
    var HTTP_CREATED = 201;

    if (!window.markingUrl) {
        alert('Dude, your bookmarklet is broken.');
        return;
    }

    if (window.XDomainRequest) {
        openBookmarkWindow();
    } else {
        createBookmark(window.bookmarkToken, window.location, document.title);
    }

    function createBookmark(token, bookmarkUrl, bookmarkName) {
        var myDiv = document.createElement('div');
        myDiv.setAttribute('style', 'position:absolute;top:0px;width:100%;margin-top:100px;');
        myDiv.innerText = 'Saving your bookmark...';
        document.body.appendChild(myDiv);

        var request = new XMLHttpRequest();
        request.open('post', window.markingUrl, true);
        request.setRequestHeader('Content-Type', 'application/json');
        request.onreadystatechange = function () {
            if (request.readyState = 4 && request.status == HTTP_CREATED) {
                myDiv.innerText = 'Bookmark saved';
                window.setTimeout(function () {
                    if (myDiv.parentNode)
                        document.body.removeChild(myDiv);
                }, 3000);
            }
        };

        request.send('{url:"' + bookmarkUrl + '", name:"' + bookmarkName + '"}');
    }

    function openBookmarkWindow() {
        alert('While waiting for me to fix this, you might want to use a newer browser.');
    }
})();
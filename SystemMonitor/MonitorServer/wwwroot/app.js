window.onload = function () {
    document.body.addEventListener('htmx:beforeSwap', function (evt) {

        console.log('beforeSwap')
        if (evt.detail.xhr.status === 404) {
            // alert the user when a 404 occurs (maybe use a nicer mechanism than alert())
        } else if (evt.detail.xhr.status === 422) {
            evt.detail.shouldSwap = true;
            // set isError to false to avoid error logging in console
            evt.detail.isError = false;
            evt.detail.target = htmx.find("#message");
        } 
    });
}
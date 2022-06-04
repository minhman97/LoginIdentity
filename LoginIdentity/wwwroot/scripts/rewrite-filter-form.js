var RewriteFilterForm = function ($filterForm, callback) {
    this.init = function () {
        $filterForm.on('submit', function () {
            var query = $(this).serialize().replace(/%20/g, '+')

            var newUrl = window.location.href.split(/[?#]/)[0] + (query ? '?' + query : '');
            if (history.replaceState) {
                history.replaceState({}, 'list', newUrl);
                $.ajax({
                    url: newUrl,
                    data: {'_': new Date().getTime()},
                    method: 'GET',
                    success: function (data) {
                        callback(data);
                    }
                })
                return false;
            }
        })
    }
}
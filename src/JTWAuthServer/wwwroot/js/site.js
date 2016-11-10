
(function (angular, $) {
    var app = angular.module('app', []);
    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.post["Content-Type"] = "application/x-www-form-urlencoded";
        $httpProvider.defaults.transformRequest.unshift(function (data) {
            var key, result = [];
            if (typeof data === "string")
                return data;
            for (key in data) {
                if (data.hasOwnProperty(key))
                    result.push(encodeURIComponent(key) + "=" + encodeURIComponent(data[key]));
            }
            return result.join("&");
        });
    }]);

    app.controller('ClientManageController', ["$scope", "$http", function ($scope, $http) {
        var $pagination = $(".simple-pagination");
        var pagedItems = [];
        $scope.pagedItems = pagedItems;
        var param = {
            PageSize: 20,
            PageIndex: 0
        };
        $pagination.pagination({
            items: 0,
            itemsOnPage: param.PageSize,
            currentPage: param.PageIndex + 1,
            cssStyle: 'light-theme',
            onPageClick: function (pageNumber) {
                param.PageIndex = pageNumber - 1;
                paged();
            }
        });

        function paged() {
            //清理数据
            pagedItems.splice(0, pagedItems.length);
            $http.get("/client/paged", param)
                 .then(function (response) {
                     angular.forEach(response.data.data, function (item) {
                         pagedItems.push(item);
                     });

                     $pagination.pagination("updateItems", response.data.total);
                     var pages = $pagination.pagination("getPagesCount");
                     if (param.PageIndex + 1 > pages) {
                         $pagination.pagination("prevPage");
                     }
                     
                 });
        }

        //页面刷新
        $(document).on("PAGED_RELOAD", function () {
            paged();
        });
        //快速查询
        $(document).keydown(function (e) {
            if (e.keyCode === 27) {
                if (param) {
                    for (var key in param) {
                        if (param.hasOwnProperty(key)) {
                            if (key === "PageSize" || key === "PageIndex") {
                                continue;
                            }
                            param[key] = "";
                        }
                    }
                }
                paged();
                return false;
            } else if (e.keyCode === 13) {
                paged();
                return false;
            }
            return true;
        });

        paged();


    }]);


    app.controller('ClientDetailController', ["$scope", "$http", function ($scope, $http) {

    }]);
})(angular, jQuery);


$(function () {
    $(document).on("click", "[popup]", function () {
        if ($(this).attr("disabled") === "disabled") {
            return false;
        }
        var href = $(this).attr("data-href") || $(this).attr("href");
        var width = $(this).attr("data-width") || "90%";
        var height = $(this).attr("data-height") || "90%";
        //var title = $(this).attr("data-title") || $(this).attr("title");
        if ($.colorbox && href) {
            $.colorbox({
                href: href,
                width: width,
                height: height,
                iframe: true,
                overlayClose: false,
                escKey: false,
                onComplete: function () {
                    var win = $('.cboxIframe')[0].contentWindow;
                    setTimeout(function () {
                        $('#cboxTitle').html(win.document.title);
                    }, 200);
                }
            });
            return false;
        }
    });
});
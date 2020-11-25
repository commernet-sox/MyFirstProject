/// <reference path="jquery-3.1.1.intellisense.js" />
/// <reference path="jquery-3.1.1.js" />
(function($, dxbsDemo) {
    var search = {
        searchTimeout: null,
        lastSearch: null,
        selectedItem: null,

        onSearchBoxGotFocus: function(args) {
            if(search.listenerTimeout)
                clearTimeout(search.listenerTimeout);
            search.listenerTimeout = setInterval(function() {
                var text = args.sender.getValue();
                if(search.lastText !== text) {
                    search.lastText = text;
                    search.doSearch(text);
                }
            }, 700);
        },
        onSearchBoxLostFocus: function(args) {
            if(search.listenerTimeout)
                clearTimeout(search.listenerTimeout);
            search.listenerTimeout = null;
        },


        doSearch: function(text) {
            search.selectedItem = null;
            if(text && text.length > 2) {
                var pathBase = $(window.top.document.body).attr("data-pathBase");
                $.ajax({
                    url: pathBase + "/api/Search/" + text,
                    method: "GET",
                    success: function(searchResults){
                        search.updateSearchResultsContainer(text, searchResults);
                    }
                });
                search.setContainerVisiblity(true);
            }
            else
                search.setContainerVisiblity(false);
        },
        updateSearchResultsContainer: function(searchText, searchResults){
            var $container = $("#searchResults");
            $container.find("ul").remove();

            searchResults.length > 0 && $("#noResultsContainer").hide() || $("#noResultsContainer").show();

            if(searchResults.length === 0)
                $("#requestText").text(searchText);
            else {
                var $searchResults = search.createSearchResultsElement(searchResults);
                $container.append($searchResults);
            }
        },
        setContainerVisiblity: function(visible) {
            var $results = $("#searchResults");
            visible && $results.fadeIn() || $results.fadeOut();
        },
        createSearchResultsElement: function(searchResult){
            if(searchResult.length === 0)
                return null;

            var $ul = $("<ul></ul>");
            for(var i = 0; i < searchResult.length; i++){
                var result = searchResult[i];
                var $li = $("<li></li>");
                $li.append("<a href='" + result.url + "'>" + result.title + "</a>");
                if(result.hasAdditionalInfo){
                    $li
                        .append("<a href='" + result.groupUrl + "'>" + result.groupTitle + "</a>")
                        .append("<span class=\"icon icon-right_arr\"></span>")
                        .append("<a href='" + result.demoUrl + "'>" + result.demoTitle + "</a>");
                }
                $ul.append($li);
            }
            return $ul;
        },
        clear: function () {
            $("#searchResults").hide();
            searchEditor.setValue("");
        },
    };
    dxbsDemo.search = search;
})(jQuery, window.dxbsDemo || (window.dxbsDemo = {}));

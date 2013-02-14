Downplay_Prototypes_Lens_BindPage = function () {
    $("[data-lens]") // .not("[data-lens-bound]").attr("data-lens-bound", true)
        .each(function () {
            var lens = $(this);
            var textId = $(this).attr("data-lens-text-id");
            var hideId = $(this).attr("data-lens-hidden-id");
            var resultsId = $(this).attr("data-lens-results-id");
            var sendLoc = $(this).attr("data-lens-url");
            var child = $("#" + resultsId);
            var context = $(this).attr("data-lens-param-context");
            var params = "";
            $(this).keypress(function (key, e) {
                
            });
            // Build params string from data attributes
            $.each(this.attributes, function (i, attrib) {
                if (attrib.name.match(/^data-lens-param-/gi)) {
                    var paramName = attrib.name.substring("data-lens-param-".length);
                    // To camel case: http://stackoverflow.com/questions/6660977/convert-hyphens-to-camel-case-camelcase
                    var camelCased = paramName.replace(/-([a-z])/g, function (g) { return g[1].toUpperCase() });
                    params += "&" + camelCased + "=" + attrib.value;
                }
            });
            var text = $("#" + textId);
            var performSearch = function (me) {
                var search = me.value;
                child.show();
                $.ajax(sendLoc + "query=" + search + params, { success: function (data) {
                    child.html(data);
                    Downplay_Prototypes_Lens_BindResults(child, $("#" + hideId), context);
                }
                });

                switch (context) {
                    case "Socket":
                    case "Search":
                        text.focus(function () {
                            child.show();
                        }).blur(function () {
                            child.hide();
                        });
                        break;
                }
            };
            var timeoutId = null;
            text.keyup(function (ev) {
                if (timeoutId != null) {
                    clearTimeout(timeoutId);
                    timeoutId = null;
                }
                timeoutId = setTimeout(function () { performSearch(this); }, 300);
            });

        });
};
Downplay_Prototypes_Lens_BindResults = function (child, resultsField, context) {
    child.children().click(function () {
        switch (context) {
            case "Socket":
                var id = $(this).attr("data-lens-content-item-id");
                if (resultsField.attr("value") == "") {
                    resultsField.attr("value", id);
                }
                else {
                    resultsField.attr("value", resultsField.attr("value") + "," + id);
                }
                break;
            case "Search":
                // Redirect
                var url = $(this).attr("data-lens-view-url");
                document.location = url;
                break;
        }
    });
}
Downplay_Prototypes_Lens_BindPage();
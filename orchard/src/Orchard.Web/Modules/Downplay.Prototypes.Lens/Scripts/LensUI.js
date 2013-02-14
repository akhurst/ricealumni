(function() {

  $(function() {
    var ActivateItem, BindLens, BindResults, Parameterise, SelectItem;
    SelectItem = function(item, lens) {
      return item.addClass("selected").siblings().removeClass("selected");
    };
    ActivateItem = function(item, lens) {
      var id, resultsField, url;
      switch (lens.context) {
        case "Socket":
          id = item.attr("data-content-item-id");
          resultsField = $("#" + lens.hideId);
          resultsField.attr("value", resultsField.attr("value") ? resultsField.attr("value") + "," + id : id);
          return $.ajax(lens.socketLoc + "&newRightId=" + idsearch + lens.params, {
            success: function(data) {
              lens.child.html(data);
              return BindResults(lens);
            }
          });
        case "Search":
          url = item.find("header a").attr("href");
          return document.location = url;
      }
    };
    BindResults = function(lens) {
      if (lens.child.children().length) {
        lens.child.addClass("has-results");
      } else {
        lens.child.removeClass("has-results");
      }
      return lens.child.children().click(function() {
        return ActivateItem($(this), lens);
      });
    };
    Parameterise = function(a) {
      var camelCased, paramName;
      paramName = a.name.substring("data-lens-param-".length);
      camelCased = paramName.replace(/-([a-z])/g, function(c) {
        return c[1].toUpperCase();
      });
      return "&" + camelCased + "=" + a.value;
    };
    BindLens = function() {
      var attrib, i, lens, performSearch, timeoutId;
      lens = {
        textId: $(this).attr("data-lens-text-id"),
        hideId: $(this).attr("data-lens-hidden-id"),
        resultsId: $(this).attr("data-lens-results-id"),
        sendLoc: $(this).attr("data-lens-url"),
        context: $(this).attr("data-lens-param-context"),
        params: ((function() {
          var _i, _len, _ref, _results;
          _ref = this.attributes;
          _results = [];
          for (i = _i = 0, _len = _ref.length; _i < _len; i = ++_i) {
            attrib = _ref[i];
            if (attrib.name.match(/^data-lens-param-/gi)) {
              _results.push(Parameterise(attrib));
            }
          }
          return _results;
        }).call(this)).join()
      };
      lens.text = $("#" + lens.textId);
      lens.child = $("#" + lens.resultsId);
      if (lens.context === "Socket") {
        lens.socket = $(this).parents(".socket").first();
        lens.socketChild = lens.socket.children(".zone-content").children(".socket-content");
        lens.socketLoc = $(this).attr("data-lens-socket-url");
      }
      performSearch = function(me) {
        var search;
        search = me.value;
        lens.child.show();
        return $.ajax(lens.sendLoc + "query=" + search + lens.params, {
          success: function(data) {
            lens.child.html(data);
            return BindResults(lens);
          }
        });
      };
      switch (lens.context) {
        case "Socket":
        case "Search":
          lens.text.focus(function() {
            if (lens.child.hasClass("has-results")) {
              return lens.child.show();
            }
          }).blur(function() {
            var resultsHideFunc;
            resultsHideFunc = function() {
              return lens.child.hide();
            };
            return setTimeout(resultsHideFunc, 200);
          });
      }
      timeoutId = null;
      return lens.text.keydown(function(ev) {
        var next, prev, selection, timeoutFunc,
          _this = this;
        selection = lens.child.children(".selected");
        switch (ev.which) {
          case 13:
            if (selection.length) {
              ev.preventDefault();
              return ActivateItem(selection, lens);
            }
            break;
          case 38:
            if (!selection.length) {
              return SelectItem(lens.child.children().last(), lens);
            } else {
              prev = selection.prev();
              return SelectItem((prev.length ? prev : lens.child.children().last()), lens);
            }
            break;
          case 40:
            if (!selection.length) {
              return SelectItem(lens.child.children().first(), lens);
            } else {
              next = selection.next();
              return SelectItem((next.length ? next : lens.child.children().first()), lens);
            }
            break;
          default:
            if (timeoutId !== null) {
              clearTimeout(timeoutId);
              timeoutId = null;
            }
            timeoutFunc = function() {
              return performSearch(_this);
            };
            return timeoutId = setTimeout(timeoutFunc, 300);
        }
      });
    };
    return $("[data-lens]").each(BindLens);
  });

}).call(this);

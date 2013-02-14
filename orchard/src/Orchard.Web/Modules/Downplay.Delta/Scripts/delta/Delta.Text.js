(function() {
  var Delta, HtmlTextPrinter, _ref, _ref1,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  /*
  namespace Delta.Text
    Text and language processing functions
  */


  Delta.Text = (_ref1 = Delta.Text) != null ? _ref1 : {};

  /*
  class HtmlTextWriter 
    Converts text into HTML formats
  */


  Delta.Text.HtmlTextPrinter = HtmlTextPrinter = (function(_super) {

    __extends(HtmlTextPrinter, _super);

    HtmlTextPrinter.name = 'HtmlTextPrinter';

    function HtmlTextPrinter() {
      return HtmlTextPrinter.__super__.constructor.apply(this, arguments);
    }

    HtmlTextPrinter.prototype.Markdown = function() {
      if (!(this._Markdown != null)) {
        this._Markdown = window.markdown;
      }
      return this._Markdown;
    };

    HtmlTextPrinter.prototype.onPrint = function(print) {
      var filter, html;
      html = (function() {
        switch (print.Type) {
          case "Markdown":
            return this.Markdown().toHTML(print.Content);
          case "HTML":
            return print.Content;
        }
      }).call(this);
      filter = {
        Nodes: $(html),
        Cancel: false
      };
      this.Raise("HtmlInsertPreProcess", filter);
      if (!filter.Cancel) {
        this.$.append(filter.Nodes);
        return this.Raise("HtmlInsertPostProcess", filter);
      }
    };

    HtmlTextPrinter.prototype.onMessage = function(message) {};

    return HtmlTextPrinter;

  })(Delta.Html.ElementEntity);

  /*
  namespace Delta.Entities
  class BaseEntity
    method Text
      Extend base Entity with text processing
  */


  Delta.Entities.BaseEntity.prototype.Text = function(thing, context, qualifier) {
    var result, _ref2;
    if (context == null) {
      context = {};
    }
    if (!(context.Subject != null)) {
      context.Subject = this;
    }
    if (qualifier != null) {
      context.Qualifier = qualifier;
    }
    result = null;
    if (qualifier != null) {
      result = this._MakeText("text" + thing + "_" + qualifier, context);
    }
    if (!(result != null)) {
      result = this._MakeText("text" + thing, context);
    }
    if (!(result != null)) {
      result = (_ref2 = this.Parent) != null ? _ref2.Text(thing, context, qualifier) : void 0;
    }
    return result;
  };

  Delta.Entities.BaseEntity.prototype._MakeText = function(name, context) {
    var target;
    target = this[name];
    if (!(target != null)) {
      return null;
    }
    if (typeof target === "string") {
      return target;
    } else {
      return target.call(this, context);
    }
  };

  Delta.Entities.BaseEntity.prototype.ProcessText = function(thing, context, qualifier) {};

  Delta.Entities.BaseEntity.prototype.Write = function(thing, context, qualifier) {
    var result;
    if (context == null) {
      context = {};
    }
    result = this.Text(thing, context, qualifier);
    context.Content = result != null ? result : "Text not found: '" + thing + "' on '" + (thing === "Name" ? this.toString() : this.Write("Name", context)) + "'";
    context.Type = "Markdown";
    return this.Raise("Print", context);
  };

  Delta.Entities.BaseEntity.prototype.toString = function(context) {
    return this.Text("Name", context);
  };

  Delta.Entities.BaseEntity.prototype.textName = function(o) {
    var _ref2;
    return this.constructor.name + " " + ((_ref2 = this.Name) != null ? _ref2 : "#" + this.Id);
  };

}).call(this);

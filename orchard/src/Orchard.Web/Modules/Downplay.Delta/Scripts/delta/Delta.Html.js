(function() {
  var Delta, ElementEntity, InputElement, Logger, _ref, _ref1,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Html = (_ref1 = Delta.Html) != null ? _ref1 : {};

  /*
  An element entity is either bound to an existing element, or
  */


  Delta.Html.ElementEntity = ElementEntity = (function(_super) {

    __extends(ElementEntity, _super);

    ElementEntity.prototype.Element = null;

    function ElementEntity(element) {
      ElementEntity.__super__.constructor.call(this);
      if (element != null) {
        this.BindElement(element);
      }
    }

    ElementEntity.prototype.BindElement = function(element) {
      if (this.Element != null) {
        this.UnBindElement();
      }
      if (element instanceof jQuery) {
        this.$ = element;
        this.Element = this.$.get(0);
      } else {
        this.Element = element;
        this.$ = $(this.Element);
      }
      return this.Fire("Bound", {
        Element: element
      });
    };

    ElementEntity.prototype.UnBindElement = function() {
      if (this.Element != null) {
        return this.Fire("UnBound", {
          Element: this.Element
        });
      }
    };

    ElementEntity.prototype.onDestroy = function() {
      return this.UnBindElement();
    };

    return ElementEntity;

  })(Delta.Entities.BaseEntity);

  Delta.Html.InputElement = InputElement = (function(_super) {

    __extends(InputElement, _super);

    function InputElement() {
      return InputElement.__super__.constructor.apply(this, arguments);
    }

    InputElement.prototype.meBound = function(bound) {
      var _this = this;
      this.$.bind("change", function(e) {
        return typeof _this.Change === "function" ? _this.Change(e) : void 0;
      });
      return this.$.bind("click", function(e) {
        return typeof _this.Click === "function" ? _this.Click(e) : void 0;
      });
      /*
          @$.bind "change", (e)=>@Change(e)
          @$.bind "change", (e)=>@Change(e)
          @$.bind "change", (e)=>@Change(e)
      */

    };

    return InputElement;

  })(Delta.Html.ElementEntity);

  Delta.Utility.Logger = Logger = (function(_super) {

    __extends(Logger, _super);

    function Logger() {
      return Logger.__super__.constructor.apply(this, arguments);
    }

    Logger.prototype.LogContainer = null;

    Logger.prototype.onLog = function(log) {
      return typeof console !== "undefined" && console !== null ? console.log(log.Text) : void 0;
    };

    /*
      Cast: (name,data)->
        super
        if not @LogContainer?
          @LogContainer = $("#log > div")
        console?.log("<p><strong>#{name}</strong> #{ ("#{k}=#{v}" for k,v of data).join(", ") }</p>")
    */


    return Logger;

  })(Delta.Entity);

}).call(this);

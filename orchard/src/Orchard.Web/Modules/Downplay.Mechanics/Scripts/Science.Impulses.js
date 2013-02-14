
/*
Mechanics: Impulses
Science Project (http://scienceproject.codeplex.com)
P. Hurst 2011-2012
*/


(function() {
  var DeleteConnectorImpulseHandler, Delta, HtmlLoader, ImpulseHandler, ImpulseHijacker, Science, _ref, _ref1, _ref2,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Science = this.Science = (_ref1 = this.Science) != null ? _ref1 : {};

  Delta.Namespace("Science.Impulses");

  Delta.Require("Delta.Entities");

  /*
  TODO: Should be part of a Delta.Ajax library?
  */


  Science.Impulses.HtmlLoader = HtmlLoader = (function(_super) {

    __extends(HtmlLoader, _super);

    function HtmlLoader() {
      return HtmlLoader.__super__.constructor.apply(this, arguments);
    }

    return HtmlLoader;

  })(Delta.Entities.BaseEntity);

  /*
  Hijacks impulse links
  TODO: Would be nice to wrap this up using Delta to wire things
  */


  Science.Impulses.ImpulseHijacker = ImpulseHijacker = (function(_super) {

    __extends(ImpulseHijacker, _super);

    function ImpulseHijacker() {
      return ImpulseHijacker.__super__.constructor.apply(this, arguments);
    }

    ImpulseHijacker.prototype.onInitialize = function(init) {
      var me;
      me = this;
      $("a[data-impulse]").live("click", function(e) {
        return me.ImpulseClicked($(this), e);
      });
      $("a[data-impulse]").live("mousedown", function(e) {
        return me.ImpulseMouseDown($(this), e);
      });
      return $("a[data-impulse]").live("mouseup", function(e) {
        return me.ImpulseMouseUp($(this), e);
      });
    };

    ImpulseHijacker.prototype.ImpulseMouseDown = function($impulse, click) {};

    ImpulseHijacker.prototype.ImpulseMouseUp = function($impulse, click) {};

    ImpulseHijacker.prototype.ImpulseClicked = function($impulse, click) {
      var actuate, impulse;
      impulse = this.ImpulseFromEntity($impulse);
      actuate = this.Raise("ActuateImpulse", impulse);
      if (actuate.Actuated) {
        click.preventDefault();
        return click.stopPropagation();
      }
    };

    ImpulseHijacker.prototype.ImpulseFromEntity = function($impulse) {
      var impulse;
      impulse = {
        ImpulseName: $impulse.attr("data-impulse-name")
      };
      return impulse;
    };

    /*
      Triggered whenever HTML is loaded into DOM
    */


    ImpulseHijacker.prototype.onLoadedHtml = function(html) {
      var $html, me;
      $html = html.$;
      me = this;
      return $html.find("a[data-impulse]").each(function() {
        var $impulse;
        $impulse = $(this);
        return me.PrimeImpulse($impulse, $html);
      });
    };

    ImpulseHijacker.prototype.PrimeImpulse = function($impulse, $html) {};

    return ImpulseHijacker;

  })(Delta.Entities.BaseEntity);

  Science.Impulses.ImpulseHandler = ImpulseHandler = (function(_super) {

    __extends(ImpulseHandler, _super);

    function ImpulseHandler() {
      return ImpulseHandler.__super__.constructor.apply(this, arguments);
    }

    ImpulseHandler.prototype.ImpulseName = "";

    ImpulseHandler.prototype.onActuateImpulse = function(actuate) {
      if (actuate.ImpulseName === this.ImpulseName) {
        if (this.Actuate(actuate)) {
          actuate.Actuated = true;
        }
      }
      return actuate.Actuated;
    };

    ImpulseHandler.prototype.Actuate = function(actuate) {
      return false;
    };

    return ImpulseHandler;

  })(Delta.Entities.BaseEntity);

  Science.Impulses.Defaults = (_ref2 = Science.Impulses.Defaults) != null ? _ref2 : {};

  Science.Impulses.Defaults.DeleteConnectorImpulseHandler = DeleteConnectorImpulseHandler = (function(_super) {

    __extends(DeleteConnectorImpulseHandler, _super);

    function DeleteConnectorImpulseHandler() {
      return DeleteConnectorImpulseHandler.__super__.constructor.apply(this, arguments);
    }

    DeleteConnectorImpulseHandler.prototype.ImpulseName = "DeleteConnector";

    DeleteConnectorImpulseHandler.prototype.Actuate = function(actuate) {
      return false;
    };

    return DeleteConnectorImpulseHandler;

  })(Science.Impulses.ImpulseHandler);

  Delta.Ready(function() {
    var CloseMenu;
    CloseMenu = function() {
      $("[data-impulses-context-menu-open=true]").attr("data-impulses-context-menu-open", null);
      return $("[data-impulses-context-menu]").remove();
    };
    return $("*").live("click", function(e) {
      var impulseParent, menu;
      switch (e.which) {
        case 1:
          menu = $(this).parents("[data-impulses-context-menu]");
          if (menu.length) {
            e.preventDefault();
            return $.impulses.trigger(this);
          } else {
            return CloseMenu();
          }
          break;
        case 3:
          impulseParent = $(this).parents("[data-impulses]").first();
          if (impulseParent) {
            e.preventDefault();
            return impulseParent.attr("data-impulse-context-menu-open", "true");
          }
      }
    });
  });

}).call(this);

(function() {
  var Delta, InterfaceEntity, Prompt, TextPrompt, TouchInterface, _ref, _ref1,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  /*
  namespace Delta.Input
    User input classes
  */


  Delta.Input = (_ref1 = Delta.Input) != null ? _ref1 : {};

  /*
  class TouchInterface
    Hooks up to an element (usually a canvas) and captures both mouse and touch events to broadcast. Performs an inverse transformation
    based on the camera's transform so we get proper game coordinates out of the events.
    TODO: This really works far better as a meldable behaviour...
  */


  Delta.Input.TouchInterface = TouchInterface = (function(_super) {

    __extends(TouchInterface, _super);

    function TouchInterface() {
      return TouchInterface.__super__.constructor.apply(this, arguments);
    }

    TouchInterface.prototype.Finger = null;

    TouchInterface.prototype.FingerPosition = null;

    TouchInterface.prototype.Transform = new Delta.Math.Transform();

    TouchInterface.prototype.Touching = false;

    TouchInterface.prototype.Buttons = {
      Left: false,
      Right: false,
      Middle: false
    };

    /*
      Catch camera event to store an inverse transform
    */


    TouchInterface.prototype.onCameraChanged = function(camera) {
      return this.Transform = camera.Transform.Inverse();
    };

    TouchInterface.prototype.meBound = function(bound) {
      var _this = this;
      return this.$.bind('mousedown', function(ev) {
        return _this.MouseDown(ev);
      }).bind('mousemove', function(ev) {
        return _this.MouseMove(ev);
      }).bind('mouseup', function(ev) {
        return _this.MouseUp(ev);
      }).bind('touchstart', function(ev) {
        return _this.TouchDown(ev);
      }).bind('touchmove', function(ev) {
        return _this.TouchMove(ev);
      }).bind('touchend', function(ev) {
        return _this.TouchUp(ev);
      });
    };

    TouchInterface.prototype.TransformToWorld = function(vector) {
      return this.Transform.ApplyToVector(vector);
    };

    TouchInterface.prototype.CursorFromTouch = function(touch) {
      var client;
      client = new Vector(touch.clientX, touch.clientY);
      return {
        Client: client,
        World: this.TransformToWorld(client)
      };
    };

    TouchInterface.prototype.FireTouch = function(type, touch) {
      var t;
      return this.Raise("Mouse", {
        Origin: "Touch",
        Type: type,
        Cursors: (function() {
          var _i, _len, _ref2, _results;
          _ref2 = touch.originalEvent.touches;
          _results = [];
          for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
            t = _ref2[_i];
            _results.push(this.CursorFromTouch(t));
          }
          return _results;
        }).call(this)
      });
    };

    TouchInterface.prototype.FireMouse = function(type, mouse) {
      var button, client;
      button = type === "Move" ? null : ((function() {
        switch (mouse.which) {
          case 1:
            return "Left";
          case 2:
            return "Middle";
          case 3:
            return "Right";
          default:
            return null;
        }
      })());
      client = new Vector(mouse.clientX, mouse.clientY);
      if (button != null) {
        this.Buttons[button] = (type === "Up" ? false : true);
      }
      return this.Raise("Mouse", {
        Origin: "Mouse",
        Type: type,
        Cursors: [
          {
            Buttons: this.Buttons,
            Client: client,
            World: this.TransformToWorld(client)
          }
        ]
      });
    };

    TouchInterface.prototype.TouchDown = function(touch) {
      this.FireTouch("Down", touch);
      return touch.preventDefault();
    };

    TouchInterface.prototype.TouchMove = function(touch) {
      this.FireTouch("Move", touch);
      return touch.preventDefault();
    };

    TouchInterface.prototype.TouchUp = function(touch) {
      this.FireTouch("Up", touch);
      return touch.preventDefault();
    };

    TouchInterface.prototype.MouseDown = function(mouse) {
      this.FireMouse("Down", mouse);
      return mouse.preventDefault();
    };

    TouchInterface.prototype.MouseMove = function(mouse) {
      this.FireMouse("Move", mouse);
      return mouse.preventDefault();
    };

    TouchInterface.prototype.MouseUp = function(mouse) {
      this.FireMouse("Up", mouse);
      return mouse.preventDefault();
    };

    return TouchInterface;

  })(Delta.Html.ElementEntity);

  /*
  class InterfaceEntity
    Base class for HTML UI elements
  */


  Delta.Input.InterfaceEntity = InterfaceEntity = (function(_super) {

    __extends(InterfaceEntity, _super);

    function InterfaceEntity() {
      return InterfaceEntity.__super__.constructor.apply(this, arguments);
    }

    InterfaceEntity.prototype.TagName = "div";

    InterfaceEntity.prototype.Html = function(context) {
      var child, e, _i, _len, _ref2;
      this.Element = document.createElement(this.TagName);
      this.$ = $(this.Element);
      this.Fire("HtmlCreating", {
        $: this.$,
        Element: this.Element
      });
      _ref2 = this.Entities();
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        child = typeof e.Html === "function" ? e.Html(context) : void 0;
        if (child != null) {
          this.$.append(child);
        }
      }
      this.Fire("HtmlCreated", {
        $: this.$,
        Element: this.Element
      });
      return this.$;
    };

    return InterfaceEntity;

  })(Delta.Html.ElementEntity);

  Delta.Input.Prompt = Prompt = (function(_super) {

    __extends(Prompt, _super);

    function Prompt() {
      return Prompt.__super__.constructor.apply(this, arguments);
    }

    Prompt.prototype.textDescribe = function(o) {
      this.Text("PreAmble");
      +this.Text("Elements");
      return +this.Text("PostAmble");
    };

    Prompt.prototype.textElements = function(o) {
      var e;
      return ((function() {
        var _i, _len, _ref2, _results;
        _ref2 = this.Entities();
        _results = [];
        for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
          e = _ref2[_i];
          _results.push(e.Text("Describe"));
        }
        return _results;
      }).call(this)).join("\n");
    };

    Prompt.prototype.textPreAmble = function(o) {
      return "Prompt:";
    };

    Prompt.prototype.textPostAmble = function(o) {
      return "";
    };

    return Prompt;

  })(Delta.Input.InterfaceEntity);

  Delta.Input.TextPrompt = TextPrompt = (function(_super) {

    __extends(TextPrompt, _super);

    function TextPrompt() {
      return TextPrompt.__super__.constructor.apply(this, arguments);
    }

    TextPrompt.prototype.EnteredText = "";

    TextPrompt.prototype.meBuild = function(build) {
      var _this = this;
      this.Add(new TextElement("Text").Wire({
        Change: function(change) {
          return _this.EnteredText = change.Value;
        }
      }));
      return this.Add(new OkButton(function(click) {
        return _this.Fire("TextEntered", {
          Text: _this.EnteredText
        });
      }));
    };

    return TextPrompt;

  })(Prompt);

}).call(this);

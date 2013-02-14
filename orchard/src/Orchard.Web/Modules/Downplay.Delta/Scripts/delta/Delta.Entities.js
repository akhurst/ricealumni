
/*
namespace Delta.Entities

Entities provide support for a parent/child object hierarchy, and simple event triggering and handling. Global events can be Raised; local ones can be Fired,
and Cast is used to cascade events down to children (Raise performs the same action from the top of the tree). Methods named onEventName will handle cascading events,
meEventName handles locals; childEventName will catch events Fired from children.
*/


(function() {
  var ArtEntity, BaseEntity, Delta, DeltaEvent, Entity, Router, Vector, _ref, _ref1,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Entities = (_ref1 = Delta.Entities) != null ? _ref1 : {};

  Vector = Delta.Math.Vector;

  Delta.Entities.DeltaEvent = DeltaEvent = (function() {

    DeltaEvent.prototype._done = [];

    DeltaEvent.prototype._isDone = true;

    DeltaEvent.prototype._reject = [];

    function DeltaEvent(data) {
      var k, v;
      this._done = [];
      this._reject = [];
      for (k in data) {
        v = data[k];
        if (data.hasOwnProperty(k)) {
          this[k] = v;
        }
      }
    }

    DeltaEvent.prototype.done = function(f) {
      if (this._resolved) {
        f(this);
      } else {
        this._done.push(f);
      }
      return this;
    };

    DeltaEvent.prototype.fail = function(f) {
      if (this._rejected) {
        f(this);
      } else {
        this._done.push(f);
      }
      return this;
    };

    DeltaEvent.prototype.resolve = function() {
      var f, _i, _len, _ref2;
      if (!this._resolved) {
        this._resolved = true;
        _ref2 = this._done;
        for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
          f = _ref2[_i];
          f(this);
        }
      }
      return this;
    };

    DeltaEvent.prototype.isResolved = function() {
      return this._resolved;
    };

    DeltaEvent.prototype.reject = function() {
      var f, _i, _len, _ref2;
      if (!this._rejected) {
        this._rejected = true;
        _ref2 = this._reject;
        for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
          f = _ref2[_i];
          f(this);
        }
      }
      return this;
    };

    DeltaEvent.prototype.IsDeltaEvent = function() {
      return true;
    };

    return DeltaEvent;

  })();

  /*
  class BaseEntity
  
  Base entity support
  */


  Delta.Entities.BaseEntity = BaseEntity = (function() {

    BaseEntity.prototype._entities = null;

    BaseEntity.prototype.Parent = null;

    BaseEntity.prototype.IsInitialized = false;

    BaseEntity.prototype.IsDirty = true;

    BaseEntity.prototype.Root = null;

    BaseEntity.prototype._DeferredUntilStep = null;

    function BaseEntity() {
      this._DeferredUntilStep = [];
      this._entities = [];
      this._onCallbacks = {};
    }

    BaseEntity.prototype._onCallbacks = null;

    /*
      Register a callback for completion of an event
      TODO: Basically redundant now we have the promise-like interface
    */


    BaseEntity.prototype.on = function(name, callback) {
      if (!(this._onCallbacks[name] != null)) {
        this._onCallbacks[name] = [];
      }
      return this._onCallbacks[name].push(callback);
    };

    BaseEntity.prototype.Options = function() {
      var options;
      this.Raise("Options", options = {});
      return options;
    };

    BaseEntity.prototype.Entities = function(clone) {
      if (clone == null) {
        clone = true;
      }
      if (!(this._entities != null)) {
        alert("Constructor not called: " + this.constructor.name);
      }
      return this._entities;
    };

    BaseEntity.prototype._Event = function(data) {
      data = data != null ? data : {};
      if (!(typeof data.IsDeltaEvent === "function" ? data.IsDeltaEvent() : void 0)) {
        data = new DeltaEvent(data);
      }
      return data;
    };

    BaseEntity.prototype.Fire = function(name, data, qualifier) {
      var c, _i, _len, _ref2;
      data = this._Event(data);
      data.Source = this;
      this.Call(name, data, "me", this, qualifier);
      if (this._onCallbacks[name] != null) {
        _ref2 = this._onCallbacks[name];
        for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
          c = _ref2[_i];
          c(data);
        }
      }
      if ((this.Parent != null) && this.Parent !== this) {
        this.Call(name, data, "child", this.Parent, qualifier);
      }
      return data;
    };

    BaseEntity.prototype.Raise = function(name, data, qualifier) {
      data = this._Event(data);
      if (!(data.Source != null)) {
        data.Source = this;
      }
      if ((this.Root != null) && this.Root !== this) {
        this.Root.Raise(name, data, qualifier);
      } else if ((this.Parent != null) && this.Parent !== this) {
        this.Parent.Raise(name, data, qualifier);
      } else {
        this.Cast(name, data, qualifier);
      }
      return data;
    };

    BaseEntity.prototype.Call = function(name, data, type, target, qualifier) {
      var look, _name;
      if (type == null) {
        type = "on";
      }
      if (target == null) {
        target = this;
      }
      data = this._Event(data);
      if (qualifier != null) {
        look = type + name + "_" + qualifier;
        if (target[look] != null) {
          return typeof target[look] === "function" ? target[look](data) : void 0;
        }
      }
      if (target[type + name] != null) {
        if (typeof target[_name = type + name] === "function") {
          target[_name](data);
        }
      }
      return data;
    };

    BaseEntity.prototype.Cast = function(name, data, qualifier) {
      var e, _i, _len, _ref2;
      data = this._Event(data);
      if (typeof this.Call === "function") {
        this.Call(name, data, qualifier);
      }
      _ref2 = this.Entities();
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        if (typeof e.Cast === "function") {
          e.Cast(name, data, qualifier);
        }
      }
      return data;
    };

    BaseEntity.prototype.Add = function(entity) {
      var add, _ref2, _ref3;
      add = {
        Parent: this,
        Child: entity,
        Cancel: false
      };
      if ((_ref2 = this.Root) != null) {
        if (typeof _ref2.Attaching === "function") {
          _ref2.Attaching(add);
        }
      }
      this.Raise("Adding", add);
      if (typeof entity.Fire === "function") {
        entity.Fire("AddingTo", add);
      }
      if (add.Cancel) {
        if ((_ref3 = this.Root) != null) {
          if (typeof _ref3.Detaching === "function") {
            _ref3.Detaching(add);
          }
        }
        return false;
      }
      this.Entities(false).push(entity);
      this.Raise("Added", add);
      if (typeof entity.Fire === "function") {
        entity.Fire("AddedTo", add);
      }
      if (!((entity.IsInitialized != null) && entity.IsInitialized)) {
        if (typeof entity.Fire === "function") {
          entity.Fire("Init", {});
        }
        return entity.IsInitialized = true;
      }
    };

    BaseEntity.prototype.meAddedTo = function(add) {
      var _ref2;
      this.Parent = add.Parent;
      return this.Root = (_ref2 = this.Parent.Root) != null ? _ref2 : this.Parent;
    };

    /*    
    Remove: (ent)->
      # Remove entity from our list
      @Fire("Removed",{})
      @Destroy()
    */


    BaseEntity.prototype.Remove = function(entity) {
      var e, remove, _ref2;
      if (!(entity != null)) {
        return (_ref2 = this.Parent) != null ? _ref2.Remove(this) : void 0;
      }
      remove = {
        Child: entity,
        Parent: parent,
        Cancel: false
      };
      this.Fire("Removing", remove);
      entity.Fire("RemovingFrom", remove);
      if (!remove.Cancel) {
        this._entities = (function() {
          var _i, _len, _ref3, _results;
          _ref3 = this.Entities(true);
          _results = [];
          for (_i = 0, _len = _ref3.length; _i < _len; _i++) {
            e = _ref3[_i];
            if (e !== entity) {
              _results.push(e);
            }
          }
          return _results;
        }).call(this);
        this.Fire("Removed", remove);
        return entity.Fire("RemovedFrom", remove);
      }
    };

    BaseEntity.prototype.Clear = function() {
      var e, _i, _len, _ref2, _results;
      _ref2 = this.Entities();
      _results = [];
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        _results.push(this.Remove(e));
      }
      return _results;
    };

    BaseEntity.prototype.meRemovedFrom = function(removed) {
      if (this.Parent === removed.Parent) {
        return this.Parent = null;
      }
    };

    /*
      method Destroy
        Destructs the entity and removes from parent (and all child entities)
    */


    BaseEntity.prototype.Destroy = function() {
      var destroy;
      destroy = {
        Cancel: false
      };
      this.Cast("Destroying", destroy);
      if (!destroy.Cancel) {
        this.Cast("Destroy");
        return this.Remove();
      }
    };

    BaseEntity.prototype.onDestroy = function() {
      return this.Remove();
    };

    /*
      method DeferUntilStep
    
      Stores a closure to execute as soon as we get a Step event. Mainly needed when creating and destroying physics entities.
    
      TODO: Am thinking some sort of optional async processing for events might improve things
    */


    BaseEntity.prototype.DeferUntilStep = function(action) {
      return this._DeferredUntilStep.push(action);
    };

    BaseEntity.prototype.onStep = function(step) {
      var a, _i, _len, _ref2, _ref3;
      if ((_ref2 = this._DeferredUntilStep) != null ? _ref2.length : void 0) {
        _ref3 = this._DeferredUntilStep;
        for (_i = 0, _len = _ref3.length; _i < _len; _i++) {
          a = _ref3[_i];
          a();
        }
        return this._DeferredUntilStep = [];
      }
    };

    return BaseEntity;

  })();

  /*
  class Router
    Sits at the top of the entities tree and manages attached entities and event dispatching.
  */


  Delta.Entities.Router = Router = (function(_super) {

    __extends(Router, _super);

    function Router() {
      this._Attached = [];
      this._Events = {};
      this.Root = this;
      this.Parent = this;
      Router.__super__.constructor.apply(this, arguments);
    }

    Router.prototype.Attaching = function(add) {
      return this._AttachEntity(add.Child);
    };

    Router.prototype.Detaching = function(det) {
      return this._DetachEntity(det.Child);
    };

    Router.prototype.Raise = function(name, data, qualifier) {
      if (qualifier == null) {
        qualifier = "on";
      }
      data = this._Event(data);
      if (qualifier === "on") {
        return this._DispatchEvent(name, data);
      } else {
        return Router.__super__.Raise.call(this, name, data, qualifier);
      }
    };

    Router.prototype._EventsForEntity = function(entity, type) {
      var k, regex, v, _results;
      if (type == null) {
        type = "on";
      }
      regex = new RegExp("^" + type + "[A-Z0-9]");
      _results = [];
      for (k in entity) {
        v = entity[k];
        if (k.match(regex)) {
          _results.push(k.substring(type.length));
        }
      }
      return _results;
    };

    Router.prototype._AttachEntity = function(entity) {
      var e, _i, _len, _ref2, _results;
      _ref2 = this._EventsForEntity(entity);
      _results = [];
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        _results.push(this._SubscribeEvent(entity, e));
      }
      return _results;
    };

    Router.prototype._DetachEntity = function(entity) {
      var e, _i, _len, _ref2, _results;
      _ref2 = this._EventsForEntity(entity);
      _results = [];
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        _results.push(this._UnsubscribeEvent(entity, e));
      }
      return _results;
    };

    Router.prototype._SubscribeEvent = function(entity, name) {
      if (!(this._Events[name] != null)) {
        this._Events[name] = [];
      }
      return this._Events[name].push(entity);
    };

    Router.prototype._UnsubscribeEvent = function(entity, name) {
      var e, _i, _len, _ref2, _results;
      _ref2 = this._Events[name];
      _results = [];
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        e = _ref2[_i];
        if (e !== entity) {
          _results.push(this._Events[name] = e);
        }
      }
      return _results;
    };

    Router.prototype._DispatchEvent = function(name, event) {
      var e, _i, _len, _ref2, _results;
      if (this._Events[name] != null) {
        _ref2 = this._Events[name];
        _results = [];
        for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
          e = _ref2[_i];
          _results.push(e.Call(name, event));
        }
        return _results;
      }
    };

    Router.prototype.Initialize = function() {
      this.Raise("Initialize", {});
      return this.Raise("Initialized", {});
    };

    return Router;

  })(BaseEntity);

  /*
  class ArtEntity
    Entity with a visual component
    TODO: Should be in Delta.Graphics?
  */


  Delta.Entities.ArtEntity = ArtEntity = (function(_super) {

    __extends(ArtEntity, _super);

    function ArtEntity() {
      return ArtEntity.__super__.constructor.apply(this, arguments);
    }

    ArtEntity.prototype.Position = new Vector(0, 0);

    ArtEntity.prototype.Rotation = 0;

    ArtEntity.prototype.Size = new Vector(1, 1);

    ArtEntity.prototype.Origin = new Vector(0, 0);

    ArtEntity.prototype.IsVisible = true;

    ArtEntity.prototype.LayerName = "Main";

    ArtEntity.prototype.Art = null;

    ArtEntity.prototype.ArtName = "Main";

    ArtEntity.prototype.ArtFrame = 0;

    ArtEntity.prototype.SetPosition = function(pos) {
      var move;
      if (pos.x !== this.Position.x && pos.y !== this.Position.y) {
        move = {
          From: this.Position,
          To: pos
        };
        this.Fire("Moving", move);
        if (!move.Cancel) {
          this.Position = pos;
          return this.Fire("Moved", move);
        }
      }
    };

    ArtEntity.prototype.SetRotation = function(rot) {
      var rotate;
      if (this.Rotation !== rot) {
        rotate = {
          From: this.Rotation,
          To: rot
        };
        this.Fire("Rotating", rotate);
        if (!rotate.Cancel) {
          this.Rotation = rot;
          return this.Fire("Rotated", rotate);
        }
      }
    };

    ArtEntity.prototype.GetBounds = function() {
      var br, tl;
      br = this.Size.Multiply(0.5);
      tl = br.Multiply(-1);
      return new Delta.Math.Quad(tl.x, tl.y, br.x, br.y);
    };

    ArtEntity.prototype.onDraw = function(draw) {
      var bounds, frame, _ref2;
      if (!this.IsVisible) {
        return;
      }
      frame = (_ref2 = this.Art) != null ? _ref2.Frame(this.ArtName, this.ArtFrame) : void 0;
      bounds = this.GetBounds();
      if (frame != null) {
        return draw.Context.drawImage(frame, bounds.x1, bounds.y1, bounds.width(), bounds.height());
      } else {
        draw.Context.fillStyle = Delta.Math.Vector.Fill(0, 3).toRGB();
        return draw.Context.fillRect(bounds.x1, bounds.y1, bounds.width(), bounds.height());
      }
    };

    return ArtEntity;

  })(Delta.Entities.BaseEntity);

  Delta.Entity = Entity = (function(_super) {

    __extends(Entity, _super);

    function Entity() {
      return Entity.__super__.constructor.apply(this, arguments);
    }

    return Entity;

  })(Delta.Entities.ArtEntity);

}).call(this);

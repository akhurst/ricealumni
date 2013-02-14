
/*
Delta engine v0.2
(C)2012 P. Hurst, Downplay Design
*/


(function() {
  var Delta, cordova, _ref, _ref1, _ref2, _ref3, _ref4,
    _this = this;

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Graphics = (_ref1 = Delta.Graphics) != null ? _ref1 : {};

  Delta.Core = (_ref2 = Delta.Core) != null ? _ref2 : {};

  Delta.Utility = (_ref3 = Delta.Utility) != null ? _ref3 : {};

  Delta.Flags = (_ref4 = Delta.Flags) != null ? _ref4 : {};

  Delta.Flags.Cordova = navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry)/);

  Delta.Router = function() {
    if (!this._Router) {
      this._Router = new Delta.Entities.Router();
    }
    return this._Router;
  };

  Delta.Ready = function(callback) {
    Delta.Ready.Callbacks.push(callback);
    return Delta.Ready.Call();
  };

  Delta.Ready.Call = function() {
    var c, _i, _len, _ref5;
    if (!(Delta.Ready.Cordova != null)) {
      Delta.Ready.Cordova = !Delta.Flags.Cordova;
    }
    if (Delta.Ready.Cordova && Delta.Ready.jQuery) {
      _ref5 = Delta.Ready.Callbacks;
      for (_i = 0, _len = _ref5.length; _i < _len; _i++) {
        c = _ref5[_i];
        c();
      }
      Delta.Ready.Callbacks = [];
      return Delta.Router().Initialize();
    }
  };

  Delta.Ready.jQuery = false;

  Delta.Ready.Callbacks = [];

  $(function() {
    Delta.Ready.jQuery = true;
    return Delta.Ready.Call();
  });

  if (Delta.Flags.Cordova) {
    cordova = function() {
      Delta.Ready.Cordova = true;
      return Delta.Ready.Call();
    };
    document.addEventListener("deviceready", cordova, false);
  }

  Delta.Namespaces = {};

  Delta.Namespace = function(name) {
    var dot, root, rootNamespace, subname, _ref5, _ref6;
    if (!(Delta.Namespaces[name] != null)) {
      dot = name.lastIndexOf(".");
      if (dot >= 0) {
        rootNamespace = name.substring(0, dot);
        subname = name.substring(dot + 1);
        root = Delta.Namespace(rootNamespace);
        Delta.Namespaces[name] = root[subname] = (_ref5 = root[subname]) != null ? _ref5 : {};
      } else {
        Delta.Namespaces[name] = _this[name] = (_ref6 = _this[name]) != null ? _ref6 : {};
      }
    }
    return Delta.Namespaces[name];
  };

  Delta.Require = function(name) {};

  /*
  namespace Delta.Utility
    Some utility functions
  
  function Class
    Determine the class of an object
    Adapted from: http://stackoverflow.com/questions/1249531/how-to-get-a-javascript-objects-class
  */


  Delta.Utility.Class = function(o) {
    if (typeof o === "undefined") {
      return "undefined";
    }
    if (o === null) {
      return "null";
    }
    if ((Object.constructor.name != null) && Object.constructor.name !== "Function") {
      return Object.constructor.name;
    }
    return Object.prototype.toString.call(o).match(/^\[object\s(.*)\]$/)[1];
  };

  Delta.Dump = function(o) {
    var k, v;
    return typeof console !== "undefined" && console !== null ? console.log(((function() {
      var _results;
      _results = [];
      for (k in o) {
        v = o[k];
        _results.push(k + "=" + v);
      }
      return _results;
    })()).join(", ")) : void 0;
  };

  Delta.Meld = function(target, patch) {
    var con, i, key, pClass, setValue, tClass, value, _i, _j, _len, _len1, _results;
    if (typeof target === "function") {
      Delta.Extend(target, patch);
      return target;
    }
    if (typeof patch === "function") {
      con = patch;
      patch = new con();
    }
    _results = [];
    for (value = _i = 0, _len = patch.length; _i < _len; value = ++_i) {
      key = patch[value];
      tClass = Delta.Utility.Class(target[key]);
      pClass = Delta.Utility.Class(value);
      setValue = false;
      if (key.indexOf("on") === 0 && (target.on != null)) {
        target.on(key.substring(2), value);
      }
      if (key.indexOf("me") === 0 && (target.me != null)) {
        target.me(key.substring(2), value);
      } else if (pClass === "Object") {
        setValue = true;
      } else if (pClass === "Array") {
        if (tClass === "Array") {
          for (_j = 0, _len1 = value.length; _j < _len1; _j++) {
            i = value[_j];
            target[key].push(i);
          }
        } else {
          setValue = true;
        }
      } else {
        setValue = true;
      }
      if (setValue) {
        _results.push(target[key] = value);
      } else {
        _results.push(void 0);
      }
    }
    return _results;
  };

  Delta.Error = function(error) {
    if ((typeof console !== "undefined" && console !== null) && (console.log != null)) {
      return console.log(error);
    } else {
      return alert("Error: " + error);
    }
  };

  Delta.Configure = function(config, hub) {
    var i, instance, k, v, _i, _len, _ref5, _ref6, _results;
    if (!(hub != null)) {
      hub = Delta.Router();
    }
    _ref5 = config.Instances;
    _results = [];
    for (_i = 0, _len = _ref5.length; _i < _len; _i++) {
      i = _ref5[_i];
      if (!(Delta.Namespaces[i.Namespace] != null)) {
        Delta.Error("Can't find namespace " + i.Namespace + " to create instance of " + i.Type);
        continue;
      }
      if (!(Delta.Namespaces[i.Namespace][i.Type] != null)) {
        Delta.Error("Can't find type " + i.Type + " in namespace " + i.Namespace);
        continue;
      }
      instance = new Delta.Namespaces[i.Namespace][i.Type]();
      _ref6 = i.Properties;
      for (k in _ref6) {
        v = _ref6[k];
        instance[k] = v;
      }
      _results.push(hub.Add(instance));
    }
    return _results;
  };

}).call(this);

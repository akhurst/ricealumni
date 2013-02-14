(function() {
  var Delta, Matrix, Quad, Random, Transform, Vector, _ref, _ref1,
    __slice = [].slice;

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Math = (_ref1 = Delta.Math) != null ? _ref1 : {};

  Delta.Math.Vector = Vector = (function() {

    Vector.PropertyNames = ['x', 'y', 'z', 'u', 'v', 'w'];

    Vector.prototype.Values = [];

    Vector.prototype.Dimensions = 0;

    function Vector() {
      var values;
      values = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
      this.SetValues.apply(this, values);
    }

    Vector.prototype.toString = function() {
      return "<" + this.Values.join(", ") + ">";
    };

    Vector.prototype.SetValues = function() {
      var n, p, values, _i, _j, _len, _ref2, _ref3, _results;
      values = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
      if (values.length === 1 && values[0] instanceof Array) {
        values = values[0];
      }
      this.Values = values;
      this.Dimensions = this.Values.length;
      _ref2 = Delta.Math.Vector.PropertyNames;
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        p = _ref2[_i];
        this[p] = null;
      }
      _results = [];
      for (n = _j = 1, _ref3 = this.Dimensions; 1 <= _ref3 ? _j <= _ref3 : _j >= _ref3; n = 1 <= _ref3 ? ++_j : --_j) {
        _results.push(this[Delta.Math.Vector.PropertyNames[n - 1]] = this.Values[n - 1]);
      }
      return _results;
    };

    Vector.prototype.Multiply = function(factor) {
      var n, v;
      if (factor instanceof Matrix) {
        return new Vector(new Matrix(this.Values).Multiply(factor).Values[0]);
      } else if (factor instanceof Vector) {
        return new Vector((function() {
          var _i, _len, _ref2, _ref3, _results;
          _ref2 = this.Values;
          _results = [];
          for (n = _i = 0, _len = _ref2.length; _i < _len; n = ++_i) {
            v = _ref2[n];
            _results.push(v * ((_ref3 = factor.Values[n]) != null ? _ref3 : 1));
          }
          return _results;
        }).call(this));
      } else {
        return new Vector((function() {
          var _i, _len, _ref2, _results;
          _ref2 = this.Values;
          _results = [];
          for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
            v = _ref2[_i];
            _results.push(v * factor);
          }
          return _results;
        }).call(this));
      }
    };

    Vector.prototype.Add = function(factor) {
      var n, v;
      if (factor instanceof Delta.Math.Vector) {
        return new Vector((function() {
          var _i, _len, _ref2, _results;
          _ref2 = this.Values;
          _results = [];
          for (n = _i = 0, _len = _ref2.length; _i < _len; n = ++_i) {
            v = _ref2[n];
            _results.push(v + factor.Values[n]);
          }
          return _results;
        }).call(this));
      } else {
        return new Vector((function() {
          var _i, _len, _ref2, _results;
          _ref2 = this.Values;
          _results = [];
          for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
            v = _ref2[_i];
            _results.push(v + factor);
          }
          return _results;
        }).call(this));
      }
    };

    Vector.prototype.Subtract = function(factor) {
      var n, v, vec;
      if (factor instanceof Delta.Math.Vector) {
        return vec = new Vector((function() {
          var _i, _len, _ref2, _results;
          _ref2 = this.Values;
          _results = [];
          for (n = _i = 0, _len = _ref2.length; _i < _len; n = ++_i) {
            v = _ref2[n];
            _results.push(v - factor.Values[n]);
          }
          return _results;
        }).call(this));
      } else {
        return new Vector((function() {
          var _i, _len, _ref2, _results;
          _ref2 = this.Values;
          _results = [];
          for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
            v = _ref2[_i];
            _results.push(v - factor);
          }
          return _results;
        }).call(this));
      }
    };

    Vector.prototype.Normalize = function() {
      var length;
      length = this.Length();
      if (length === 0) {
        return this;
      }
      return this.Multiply(1 / length);
    };

    Vector.prototype.Length = function() {
      var total, v, _i, _len, _ref2;
      total = 0;
      _ref2 = this.Values;
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        v = _ref2[_i];
        total += v * v;
      }
      return Math.sqrt(total);
    };

    Vector.Fill = function(value, count) {
      var n;
      return new Vector((function() {
        var _i, _results;
        _results = [];
        for (n = _i = 1; 1 <= count ? _i <= count : _i >= count; n = 1 <= count ? ++_i : --_i) {
          _results.push(value);
        }
        return _results;
      })());
    };

    return Vector;

  })();

  Delta.Math.Matrix = Matrix = (function() {

    function Matrix() {}

    return Matrix;

  })();

  Delta.Math.Transform = Transform = (function() {

    function Transform() {}

    Transform.prototype.a = 1;

    Transform.prototype.b = 0;

    Transform.prototype.c = 0;

    Transform.prototype.d = 1;

    Transform.prototype.e = 0;

    Transform.prototype.f = 0;

    Transform.prototype.toString = function() {
      return [this.a, this.b, this.c, this.d, this.e, this.f].join(" ");
    };

    Transform.Identity = function() {
      return new Transform();
    };

    Transform.Translate = function(x, y) {
      return this.Identity().Translate(x, y);
    };

    Transform.Scale = function(x, y) {
      return this.Identity().Scale(x, y);
    };

    Transform.prototype.Inverse = function() {
      var t;
      t = new Transform();
      t.a = 1 / this.a;
      t.b = this.b;
      t.c = this.c;
      t.d = 1 / this.d;
      t.e = -this.e;
      t.f = -this.f;
      return t;
    };

    Transform.prototype.Translate = function(x, y) {
      var t;
      t = new Transform();
      t.a = this.a;
      t.b = this.b;
      t.c = this.c;
      t.d = this.d;
      t.e = this.e + x;
      t.f = this.f + y;
      return t;
    };

    Transform.prototype.Scale = function(x, y) {
      var t;
      t = new Transform();
      t.a = this.a * x;
      t.b = this.b;
      t.c = this.c;
      t.d = this.d * y;
      t.e = this.e;
      t.f = this.f;
      return t;
    };

    Transform.prototype.ApplyToCanvas = function(context) {
      return context.transform(this.a, this.b, this.c, this.d, this.e, this.f);
    };

    Transform.prototype.ApplyToVector = function(vector) {
      return new Vector(vector.x * this.a + this.e, vector.y * this.d + this.f);
    };

    return Transform;

  })();

  Delta.Math.Quad = Quad = (function() {

    function Quad(x1, y1, x2, y2) {
      var v1, v2, _ref2, _ref3;
      this.x1 = x1;
      this.y1 = y1;
      this.x2 = x2;
      this.y2 = y2;
      if (this.x1 instanceof Vector && this.y1 instanceof Vector) {
        _ref2 = [this.x1, this.y1], v1 = _ref2[0], v2 = _ref2[1];
        _ref3 = [v1.x, v1.y, v2.x, v2.y], this.x1 = _ref3[0], this.y1 = _ref3[1], this.x2 = _ref3[2], this.y2 = _ref3[3];
      }
    }

    Quad.prototype.width = function() {
      return this.x2 - this.x1;
    };

    Quad.prototype.height = function() {
      return this.y2 - this.y1;
    };

    Quad.prototype.Intersects = function(quad) {
      var missX, missY;
      if ((missX = (this.x1 < quad.x1 && this.x2 < quad.x1) || (this.x1 > quad.x2 && this.x2 > quad.x2))) {
        return false;
      }
      missY = (this.y1 < quad.y1 && this.y2 < quad.y1) || (this.y1 > quad.y2 && this.y2 > quad.y2);
      return !missY;
    };

    return Quad;

  })();

  Delta.Math.Random = Random = (function() {

    function Random() {}

    return Random;

  })();

}).call(this);

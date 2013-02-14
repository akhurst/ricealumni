
/*
Delta.Geometry

Defines functions relating to shape and spacial concerns
*/


(function() {
  var Delta, QuadShape, RegularPolygonShape, Shape, Vector, b2CircleShape, b2MassData, b2PolygonShape, _ref, _ref1, _ref2,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Geometry = (_ref1 = Delta.Geometry) != null ? _ref1 : {};

  Vector = Delta.Math.Vector;

  _ref2 = Box2D.Collision.Shapes, b2MassData = _ref2.b2MassData, b2PolygonShape = _ref2.b2PolygonShape, b2CircleShape = _ref2.b2CircleShape;

  Shape = Delta.Geometry.Shape = (function() {

    Shape.name = 'Shape';

    function Shape() {}

    Shape.prototype.Draw = function(draw) {
      return null;
    };

    Shape.prototype.Box2D = function() {
      return null;
    };

    return Shape;

  })();

  Delta.Geometry.CircleShape = (function(_super) {

    __extends(CircleShape, _super);

    CircleShape.name = 'CircleShape';

    CircleShape.prototype.Radius = 1;

    function CircleShape(radius) {
      if (radius != null) {
        this.Radius = radius;
      }
    }

    CircleShape.prototype.Draw = function(draw) {
      return draw.Circle(this.Radius);
    };

    CircleShape.prototype.DrawCanvas = function(canvas) {
      return canvas.arc(0, 0, this.Radius, 0, 2 * Math.PI, false);
    };

    CircleShape.prototype.Box2D = function() {
      return new b2CircleShape(this.Radius);
    };

    return CircleShape;

  })(Shape);

  Delta.Geometry.PolygonShape = (function(_super) {

    __extends(PolygonShape, _super);

    PolygonShape.name = 'PolygonShape';

    function PolygonShape() {
      return PolygonShape.__super__.constructor.apply(this, arguments);
    }

    PolygonShape.prototype.Draw = function(draw) {
      return draw.Path(this.GetVertices());
    };

    PolygonShape.prototype.DrawCanvas = function(canvas) {
      var n, path, _i, _ref3;
      path = this.GetVertices();
      canvas.moveTo(path[0].x, path[0].y);
      for (n = _i = 1, _ref3 = path.length - 1; 1 <= _ref3 ? _i <= _ref3 : _i >= _ref3; n = 1 <= _ref3 ? ++_i : --_i) {
        canvas.lineTo(path[n].x, path[n].y);
      }
      return canvas.lineTo(path[0].x, path[0].y);
    };

    PolygonShape.prototype.GetVertices = function() {
      if (!(this._cachedVertices != null)) {
        this._cachedVertices = this.MakeVertices();
      }
      return this._cachedVertices;
    };

    PolygonShape.prototype.MakeVertices = function() {
      return [];
    };

    PolygonShape.prototype.Box2D = function() {
      var shape, vertices;
      shape = new b2PolygonShape();
      vertices = this.GetVertices();
      shape.SetAsArray(vertices, vertices.length);
      return shape;
    };

    return PolygonShape;

  })(Shape);

  Delta.Geometry.QuadShape = QuadShape = (function(_super) {

    __extends(QuadShape, _super);

    QuadShape.name = 'QuadShape';

    function QuadShape(Bounds) {
      this.Bounds = Bounds;
    }

    QuadShape.prototype.MakeVertices = function() {
      return [new Vector(this.Bounds.x1, this.Bounds.y1), new Vector(this.Bounds.x1, this.Bounds.y2), new Vector(this.Bounds.x2, this.Bounds.y2), new Vector(this.Bounds.x2, this.Bounds.y1)];
    };

    return QuadShape;

  })(Delta.Geometry.PolygonShape);

  Delta.Geometry.RegularPolygonShape = RegularPolygonShape = (function(_super) {

    __extends(RegularPolygonShape, _super);

    RegularPolygonShape.name = 'RegularPolygonShape';

    function RegularPolygonShape(Radius, Faces) {
      this.Radius = Radius;
      this.Faces = Faces;
    }

    RegularPolygonShape.prototype.MakeVertices = function() {
      var n, _i, _ref3, _results;
      _results = [];
      for (n = _i = _ref3 = this.Faces - 1; _ref3 <= 0 ? _i <= 0 : _i >= 0; n = _ref3 <= 0 ? ++_i : --_i) {
        _results.push(new Vector(Math.sin(Math.PI * n * 2 / this.Faces), Math.cos(Math.PI * n * 2 / this.Faces)).Multiply(this.Radius));
      }
      return _results;
    };

    return RegularPolygonShape;

  })(Delta.Geometry.PolygonShape);

}).call(this);

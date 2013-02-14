var Art, Camera, ClunkyCamera, DeferredCanvas, Delta, ImageLoader, Layer, Style, TextureFill, Vector, _ref, _ref1,
  __hasProp = {}.hasOwnProperty,
  __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
  __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

Delta.Graphics = (_ref1 = Delta.Graphics) != null ? _ref1 : {};

Vector = Delta.Math.Vector;

Vector.prototype.toRGB = function(fac) {
  if (fac != null) {
    return this.Multiply(fac).toRGB();
  } else {
    return "rgb(" + (Math.floor(this.x * 255)) + "," + (Math.floor(this.y * 255)) + "," + (Math.floor(this.z * 255)) + ")";
  }
};

Vector.prototype.toRGBA = function(fac, a) {
  if (fac != null) {
    return this.Multiply(fac).toRGBA(null, a);
  } else {
    if (!(a != null)) {
      a = this.u;
    }
    return "rgb(" + (Math.floor(this.x * 255)) + "," + (Math.floor(this.y * 255)) + "," + (Math.floor(this.z * 255)) + "," + (Math.floor(a * 255)) + "))";
  }
};

Delta.Graphics.Layer = Layer = (function(_super) {

  __extends(Layer, _super);

  Layer.prototype.UnitMaxPixelSize = new Vector(20, 20);

  function Layer(Name, Canvas, Width, Height) {
    var _ref2, _ref3;
    this.Name = Name;
    this.Canvas = Canvas;
    this.Width = Width;
    this.Height = Height;
    this.Width = (_ref2 = this.Width) != null ? _ref2 : this.Canvas.width();
    this.Height = (_ref3 = this.Height) != null ? _ref3 : this.Canvas.height();
    Layer.__super__.constructor.call(this);
  }

  Layer.prototype.MakeCanvas = function() {
    if (!(this.Canvas != null)) {
      this.Canvas = $("<canvas width='{#@Width}' height='{#@Height}'/>");
    }
    this.Canvas.width = this.Width;
    this.Canvas.height = this.Height;
    this.$ = $(this.Canvas);
    return this.Context = this.Canvas.get(0).getContext("2d");
  };

  Layer.prototype.onCameraChanged = function(camera) {
    this.Transform = camera.Transform;
    if (!(this.Context != null)) {
      this.Width = camera.MaxBounds.width() * this.UnitMaxPixelSize.x;
      this.Height = camera.MaxBounds.height() * this.UnitMaxPixelSize.y;
      alert(this.Width);
      this.MakeCanvas();
    }
    this.$.width(this.Width * this.Transform.a);
    this.$.height(this.Height * this.Transform.d);
    return this.$.css({
      left: this.Transform.e * this.UnitMaxPixelSize.x + "px",
      top: this.Transform.f * this.UnitMaxPixelSize.y + "px"
    });
  };

  Layer.prototype.onAdding = function(add) {
    if ((add.Parent === this.Parent) && ((add.Child.LayerName != null) && (add.Child.LayerName === this.Name))) {
      add.Cancel = true;
      return this.Add(add.Child);
    }
  };

  Layer.prototype.childMoved = function(move) {
    var half;
    move.Source.IsDirty = true;
    if (!(move.Source.DirtyRectangle != null)) {
      half = move.Source.Size.Multiply(0.5);
      this.DirtyInBounds(move.Source.DirtyRectangle = new Delta.Math.Quad(move.From.Subtract(half), move.From.Add(half)));
    }
    return this.IsDirty = true;
  };

  Layer.prototype.childRotated = function(rotate) {
    var half;
    rotate.Source.IsDirty = true;
    if (!(rotate.Source.DirtyRectangle != null)) {
      half = rotate.Source.Size.Multiply(0.6);
      this.DirtyInBounds(rotate.Source.DirtyRectangle = new Delta.Math.Quad(rotate.Source.Position.Subtract(half), rotate.Source.Position.Add(half)));
    }
    return this.IsDirty = true;
  };

  Layer.prototype.DirtyInBounds = function(quad) {
    var bounds, e, half, _i, _len, _ref2, _results;
    _ref2 = this.Entities();
    _results = [];
    for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
      e = _ref2[_i];
      if ((e.Size != null) && (e.Position != null)) {
        half = e.Size.Multiply(0.5);
        bounds = new Delta.Math.Quad(e.Position.Subtract(half), e.Position.Add(half));
        if (bounds.Intersects(quad)) {
          _results.push(e.IsDirty = true);
        } else {
          _results.push(void 0);
        }
      } else {
        _results.push(void 0);
      }
    }
    return _results;
  };

  Layer.prototype.onRender = function(render) {
    var draw, e, _i, _j, _len, _len1, _ref2, _ref3;
    if (!this.IsDirty) {
      return false;
    }
    this.Context.save();
    this.IsDirty = false;
    draw = {
      Canvas: this.Canvas,
      Context: this.Context,
      Cancel: false
    };
    _ref2 = this.Entities();
    for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
      e = _ref2[_i];
      if (e.DirtyRectangle != null) {
        this.Context.clearRect(e.DirtyRectangle.x1, e.DirtyRectangle.y1, e.DirtyRectangle.width(), e.DirtyRectangle.height());
        e.DirtyRectangle = null;
      }
    }
    _ref3 = this.Entities();
    for (_j = 0, _len1 = _ref3.length; _j < _len1; _j++) {
      e = _ref3[_j];
      if (!e.IsDirty) {
        continue;
      }
      e.IsDirty = false;
      this.Context.save();
      if (e.Position != null) {
        this.Context.translate(e.Position.x, e.Position.y);
      }
      if (e.Rotation != null) {
        this.Context.rotate(e.Rotation);
      }
      if (typeof e.Cast === "function") {
        e.Cast("Drawing", draw);
      }
      if (!draw.Cancel) {
        if (typeof e.Cast === "function") {
          e.Cast("Draw", draw);
        }
        if (typeof e.Cast === "function") {
          e.Cast("Drawn", draw);
        }
      }
      this.Context.restore();
    }
    return this.Context.restore();
  };

  return Layer;

})(Delta.Entities.BaseEntity);

Delta.Graphics.Camera = Camera = (function(_super) {

  __extends(Camera, _super);

  Camera.prototype.MaxBounds = null;

  Camera.prototype.Current = null;

  Camera.prototype.Target = null;

  Camera.prototype.Delta = null;

  function Camera() {
    Camera.__super__.constructor.apply(this, arguments);
    this.Target = {
      Position: new Vector(0, 0),
      Scale: new Vector(1, 1),
      Rotation: 0
    };
    this.Current = {
      Position: new Vector(0, 0),
      Scale: new Vector(1, 1),
      Rotation: 0
    };
    this.Delta = {};
    this.MaxBounds = new Delta.Math.Quad(0, 0, 0, 0);
  }

  Camera.prototype.onAdded = function(added) {
    if (added.Child.Bounds != null) {
      if (added.Child.Bounds.x1 < this.MaxBounds.x1) {
        this.MaxBounds.x1 = added.Child.Bounds.x1;
      }
      if (added.Child.Bounds.y1 < this.MaxBounds.y1) {
        this.MaxBounds.y1 = added.Child.Bounds.y1;
      }
      if (added.Child.Bounds.x2 > this.MaxBounds.x2) {
        this.MaxBounds.x2 = added.Child.Bounds.x2;
      }
      if (added.Child.Bounds.y2 > this.MaxBounds.y2) {
        return this.MaxBounds.y2 = added.Child.Bounds.y2;
      }
    }
  };

  Camera.prototype.TargetToBounds = function() {};

  Camera.prototype.onStep = function(step) {};

  return Camera;

})(Delta.Entities.BaseEntity);

Delta.Graphics.ClunkyCamera = ClunkyCamera = (function(_super) {

  __extends(ClunkyCamera, _super);

  ClunkyCamera.prototype.MaxBounds = null;

  ClunkyCamera.prototype.MaxSurface = new Vector(1, 1);

  ClunkyCamera.prototype.Transform = null;

  function ClunkyCamera() {
    ClunkyCamera.__super__.constructor.apply(this, arguments);
    this.MaxBounds = new Delta.Math.Quad(0, 0, 0, 0);
  }

  ClunkyCamera.prototype.onAdded = function(added) {
    if (added.Child.Bounds != null) {
      if (added.Child.Bounds.x1 < this.MaxBounds.x1) {
        this.MaxBounds.x1 = added.Child.Bounds.x1;
      }
      if (added.Child.Bounds.y1 < this.MaxBounds.y1) {
        this.MaxBounds.y1 = added.Child.Bounds.y1;
      }
      if (added.Child.Bounds.x2 > this.MaxBounds.x2) {
        this.MaxBounds.x2 = added.Child.Bounds.x2;
      }
      if (added.Child.Bounds.y2 > this.MaxBounds.y2) {
        this.MaxBounds.y2 = added.Child.Bounds.y2;
      }
    }
    if (added.Child instanceof Layer) {
      return this.MaxSurface = new Vector(added.Child.Width, added.Child.Height);
    }
  };

  ClunkyCamera.prototype.TransformFromBounds = function(bounds) {
    var height, scaleFinal, scaleX, scaleY, width;
    width = bounds.x2 - bounds.x1;
    height = bounds.y2 - bounds.y1;
    scaleX = this.MaxSurface.x / width;
    scaleY = this.MaxSurface.y / height;
    scaleFinal = scaleX > scaleY ? (height * scaleX > this.MaxSurface.y ? scaleY : scaleX) : (width * scaleY > this.MaxSurface.x ? scaleX : scaleY);
    return Delta.Math.Transform.Translate(bounds.x1, bounds.y1).Scale(scaleFinal, scaleFinal);
  };

  ClunkyCamera.prototype.onSetup = function(setup) {
    return this.SetTransform(this.TransformFromBounds(this.MaxBounds));
  };

  ClunkyCamera.prototype.SetTransform = function(transform) {
    this.Transform = transform;
    return this.Raise("CameraChanged", {
      Transform: this.Transform,
      MaxBounds: this.MaxBounds,
      MaxScale: this.MaxScale
    });
  };

  ClunkyCamera.prototype.AnimateToTransform = function(transform) {
    return this.SetTransform(transform);
  };

  return ClunkyCamera;

})(Camera);

Delta.Graphics.Style = Style = (function() {

  function Style() {}

  return Style;

})();

Delta.Graphics.Art = Art = (function() {

  function Art() {
    this.Frames = {};
  }

  Art.prototype.Name = "base";

  Art.prototype.Size = new Vector(128, 128);

  Art.prototype.Frames = null;

  Art.prototype.FrameInfo = {
    Main: {
      Count: 1
    }
  };

  Art.prototype.Render = function(name, frameInfo, num) {
    var canvas, context;
    canvas = document.createElement("canvas");
    canvas.width = this.Size.x;
    canvas.height = this.Size.y;
    context = canvas.getContext("2d");
    if ((this["Draw" + name] != null)) {
      this["Draw" + name](context, num / frameInfo.Count);
    } else {
      this.Draw(context, name, num / frameInfo.Count);
    }
    return canvas;
  };

  Art.prototype.Draw = function(canvas, name, num) {
    return null;
  };

  Art.prototype.Frame = function(name, num) {
    var n;
    if (!(this.Frames[name] != null)) {
      if (!(this.FrameInfo[name] != null)) {
        alert("Frame '" + name + "' not found for " + (Delta.Utility.Class(this)) + " '" + this.Name + "'");
        return null;
      }
      this.Frames[name] = (function() {
        var _i, _ref2, _results;
        _results = [];
        for (n = _i = 1, _ref2 = this.FrameInfo[name].Count; 1 <= _ref2 ? _i <= _ref2 : _i >= _ref2; n = 1 <= _ref2 ? ++_i : --_i) {
          _results.push(null);
        }
        return _results;
      }).call(this);
    }
    if (!(this.Frames[name][num] != null)) {
      this.Frames[name][num] = this.Render(name, this.FrameInfo[name], num);
    }
    return this.Frames[name][num];
  };

  return Art;

})();

Delta.Graphics.DeferredCanvas = DeferredCanvas = (function() {

  function DeferredCanvas() {}

  return DeferredCanvas;

})();

Delta.Graphics.ImageLoader = ImageLoader = (function() {

  function ImageLoader(Url) {
    this.Url = Url;
    this.Loaded = __bind(this.Loaded, this);

    this.Image = new Image();
    this.Image.onload = this.Loaded;
    this.Image.src = this.Url;
  }

  ImageLoader.prototype.IsLoaded = false;

  ImageLoader.prototype.Loaded = function() {
    return this.IsLoaded = true;
  };

  ImageLoader.prototype.Filter = function(func) {
    var canvas, context, f, filtered, n, p, pixels, _i, _len;
    if (!this.IsLoaded) {
      return null;
    }
    canvas = document.createElement('canvas');
    canvas.width = this.Image.width;
    canvas.height = this.Image.height;
    context = canvas.getContext('2d');
    context.drawImage(this.Image, 0, 0);
    pixels = context.getImageData(0, 0, canvas.width, canvas.height);
    filtered = (function() {
      var _i, _len, _ref2, _results;
      _ref2 = (function() {
        var _j, _ref2, _results1;
        _results1 = [];
        for (n = _j = 0, _ref2 = (pixels.data.length - 1) / 4; 0 <= _ref2 ? _j <= _ref2 : _j >= _ref2; n = 0 <= _ref2 ? ++_j : --_j) {
          _results1.push([pixels.data[n * 4], pixels.data[n * 4 + 1], pixels.data[n * 4 + 2], pixels.data[n * 4 + 3]]);
        }
        return _results1;
      })();
      _results = [];
      for (_i = 0, _len = _ref2.length; _i < _len; _i++) {
        p = _ref2[_i];
        _results.push(func(new Vector(p[0], p[1], p[2], p[3])));
      }
      return _results;
    })();
    for (n = _i = 0, _len = filtered.length; _i < _len; n = ++_i) {
      f = filtered[n];
      pixels.data[n * 4] = Math.floor(f.x);
      pixels.data[n * 4 + 1] = Math.floor(f.y);
      pixels.data[n * 4 + 2] = Math.floor(f.z);
      pixels.data[n * 4 + 3] = Math.floor(f.u);
    }
    context.putImageData(pixels, 0, 0);
    return canvas;
  };

  return ImageLoader;

})();

Delta.Graphics.TextureFill = TextureFill = (function() {

  function TextureFill(Texture, Colour) {
    this.Texture = Texture;
    this.Colour = Colour;
    if (!(this.Colour instanceof Vector)) {
      this.Colour = new Vector(this.Colour);
    }
    if (typeof this.Texture === "string") {
      this.Texture = new ImageLoader(this.Texture);
    } else {
      this.Texture = {
        IsLoaded: true,
        Image: this.Texture
      };
    }
  }

  TextureFill.prototype.Fill = function(canvas, bounds) {
    var _ref2;
    if ((_ref2 = this.Texture) != null ? _ref2.IsLoaded : void 0) {
      return this.Texture.Image;
    } else {
      return this.Colour;
    }
    /*
        if bounds instanceof Delta.Math.Quad
          if @Texture?.IsLoaded
            canvas.drawImage(@Texture.Image,bounds.x1,bounds.y1,bounds.x2-bounds.x1,bounds.y2-bounds.y1)
          else
            canvas.fillStyle = "rgb(#{@Colour.x*255},#{@Colour.y*255},#{@Colour.z*255})"
            canvas.fillRect(bounds.x1,bounds.y1,bounds.x2-bounds.x1,bounds.y2-bounds.y1)
    */

  };

  return TextureFill;

})();

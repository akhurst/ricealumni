(function() {
  var Box2DCollisionListener, Box2DWorld, Delta, Physics, PhysicsEntity, b2AABB, b2Body, b2BodyDef, b2CircleShape, b2DebugDraw, b2Fixture, b2FixtureDef, b2MassData, b2PolygonShape, b2Vec2, b2World, k, v, _ref, _ref1, _ref2, _ref3,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; },
    __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Physics = (_ref1 = Delta.Physics) != null ? _ref1 : {};

  b2Vec2 = Box2D.Common.Math.b2Vec2;

  b2AABB = Box2D.Collision.b2AABB;

  _ref2 = Box2D.Dynamics, b2BodyDef = _ref2.b2BodyDef, b2Body = _ref2.b2Body, b2FixtureDef = _ref2.b2FixtureDef, b2Fixture = _ref2.b2Fixture, b2World = _ref2.b2World, b2DebugDraw = _ref2.b2DebugDraw;

  _ref3 = Box2D.Collision.Shapes, b2MassData = _ref3.b2MassData, b2PolygonShape = _ref3.b2PolygonShape, b2CircleShape = _ref3.b2CircleShape;

  /*
  Class: Delta.Math.Vector
  
  Function: toBox2D
  Converts this Vector to a b2Vec2 vector object.
  */


  Delta.Math.Vector.prototype.toBox2D = function() {
    return new b2Vec2(this.x, this.y);
  };

  b2Vec2.prototype.toVector = function() {
    return new Delta.Math.Vector(this.x, this.y);
  };

  /*
  Namespace: Delta.Physics
  */


  Delta.Physics.Box2DWorld = Box2DWorld = (function(_super) {

    __extends(Box2DWorld, _super);

    Box2DWorld.name = 'Box2DWorld';

    function Box2DWorld() {
      return Box2DWorld.__super__.constructor.apply(this, arguments);
    }

    Box2DWorld.prototype.StepSize = 1 / 120.0;

    Box2DWorld.prototype.StepPool = 0;

    Box2DWorld.prototype.VelocityIterations = 8;

    Box2DWorld.prototype.PositionIterations = 3;

    Box2DWorld.prototype.onSetup = function(setup) {
      this.World = new b2World(new b2Vec2(0, 0), true);
      return this.Raise("Box2DWorldCreated", {
        World: this.World
      });
    };

    Box2DWorld.prototype.onTick = function(tick) {
      var delta;
      this.StepPool += tick.Delta;
      delta = 0;
      while (this.StepPool > this.StepSize) {
        this.World.Step(this.StepSize, this.VelocityIterations, this.PositionIterations);
        this.World.ClearForces();
        this.StepPool -= this.StepSize;
        delta += this.StepSize;
      }
      return this.Raise("Step", {
        Delta: delta,
        World: this.World
      });
    };

    return Box2DWorld;

  })(Delta.Entities.BaseEntity);

  Delta.Physics.Box2DCollisionListener = Box2DCollisionListener = (function(_super) {

    __extends(Box2DCollisionListener, _super);

    Box2DCollisionListener.name = 'Box2DCollisionListener';

    function Box2DCollisionListener() {
      this.B2PostSolve = __bind(this.B2PostSolve, this);

      this.B2PreSolve = __bind(this.B2PreSolve, this);

      this.B2EndContact = __bind(this.B2EndContact, this);

      this.B2BeginContact = __bind(this.B2BeginContact, this);
      return Box2DCollisionListener.__super__.constructor.apply(this, arguments);
    }

    Box2DCollisionListener.prototype.onBox2DWorldCreated = function(b2d) {
      this._CollisionListener = new Box2D.Dynamics.b2ContactListener;
      this._CollisionListener.BeginContact = this.B2BeginContact;
      this._CollisionListener.EndContact = this.B2EndContact;
      this._CollisionListener.PostSolve = this.B2PostSolve;
      this._CollisionListener.PreSolve = this.B2PreSolve;
      return b2d.World.SetContactListener(this._CollisionListener);
    };

    Box2DCollisionListener.prototype.BuildCollideEvent = function(contact) {
      var collide, k, _i, _len, _ref4;
      collide = {
        Handled: false,
        Contact: contact,
        Cancel: false,
        A: {
          Fixture: contact.GetFixtureA()
        },
        B: {
          Fixture: contact.GetFixtureB()
        }
      };
      _ref4 = ['A', 'B'];
      for (_i = 0, _len = _ref4.length; _i < _len; _i++) {
        k = _ref4[_i];
        collide[k].Body = collide[k].Fixture.GetBody();
        collide[k].Entity = collide[k].Body.GetUserData();
      }
      return collide;
    };

    Box2DCollisionListener.prototype.FireCollideEvent = function(name, collide) {
      var _base, _base1, _ref4;
      if (typeof (_base = collide.A.Entity).Fire === "function") {
        _base.Fire(name, collide);
      }
      _ref4 = [collide.B, collide.A], collide.A = _ref4[0], collide.B = _ref4[1];
      return typeof (_base1 = collide.A.Entity).Fire === "function" ? _base1.Fire(name, collide) : void 0;
    };

    Box2DCollisionListener.prototype.B2BeginContact = function(contact) {
      var collide;
      collide = this.BuildCollideEvent(contact);
      this.FireCollideEvent("CollideBegin", collide);
      if (collide.Cancel) {
        return contact.SetEnabled(false);
      }
    };

    Box2DCollisionListener.prototype.B2EndContact = function(contact) {
      var collide;
      collide = this.BuildCollideEvent(contact);
      return this.FireCollideEvent("CollideEnd", collide);
    };

    /*
      Function: B2PreSolve
    
      Handles Box2D's "PreSolve" contact event. Fires a CollidePreSolve event on both colliding entities.
    
      Event: CollidePreSolve
        A.Entity:  The entity the event is called on
        B.Entity:  The other entity being collided with
        Contact:   The Box2D contact object
        Cancel:    Set true to make collision have no effect
    */


    Box2DCollisionListener.prototype.B2PreSolve = function(contact, lastManifold) {
      var collide;
      collide = this.BuildCollideEvent(contact);
      collide.LastManifold = lastManifold;
      this.FireCollideEvent("CollidePreSolve", collide);
      if (collide.Cancel) {
        return contact.SetEnabled(false);
      }
    };

    Box2DCollisionListener.prototype.B2PostSolve = function(contact, impulse) {
      var collide;
      collide = this.BuildCollideEvent(contact);
      collide.Impulses = impulse;
      return this.FireCollideEvent("CollidePostSolve", collide);
    };

    return Box2DCollisionListener;

  })(Delta.Entities.BaseEntity);

  Delta.Physics.PhysicsEntity = PhysicsEntity = (function(_super) {

    __extends(PhysicsEntity, _super);

    PhysicsEntity.name = 'PhysicsEntity';

    function PhysicsEntity() {
      return PhysicsEntity.__super__.constructor.apply(this, arguments);
    }

    PhysicsEntity.prototype.Body = null;

    PhysicsEntity.prototype.BodyType = "Dynamic";

    PhysicsEntity.prototype.IsAwake = false;

    PhysicsEntity.B2BodyTypes = {
      Dynamic: 2,
      Kinematic: 1,
      Static: 0
    };

    PhysicsEntity.prototype.BodyLinearDamping = 3.0;

    PhysicsEntity.prototype.BodyAngularDamping = 2.5;

    PhysicsEntity.prototype.FixtureDensity = 1.0;

    PhysicsEntity.prototype.FixtureFriction = 0.25;

    PhysicsEntity.prototype.FixtureRestitution = 0.7;

    PhysicsEntity.prototype.SetPosition = function(pos, ignore) {
      if (ignore == null) {
        ignore = false;
      }
      if (!ignore) {
        this.Body.SetPosition(pos.toBox2D());
      }
      return PhysicsEntity.__super__.SetPosition.call(this, pos);
    };

    PhysicsEntity.prototype.SetRotation = function(rot, ignore) {
      if (ignore == null) {
        ignore = false;
      }
      if (!ignore) {
        this.Body.SetAngle(rot);
      }
      return PhysicsEntity.__super__.SetRotation.call(this, rot);
    };

    PhysicsEntity.prototype.onBox2DWorldCreated = function(b2d) {
      this.World = b2d.World;
      return this.Build({
        World: this.World
      });
    };

    PhysicsEntity.prototype.Build = function(build) {
      var bodyDef;
      bodyDef = new b2BodyDef;
      bodyDef.type = Delta.Physics.PhysicsEntity.B2BodyTypes[this.BodyType];
      bodyDef.linearDamping = this.BodyLinearDamping;
      bodyDef.angularDamping = this.BodyAngularDamping;
      bodyDef.position = this.Position;
      bodyDef.userData = this;
      build = {
        BodyDef: bodyDef,
        Components: [],
        FixDefs: []
      };
      this.Fire("Building", build);
      this.Body = build.Body = this.Root.World.CreateBody(bodyDef);
      this.BuildComponents(build);
      return this.Fire("Built", build);
    };

    PhysicsEntity.prototype.BuildComponents = function(build) {
      var c, fixDef, k, subBuild, v, _i, _len, _ref4, _ref5, _ref6, _ref7, _ref8, _results;
      subBuild = {
        Body: build.Body,
        BodyDef: build.BodyDef,
        Components: []
      };
      this.Fire("BuildingComponents", subBuild);
      this.Fire("BuildComponents", subBuild);
      this.Fire("BuiltComponents", subBuild);
      this._PhysicsComponents = subBuild.Components;
      _ref4 = this._PhysicsComponents;
      _results = [];
      for (_i = 0, _len = _ref4.length; _i < _len; _i++) {
        c = _ref4[_i];
        fixDef = new b2FixtureDef;
        fixDef.density = (_ref5 = c.Density) != null ? _ref5 : this.FixtureDensity;
        fixDef.friction = (_ref6 = c.Friction) != null ? _ref6 : this.FixtureFriction;
        fixDef.restitution = (_ref7 = c.Restitution) != null ? _ref7 : this.FixtureRestitution;
        if (!(c.Shape != null)) {
          alert(((function() {
            var _results1;
            _results1 = [];
            for (k in c) {
              v = c[k];
              _results1.push(k + ": " + v);
            }
            return _results1;
          })()).join(", "));
        }
        fixDef.shape = c.Shape.Box2D();
        fixDef.isSensor = (_ref8 = c.IsSensor) != null ? _ref8 : false;
        build.FixDefs.push(fixDef);
        _results.push(this.Body.CreateFixture(fixDef));
      }
      return _results;
    };

    PhysicsEntity.prototype.meDestroy = function(destroy) {
      if (this.Body != null) {
        this.Root.World.DestroyBody(this.Body);
        return this.Body = null;
      }
    };

    PhysicsEntity.prototype.Impulse = function(force, position) {
      var _ref4;
      if (position == null) {
        position = this.Position;
      }
      return (_ref4 = this.Body) != null ? _ref4.ApplyImpulse(force, position.toBox2D()) : void 0;
    };

    PhysicsEntity.prototype.WakeUp = function() {
      if (!this.IsAwake) {
        this.IsAwake = true;
        return this.Fire("Wake", {});
      }
    };

    PhysicsEntity.prototype.Sleep = function() {
      if (this.IsAwake) {
        this.IsAwake = false;
        return this.Fire("Sleep", {});
      }
    };

    PhysicsEntity.prototype.onStep = function(step) {
      var pos, _ref4;
      if ((_ref4 = this.Body) != null ? _ref4.IsAwake() : void 0) {
        this.WakeUp();
        this.SetRotation(this.Body.GetAngle(), true);
        pos = this.Body.GetPosition();
        this.SetPosition(pos.toVector(), true);
      } else {
        if (this.IsAwake) {
          this.IsAwake = false;
        }
      }
      return PhysicsEntity.__super__.onStep.apply(this, arguments);
    };

    return PhysicsEntity;

  })(Delta.Entity);

  /*
  Raycast Helpers
  
  # TODO: It's almost impossible to see what's going on with these raycast functions ... needs some cleanup and documentation
  # TODO: And somehow they need packaging into more convenient entities and events ...
  */


  Physics = {
    RayCastNearest: function(world, origin, vector, filter) {
      var result;
      result = this._RayCast(world, origin, vector, function(hit, state) {
        if (!(state.Nearest != null) || state.Nearest.Fraction < hit.Fraction) {
          state.Nearest = hit;
        }
        return [hit.Fraction, filter != null ? filter(hit) : true];
      });
      return result.Nearest;
    },
    RayCastAll: function(world, origin, vector, filter) {
      var result;
      result = this._RayCast(world, origin, vector, function(hit, state) {
        return [1, filter != null ? filter(hit) : true];
      });
      return result.Hits;
    },
    _RayCast: function(world, origin, vector, callback) {
      var hits, rayCallback, state,
        _this = this;
      hits = [];
      state = {
        Hits: hits,
        Last: null
      };
      rayCallback = function(fixture, intersect, normal, fraction) {
        return _this._CastImpulseRay(fixture, intersect, normal, fraction, state, callback);
      };
      world.RayCast(rayCallback, origin, origin.Add(vector));
      return state;
    },
    _CastImpulseRay: function(fixture, intersect, normal, fraction, state, callback) {
      var entity, hit, next, store, _ref4;
      if (fixture.IsSensor()) {
        return 1;
      }
      entity = fixture.GetBody().GetUserData();
      if (!entity instanceof Delta.Entities.BaseEntity) {
        return 1;
      }
      hit = {
        Normal: normal,
        Fraction: fraction,
        Fixture: fixture,
        Intersect: intersect,
        Entity: entity
      };
      _ref4 = callback(hit, state), next = _ref4[0], store = _ref4[1];
      if (store) {
        state.Hits.push(hit);
      }
      return next;
    }
  };

  for (k in Physics) {
    v = Physics[k];
    Delta.Physics[k] = v;
  }

}).call(this);

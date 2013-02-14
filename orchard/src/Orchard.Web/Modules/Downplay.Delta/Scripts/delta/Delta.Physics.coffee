Delta = @Delta = @Delta ? {}
Delta.Physics = Delta.Physics ? {}

b2Vec2 = Box2D.Common.Math.b2Vec2
b2AABB = Box2D.Collision.b2AABB
{b2BodyDef, b2Body, b2FixtureDef, b2Fixture, b2World, b2DebugDraw} = Box2D.Dynamics
{b2MassData, b2PolygonShape, b2CircleShape} = Box2D.Collision.Shapes

###
Class: Delta.Math.Vector

Function: toBox2D
Converts this Vector to a b2Vec2 vector object.
###
Delta.Math.Vector::toBox2D = ()->
  new b2Vec2(@x,@y)

b2Vec2::toVector = ()->
  new Delta.Math.Vector(@x,@y)

###
Namespace: Delta.Physics

###
Delta.Physics.Box2DWorld = class Box2DWorld extends Delta.Entities.BaseEntity
  
  StepSize: 1/120.0
  StepPool: 0

  VelocityIterations: 8  # 10
  PositionIterations: 3  # 10

  onSetup: (setup)->
    @World = new b2World(new b2Vec2(0,0),true)
    @Raise("Box2DWorldCreated",{World:@World})

  onTick: (tick)->
    @StepPool += tick.Delta
    delta = 0
    while @StepPool > @StepSize
      @World.Step(@StepSize, @VelocityIterations, @PositionIterations)
      @World.ClearForces()
      @StepPool -= @StepSize
      delta += @StepSize
    # Raises a physics step event
    # TODO: As an experiment, running this after all steps are completed. This might help performance a little, and I can't reasonably think of why
    # we'd need more frequent updates of position etc. since we get notified of collisions anyway.
    @Raise("Step",{Delta: delta, World:@World})

Delta.Physics.Box2DCollisionListener = class Box2DCollisionListener extends Delta.Entities.BaseEntity

  onBox2DWorldCreated: (b2d)->
    # Set up listeners
    @_CollisionListener = new Box2D.Dynamics.b2ContactListener
    @_CollisionListener.BeginContact = @B2BeginContact
    @_CollisionListener.EndContact = @B2EndContact
    @_CollisionListener.PostSolve = @B2PostSolve
    @_CollisionListener.PreSolve = @B2PreSolve

    b2d.World.SetContactListener(@_CollisionListener)

  BuildCollideEvent: (contact)->
    collide =
      Handled: false
      Contact: contact
      Cancel: false
      A:
        Fixture: contact.GetFixtureA()
      B:
        Fixture: contact.GetFixtureB()
    for k in ['A','B']
      collide[k].Body = collide[k].Fixture.GetBody()
      collide[k].Entity = collide[k].Body.GetUserData()

    collide

  FireCollideEvent: (name, collide)->
    collide.A.Entity.Fire?(name,collide)
    # Switch and bait
    [collide.A,collide.B]=[collide.B,collide.A]
    # Fire on the other entity
    collide.A.Entity.Fire?(name,collide)

  B2BeginContact: (contact)=>
    collide = @BuildCollideEvent(contact)
    @FireCollideEvent("CollideBegin",collide)
    if collide.Cancel
      contact.SetEnabled(false)

  B2EndContact: (contact)=>
    collide = @BuildCollideEvent(contact)
    @FireCollideEvent("CollideEnd",collide)
    
  ###
  Function: B2PreSolve

  Handles Box2D's "PreSolve" contact event. Fires a CollidePreSolve event on both colliding entities.

  Event: CollidePreSolve
    A.Entity:  The entity the event is called on
    B.Entity:  The other entity being collided with
    Contact:   The Box2D contact object
    Cancel:    Set true to make collision have no effect
  ###
  B2PreSolve: (contact,lastManifold)=>
    collide = @BuildCollideEvent(contact)
    collide.LastManifold = lastManifold
    @FireCollideEvent("CollidePreSolve",collide)
    if collide.Cancel
      contact.SetEnabled(false)

  B2PostSolve: (contact, impulse)=>
    collide = @BuildCollideEvent(contact)
    collide.Impulses = impulse
    @FireCollideEvent("CollidePostSolve",collide)

Delta.Physics.PhysicsEntity = class PhysicsEntity extends Delta.Entity

  Body: null
  BodyType: "Dynamic"
  
  IsAwake: false

  @B2BodyTypes:
    Dynamic: 2
    Kinematic: 1
    Static: 0

  BodyLinearDamping: 3.0
  BodyAngularDamping: 2.5

  FixtureDensity: 1.0
  FixtureFriction: 0.25
  FixtureRestitution: 0.7

  SetPosition: (pos,ignore=false)->
    if !ignore
      # Change the physic body's position to match
      @Body.SetPosition(pos.toBox2D())
    super(pos)

  SetRotation: (rot,ignore=false)->
    if !ignore
      # Change the physic body's position to match
      @Body.SetAngle(rot)
    super(rot)
    
  onBox2DWorldCreated: (b2d)->
    # HACK: Need a somewhat better way to grab the world object (since this won't work for subsequently added entities)
    # Actually the best way would be to raise "BuildBody", "BuildFixture" etc. messages
    @World = b2d.World
    @Build({World:@World})

  Build: (build)->
    bodyDef = new b2BodyDef
    bodyDef.type = Delta.Physics.PhysicsEntity.B2BodyTypes[@BodyType]
    bodyDef.linearDamping = @BodyLinearDamping
    bodyDef.angularDamping = @BodyAngularDamping
    bodyDef.position = @Position
    bodyDef.userData = this

    build = 
      BodyDef: bodyDef
      Components: []
      FixDefs: []

    @Fire("Building",build)
    @Body = build.Body = @Root.World.CreateBody(bodyDef)
    @BuildComponents(build)
    @Fire("Built",build)

  BuildComponents: (build)->
    subBuild =
      Body: build.Body
      BodyDef: build.BodyDef
      Components: []

    @Fire("BuildingComponents",subBuild)
    @Fire("BuildComponents",subBuild)
    @Fire("BuiltComponents",subBuild)

    @_PhysicsComponents = subBuild.Components
    for c in @_PhysicsComponents
      # TODO: Fire an additional event so we can modify the fixture def
      fixDef = new b2FixtureDef
      fixDef.density = c.Density ? @FixtureDensity
      fixDef.friction = c.Friction ? @FixtureFriction
      fixDef.restitution = c.Restitution ? @FixtureRestitution
      if not c.Shape?
        alert (k+": "+v for k,v of c).join(", ")
      fixDef.shape = c.Shape.Box2D()
      fixDef.isSensor = c.IsSensor ? false
      build.FixDefs.push(fixDef)
      @Body.CreateFixture(fixDef)

  meDestroy: (destroy)->
    if @Body?
      @Root.World.DestroyBody(@Body)
      @Body = null

  Impulse: (force,position=@Position)->
    @Body?.ApplyImpulse(force,position.toBox2D())

  # Woken up
  WakeUp: ()->
    if !@IsAwake
      @IsAwake = true
      @Fire("Wake", {})

  # Gone to sleep
  Sleep: ()->
    if @IsAwake
      @IsAwake = false
      @Fire("Sleep", {})

  # Physics world step
  onStep: (step)->
    if @Body?.IsAwake()
      @WakeUp()
      @SetRotation(@Body.GetAngle(),true)
      pos = @Body.GetPosition()
      @SetPosition(pos.toVector(),true)
      # TODO: Layer will catch the move/rotate events to flag dirty ... altho we could just set it here:
      # @Parent.IsDirty = true
    else
      if @IsAwake
        @IsAwake = false
    super

###
Raycast Helpers

# TODO: It's almost impossible to see what's going on with these raycast functions ... needs some cleanup and documentation
# TODO: And somehow they need packaging into more convenient entities and events ...
###
Physics =
  RayCastNearest: (world,origin,vector,filter)->
    result = @_RayCast(world,origin,vector,(hit,state)->
      if not state.Nearest? or state.Nearest.Fraction<hit.Fraction
        state.Nearest = hit
      return [hit.Fraction,if filter? then filter(hit) else true])
    return result.Nearest

  RayCastAll: (world,origin,vector,filter)->
    result = @_RayCast(world,origin,vector,(hit,state)->
      return [1,if filter? then filter(hit) else true]
    )
    return result.Hits

  _RayCast: (world,origin,vector,callback)->
    hits = []
    state = 
      Hits: hits
      Last: null
    rayCallback = (fixture, intersect, normal, fraction) => 
      @_CastImpulseRay(fixture,intersect,normal,fraction,state,callback)
    world.RayCast(rayCallback, origin, origin.Add(vector))
    return state

  _CastImpulseRay: (fixture, intersect, normal, fraction, state, callback) ->
    # Ignore sensors
    if (fixture.IsSensor())
      return 1

    # Check attached Entity
    entity = fixture.GetBody().GetUserData()
    if not entity instanceof Delta.Entities.BaseEntity
      return 1

    hit = 
      Normal: normal
      Fraction: fraction
      Fixture: fixture
      Intersect: intersect
      Entity: entity

    [next, store] = callback(hit,state)

    if store
      state.Hits.push(hit)

    return next

# Append to namespace
Delta.Physics[k] = v for k,v of Physics
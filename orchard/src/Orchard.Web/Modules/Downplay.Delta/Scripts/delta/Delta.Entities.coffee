###
namespace Delta.Entities

Entities provide support for a parent/child object hierarchy, and simple event triggering and handling. Global events can be Raised; local ones can be Fired,
and Cast is used to cascade events down to children (Raise performs the same action from the top of the tree). Methods named onEventName will handle cascading events,
meEventName handles locals; childEventName will catch events Fired from children.
###

Delta = @Delta = @Delta ? {}
Delta.Entities = Delta.Entities ? {}

Vector = Delta.Math.Vector

Delta.Entities.DeltaEvent = class DeltaEvent
  
  _done: []
  _isDone: true
  _reject: []

  constructor: (data)->
    @_done = []
    @_reject = []
    for k,v of data
      if data.hasOwnProperty(k)
        @[k] = v

  done: (f)->
    if @_resolved
      f(@)
    else
      @_done.push(f)
    @

  fail: (f)->
    if @_rejected
      f(@)
    else
      @_done.push(f)
    @

  resolve: ()->
    if !@_resolved
      @_resolved = true
      f(@) for f in @_done
    @

  isResolved: ()->@_resolved

  reject: ()->
    if !@_rejected
      @_rejected = true
      f(@) for f in @_reject
    @

  IsDeltaEvent: ()->true

###
class BaseEntity

Base entity support
###
Delta.Entities.BaseEntity = class BaseEntity
  _entities: null
  Parent: null
  IsInitialized: false
  IsDirty: true
  Root: null

  _DeferredUntilStep: null

  constructor: ()->
    @_DeferredUntilStep = []
    @_entities = []
    @_onCallbacks = {}

  _onCallbacks: null

  ###
  Register a callback for completion of an event
  TODO: Basically redundant now we have the promise-like interface
  ###
  on: (name,callback)->
    if !@_onCallbacks[name]?
      @_onCallbacks[name] = []
    @_onCallbacks[name].push(callback)

  Options: ()->
    @Raise("Options",options = {})
    options

  Entities: (clone = true)->
    if not @_entities?
      alert "Constructor not called: " + @constructor.name
    # TODO: Probably kinda slow doing an array copy, but it saves a lot of brainache; ultimately when I switch round the event registrations we'll need something better anyway
    # if clone then @_entities.slice() else @_entities
    @_entities

  _Event: (data)->
    data = data ? {}
    if !data.IsDeltaEvent?()
      data = new DeltaEvent(data)
    data
    
  Fire: (name,data,qualifier)->
    data = @_Event(data)
    data.Source = this

    @Call(name,data,"me",this,qualifier)
    if @_onCallbacks[name]?
      c(data) for c in @_onCallbacks[name]
    # Fire child events on Parent
    if @Parent? and @Parent isnt this
      @Call(name,data,"child",@Parent,qualifier)
    data
  
  Raise: (name,data,qualifier)->
    data = @_Event(data)
    if not data.Source? then data.Source = this
    if @Root? and @Root != this
      @Root.Raise(name,data,qualifier)
    else if @Parent? and @Parent != this
      @Parent.Raise(name,data,qualifier)
    else
      @Cast(name,data,qualifier)
    data

  Call: (name,data,type="on",target=@,qualifier)->
    # if target.Removed then return
    data = @_Event(data)
    # try
    if qualifier?
      look = type+name+"_"+qualifier
      if target[look]?
        return target[look]?(data)
    if target[type+name]?
      target[type+name]?(data)
    data
    # catch error
    #  @Raise("Log",{ Text: "Error in #{@constructor.name} calling #{type+name}: #{error}" })

  Cast: (name,data,qualifier)->
    # if @Removed then return
    data = @_Event(data)
    # Bubble to children
    @Call?(name,data,qualifier)
    e.Cast?(name,data,qualifier) for e in @Entities()
    data

  Add: (entity)->
    add =
      Parent: @
      Child: entity
      Cancel: false
    @Root?.Attaching?(add)
    @Raise("Adding",add)
    entity.Fire?("AddingTo",add)
    if add.Cancel
      @Root?.Detaching?(add)
      return false
    @Entities(false).push(entity)
    @Raise("Added",add)
    entity.Fire?("AddedTo",add)
    if not (entity.IsInitialized? and entity.IsInitialized)
      entity.Fire?("Init",{})
      entity.IsInitialized = true

  meAddedTo: (add)->
    @Parent = add.Parent
    @Root = @Parent.Root ? @Parent

  ###    
  Remove: (ent)->
    # Remove entity from our list
    @Fire("Removed",{})
    @Destroy()
  ###

  Remove: (entity)->
    # Remove from parent if no entity
    if not entity?
      return @Parent?.Remove(@)

    # Otherwise remove it from the entities collections
    remove = 
      Child: entity
      Parent: parent
      Cancel: false
    @Fire("Removing", remove)
    entity.Fire("RemovingFrom",remove)
    if !remove.Cancel
      # Remove safely: rather than calling .splice on the original array, filter out into a new array - so
      # any event loops we're in the middle of will still have a reference to the original array
      @_entities = (e for e in @Entities(true) when e isnt entity)
      @Fire("Removed", remove)
      entity.Fire("RemovedFrom", remove)

  Clear: ()->
    @Remove(e) for e in @Entities()

  meRemovedFrom: (removed)->
    if @Parent is removed.Parent
      @Parent = null

  ###
  method Destroy
    Destructs the entity and removes from parent (and all child entities)
  ###
  Destroy: ()->
    destroy = { Cancel:false }
    @Cast("Destroying",destroy)
    if !destroy.Cancel
      @Cast("Destroy")
      @Remove()

  onDestroy: ()->
    @Remove()

  ###
  method DeferUntilStep

  Stores a closure to execute as soon as we get a Step event. Mainly needed when creating and destroying physics entities.

  TODO: Am thinking some sort of optional async processing for events might improve things
  ###
  DeferUntilStep: (action)->
    # TODO: We could completely automate deference of object creation / destruction with a higher-level API, a custom World object, and flagging a status
    # from the contact listener. *However* it's not really necessary since we don't need to do it so often (although, any time a collision causes
    @_DeferredUntilStep.push(action)

  # TODO: I don't like having this here as we have to remember to call super anytime we override onStep (which thankfully shouldn't be all that often).
  # Also it would be better if all such actions were hoisted up to Root so it doesn't matter anywhere if we're removing from an array during an event loop.
  # TODO: Additionally, this is also assign that there's way too much going on in this base entitiy. Need to split things out more (presumably into
  # completely isolated behaviors / mixins, which will also help us not having to call super so often!)
  onStep: (step)->
    if @_DeferredUntilStep?.length
      a() for a in @_DeferredUntilStep
      @_DeferredUntilStep = []
###
class Router
  Sits at the top of the entities tree and manages attached entities and event dispatching.
###
Delta.Entities.Router = class Router extends BaseEntity

  constructor: () ->
    @_Attached = []
    @_Events = {}
    @Root = this
    @Parent = this
    super

  Attaching: (add)->
    @_AttachEntity(add.Child)

  Detaching: (det)->
    @_DetachEntity(det.Child)

  Raise: (name,data,qualifier = "on")->
    data = @_Event(data)
    if qualifier == "on"
      @_DispatchEvent(name,data)
    else
      super(name,data,qualifier)

  _EventsForEntity: (entity, type="on") ->
    regex = new RegExp("^"+type+"[A-Z0-9]")
    k.substring(type.length) for k,v of entity when k.match(regex)

  _AttachEntity: (entity) ->
    for e in @_EventsForEntity(entity)
      @_SubscribeEvent(entity,e)
      
  _DetachEntity: (entity) ->
    for e in @_EventsForEntity(entity)
      @_UnsubscribeEvent(entity,e)

  _SubscribeEvent: (entity, name) ->
    if !@_Events[name]?
      @_Events[name] = []
    @_Events[name].push(entity)

  _UnsubscribeEvent: (entity, name) ->
    @_Events[name] = e for e in @_Events[name] when e isnt entity

  _DispatchEvent: (name,event) ->
    if @_Events[name]?
      for e in @_Events[name]
        e.Call(name,event)

  Initialize: ()->
    @Raise("Initialize", {})
    @Raise("Initialized", {})

###
class ArtEntity
  Entity with a visual component
  TODO: Should be in Delta.Graphics?
###
Delta.Entities.ArtEntity = class ArtEntity extends Delta.Entities.BaseEntity
  
  Position: new Vector(0,0)
  Rotation: 0
  Size: new Vector(1,1)
  Origin: new Vector(0,0)

  IsVisible: true

  LayerName: "Main"
  Art: null
  ArtName: "Main"
  ArtFrame:  0

  SetPosition: (pos)->
    if pos.x isnt @Position.x and pos.y isnt @Position.y
      move = 
        From: @Position
        To: pos
      @Fire("Moving",move)
      if not move.Cancel
        @Position = pos
        @Fire("Moved",move)

  SetRotation: (rot)->
    if @Rotation isnt rot
      rotate = 
        From: @Rotation
        To: rot
      @Fire("Rotating",rotate)
      if not rotate.Cancel
        @Rotation = rot
        @Fire("Rotated",rotate)

  GetBounds: ()->
    # Ignoring origin right now
    br = @Size.Multiply(0.5)
    tl = br.Multiply(-1)
    new Delta.Math.Quad(tl.x,tl.y,br.x,br.y)

  onDraw: (draw)->
    if !@IsVisible then return
    # Attempt to get a frame and draw it
    frame = @Art?.Frame(@ArtName,@ArtFrame)
    bounds = @GetBounds()
    if frame?
      draw.Context.drawImage(frame, bounds.x1, bounds.y1, bounds.width(), bounds.height())
    else
      # Otherwise an ugly black rectangle
      draw.Context.fillStyle = Delta.Math.Vector.Fill(0,3).toRGB()
      draw.Context.fillRect(bounds.x1,bounds.y1,bounds.width(),bounds.height())

# Easier reference for Entity
Delta.Entity = class Entity extends Delta.Entities.ArtEntity

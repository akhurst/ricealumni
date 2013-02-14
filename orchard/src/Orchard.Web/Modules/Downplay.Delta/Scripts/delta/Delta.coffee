###
Delta engine v0.2
(C)2012 P. Hurst, Downplay Design
###

Delta = @Delta = @Delta ? {}
Delta.Graphics = Delta.Graphics ? {}
Delta.Core = Delta.Core ? {}
Delta.Utility = Delta.Utility ? {}
Delta.Flags = Delta.Flags ? {}
Delta.Flags.Cordova = (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry)/))

# TODO: A dependency on Entities is implied (but we *always* need this anyway...)
Delta.Router = ()->
  if !@_Router
    @_Router = new Delta.Entities.Router()
  @_Router

Delta.Ready = (callback) ->
  Delta.Ready.Callbacks.push(callback)
  Delta.Ready.Call()

Delta.Ready.Call = ->
  # Check if we need Cordova
  if !Delta.Ready.Cordova?
    Delta.Ready.Cordova = !Delta.Flags.Cordova
  # Check for readiness
  if Delta.Ready.Cordova and Delta.Ready.jQuery
    # Fire and clear callbacks queue
    c() for c in Delta.Ready.Callbacks
    Delta.Ready.Callbacks = []
    Delta.Router().Initialize()
    
Delta.Ready.jQuery = false
Delta.Ready.Callbacks = []

# jQuery readiness
$ ->
  Delta.Ready.jQuery = true
  Delta.Ready.Call()

# Cordova readiness
if Delta.Flags.Cordova
  cordova = ()->
    Delta.Ready.Cordova = true
    Delta.Ready.Call()
  document.addEventListener("deviceready", cordova, false)

# Namespace management
Delta.Namespaces = {}
Delta.Namespace = (name)=>
  if !Delta.Namespaces[name]?
    dot = name.lastIndexOf(".")
    if dot>=0
      rootNamespace = name.substring(0,dot)
      subname = name.substring(dot+1)
      root = Delta.Namespace(rootNamespace)
      Delta.Namespaces[name] = root[subname] = root[subname] ? {}
    else
      Delta.Namespaces[name] = @[name] = @[name] ? {}

  return Delta.Namespaces[name]

# Stub function, will want to do something with Require one day
Delta.Require = (name)->

###
namespace Delta.Utility
  Some utility functions

function Class
  Determine the class of an object
  Adapted from: http://stackoverflow.com/questions/1249531/how-to-get-a-javascript-objects-class
###
Delta.Utility.Class = (o)->
  if (typeof o is "undefined")
    return "undefined"
  if (o is null)
    return "null"
  if Object.constructor.name? and Object.constructor.name != "Function"
    return Object.constructor.name
  return Object.prototype.toString.call(o)
    .match(/^\[object\s(.*)\]$/)[1]

Delta.Dump = (o)->
  console?.log (k+"="+v for k,v of o).join(", ")

Delta.Meld = (target, patch)->

  # Passing a target function means we meld a type's prototype to another
  if typeof target is "function"
    Delta.Extend(target, patch)
    return target

  # If we get a type then create the object first before melding
  if typeof patch is "function"
    con = patch
    patch = new con()

  for key, value in patch
    tClass = Delta.Utility.Class(target[key])
    pClass = Delta.Utility.Class(value)
    setValue = false

    # Attach event handlers
    # TODO: Allow Unmeld to remove the handler ... need to start tracking what has been added per mixin
    if key.indexOf("on") == 0 && target.on?
      target.on(key.substring(2), value)
    if key.indexOf("me") == 0 && target.me?
      target.me(key.substring(2), value)

    # Melding an anonymous object (not any other class)
    else if pClass is "Object"
      setValue = true
      # TODO: Set individual 
      # if target[key]? and typeof target[value]
      # if not (tClass is "null" or tClass is "Array" or tClass is "Number" or tClass is "String" or tClass is "Boolean")
      #   Meld sub object??

    else if pClass is "Array"
      # TODO: Need a convention to allow us to insert or replace arrays
      if tClass is "Array"
        target[key].push(i) for i in value
      else
        setValue = true
    else 
      setValue = true
    if setValue
      target[key] = value

Delta.Error = (error)->
  # TODO: Push to logging service if available in production
  if console? && console.log?
    console.log(error)
  else
    alert "Error: " + error

Delta.Configure = (config,hub)->

  if !hub?
    hub = Delta.Router()

  # Configure each instance
  for i in config.Instances
    
    if !Delta.Namespaces[i.Namespace]?
      Delta.Error("Can't find namespace #{i.Namespace} to create instance of #{i.Type}")
      continue
    if !Delta.Namespaces[i.Namespace][i.Type]?
      Delta.Error("Can't find type #{i.Type} in namespace #{i.Namespace}")
      continue
    
    instance = new Delta.Namespaces[i.Namespace][i.Type]()
    # Configure properties
    for k,v of i.Properties
      instance[k] = v

    # TODO: Allow other post-creation callbacks? Or will they just be handled with normal Delta events?
    hub.Add(instance)


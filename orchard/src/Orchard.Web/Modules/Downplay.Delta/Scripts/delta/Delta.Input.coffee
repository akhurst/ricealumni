Delta = @Delta = @Delta ? {}

###
namespace Delta.Input
  User input classes
###
Delta.Input = Delta.Input ? {}

###
class TouchInterface
  Hooks up to an element (usually a canvas) and captures both mouse and touch events to broadcast. Performs an inverse transformation
  based on the camera's transform so we get proper game coordinates out of the events.
  TODO: This really works far better as a meldable behaviour...
###
Delta.Input.TouchInterface = class TouchInterface extends Delta.Html.ElementEntity

  Finger: null
  FingerPosition: null
  Transform: new Delta.Math.Transform()
  Touching: false
  Buttons:
    Left: false
    Right: false
    Middle: false

  ###
  Catch camera event to store an inverse transform
  ###
  onCameraChanged: (camera)->
    @Transform = camera.Transform.Inverse()

  meBound: (bound)->
    @$
      .bind('mousedown', (ev) => @MouseDown(ev))
      .bind('mousemove', (ev) => @MouseMove(ev))
      .bind('mouseup', (ev) => @MouseUp(ev))
      .bind('touchstart', (ev) => @TouchDown(ev))
      .bind('touchmove', (ev) => @TouchMove(ev))
      .bind('touchend', (ev) => @TouchUp(ev))
    #  .bind('tap', (ev) => @Tap(ev))
    #  @Element.addEventListener('touchstart', ((ev) => @TouchDown(ev)), false)
    #  @Element.addEventListener('touchmove', ((ev) => @TouchMove(ev)), false)
    #  @Element.addEventListener('touchend', ((ev) => @TouchUp(ev)), false)

  TransformToWorld: (vector)->
    @Transform.ApplyToVector(vector)

  CursorFromTouch: (touch)->
    client = new Vector(touch.clientX,touch.clientY)
    Client: client
    World: @TransformToWorld(client)

  FireTouch: (type,touch)->
    @Raise "Mouse",
      Origin: "Touch"
      Type: type
      Cursors: @CursorFromTouch(t) for t in touch.originalEvent.touches

  FireMouse: (type,mouse)->
    button = if type is "Move" then null else (
      switch mouse.which
        when 1 then "Left"
        when 2 then "Middle"
        when 3 then "Right"
        else null
    )
    client = new Vector(mouse.clientX,mouse.clientY)
    if button? then @Buttons[button] = (if type is "Up" then false else true)
    @Raise "Mouse",
      Origin: "Mouse"
      Type: type
      Cursors: [
        Buttons: @Buttons
        Client: client
        World: @TransformToWorld(client)
      ]
  
  TouchDown: (touch)->
    @FireTouch("Down",touch)
    touch.preventDefault()

  TouchMove: (touch)->
    @FireTouch("Move",touch)
    touch.preventDefault()

  TouchUp: (touch)->
    @FireTouch("Up",touch)
    touch.preventDefault()

  MouseDown: (mouse)->
    @FireMouse("Down",mouse)
    mouse.preventDefault()

  MouseMove: (mouse)->
    @FireMouse("Move",mouse)
    mouse.preventDefault()

  MouseUp: (mouse)->
    @FireMouse("Up",mouse)
    mouse.preventDefault()

###
class InterfaceEntity
  Base class for HTML UI elements
###
Delta.Input.InterfaceEntity = class InterfaceEntity extends Delta.Html.ElementEntity
  
  TagName: "div"

  Html: (context)->
    @Element = document.createElement(@TagName)
    @$ = $(@Element)
    @Fire("HtmlCreating", {$: @$, Element:@Element})
    # Child elements
    for e in @Entities()
      child = e.Html?(context)
      if child?
        @$.append(child)
    @Fire("HtmlCreated", {$: @$, Element:@Element})
    @$

Delta.Input.Prompt = class Prompt extends Delta.Input.InterfaceEntity
  textDescribe: (o)->
    @Text("PreAmble")
    + @Text("Elements")
    + @Text("PostAmble")

  textElements: (o)->
    (e.Text("Describe") for e in @Entities()).join("\n")

  textPreAmble: (o) -> "Prompt:"
  textPostAmble: (o) -> ""

Delta.Input.TextPrompt = class TextPrompt extends Prompt

  EnteredText: ""

  meBuild: (build)->
    @Add(
      new TextElement("Text").Wire(
        Change: (change)=>
          @EnteredText = change.Value
      )
    )
    @Add(
      new OkButton((click)=>
        @Fire("TextEntered",{Text:@EnteredText})
      )
    )

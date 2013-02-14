Delta = @Delta = @Delta ? {}
Delta.Graphics = Delta.Graphics ? {}

Vector = Delta.Math.Vector

Vector::toRGB = (fac)->
  if fac?
    @Multiply(fac).toRGB()
  else
    "rgb(#{Math.floor(@x*255)},#{Math.floor(@y*255)},#{Math.floor(@z*255)})"
Vector::toRGBA = (fac,a)->
  if fac?
    @Multiply(fac).toRGBA(null,a)
  else
    if not a? then a = @u
    "rgb(#{Math.floor(@x*255)},#{Math.floor(@y*255)},#{Math.floor(@z*255)},#{Math.floor(a*255)}))"

# Canvas surface renders to a canvas
Delta.Graphics.Layer = class Layer extends Delta.Entities.BaseEntity

  UnitMaxPixelSize: new Vector(20,20)
  
  constructor: (@Name,@Canvas,@Width,@Height)->
    @Width = @Width ? @Canvas.width()
    @Height = @Height ? @Canvas.height()
    super()

  MakeCanvas: ()->
    if not @Canvas?
      @Canvas = $("<canvas width='{#@Width}' height='{#@Height}'/>")

    @Canvas.width = @Width
    @Canvas.height = @Height

    @$ = $ @Canvas
    @Context = @Canvas.get(0).getContext("2d")

  onCameraChanged: (camera)->
    # @IsDirty = true
    @Transform = camera.Transform
    if not @Context?
      @Width = camera.MaxBounds.width()*@UnitMaxPixelSize.x
      @Height = camera.MaxBounds.height()*@UnitMaxPixelSize.y
      alert @Width
      @MakeCanvas()
    
    @$.width(@Width * @Transform.a)
    @$.height(@Height * @Transform.d)
    @$.css
      left: @Transform.e * @UnitMaxPixelSize.x+"px"
      top: @Transform.f * @UnitMaxPixelSize.y+"px"
    
  # To be fired when anything is being added to the parent - hijack and add it to ourselves instead
  onAdding: (add)->
    if (add.Parent is @Parent) and (add.Child.LayerName? and (add.Child.LayerName is @Name))
      add.Cancel = true
      @Add(add.Child)

  childMoved: (move)->
    move.Source.IsDirty = true
    if not move.Source.DirtyRectangle?
      half = move.Source.Size.Multiply(0.5)
      @DirtyInBounds(move.Source.DirtyRectangle = new Delta.Math.Quad(move.From.Subtract(half),move.From.Add(half)))
    @IsDirty = true

  childRotated: (rotate)->
    rotate.Source.IsDirty = true
    if not rotate.Source.DirtyRectangle?
      half = rotate.Source.Size.Multiply(0.6)
      @DirtyInBounds(rotate.Source.DirtyRectangle = new Delta.Math.Quad(rotate.Source.Position.Subtract(half),rotate.Source.Position.Add(half)))
    @IsDirty = true

  DirtyInBounds: (quad)->
    # Make sure anything else in range is redrawn
    for e in @Entities()
      if e.Size? and e.Position?
        half = e.Size.Multiply(0.5)
        bounds = new Delta.Math.Quad(e.Position.Subtract(half),e.Position.Add(half))
        if bounds.Intersects(quad) then e.IsDirty = true

  onRender: (render)->
    if not @IsDirty then return false
    @Context.save()
    @IsDirty = false
    draw = 
      Canvas: @Canvas
      Context: @Context
      Cancel: false
    # Wipe out any dirtied regions
    for e in @Entities()
      if e.DirtyRectangle?
        @Context.clearRect(e.DirtyRectangle.x1,e.DirtyRectangle.y1,e.DirtyRectangle.width(),e.DirtyRectangle.height())
        e.DirtyRectangle = null
    # Draw anything that needs redrawing
    for e in @Entities()
      if not e.IsDirty then continue
      e.IsDirty = false
      @Context.save()
      # Translate / rotate so the entity can draw to local coords
      if e.Position?
        @Context.translate(e.Position.x, e.Position.y)
      if e.Rotation?
        @Context.rotate(e.Rotation)
      e.Cast?("Drawing",draw)
      if !draw.Cancel
        e.Cast?("Draw",draw)
        e.Cast?("Drawn",draw)
      @Context.restore()
    @Context.restore()

Delta.Graphics.Camera = class Camera extends Delta.Entities.BaseEntity

  MaxBounds: null

  Current: null
  Target: null
  Delta: null
  constructor: ()->
    super
    @Target =
      Position: new Vector(0,0)
      Scale: new Vector(1,1)
      Rotation: 0  # Not implemented
    @Current =
      Position: new Vector(0,0)
      Scale: new Vector(1,1)
      Rotation: 0  # Not implemented
    @Delta = {}
    @MaxBounds = new Delta.Math.Quad(0,0,0,0)

  # When a child is added anywhere, extend the bounds
  onAdded: (added)->
    if added.Child.Bounds?
      if added.Child.Bounds.x1<@MaxBounds.x1 then @MaxBounds.x1 = added.Child.Bounds.x1
      if added.Child.Bounds.y1<@MaxBounds.y1 then @MaxBounds.y1 = added.Child.Bounds.y1
      if added.Child.Bounds.x2>@MaxBounds.x2 then @MaxBounds.x2 = added.Child.Bounds.x2
      if added.Child.Bounds.y2>@MaxBounds.y2 then @MaxBounds.y2 = added.Child.Bounds.y2

  TargetToBounds: ()->

  onStep: (step)->
    # if @IsAnimating
      
Delta.Graphics.ClunkyCamera = class ClunkyCamera extends Camera

  MaxBounds: null
  MaxSurface: new Vector(1,1)
  Transform: null
  constructor: ()->
    super
    @MaxBounds = new Delta.Math.Quad(0,0,0,0)

  onAdded: (added)->
    if added.Child.Bounds?
      if added.Child.Bounds.x1<@MaxBounds.x1 then @MaxBounds.x1 = added.Child.Bounds.x1
      if added.Child.Bounds.y1<@MaxBounds.y1 then @MaxBounds.y1 = added.Child.Bounds.y1
      if added.Child.Bounds.x2>@MaxBounds.x2 then @MaxBounds.x2 = added.Child.Bounds.x2
      if added.Child.Bounds.y2>@MaxBounds.y2 then @MaxBounds.y2 = added.Child.Bounds.y2
    if added.Child instanceof Layer
      @MaxSurface = new Vector(added.Child.Width,added.Child.Height)

  TransformFromBounds: (bounds)->
    width = (bounds.x2-bounds.x1)
    height = (bounds.y2-bounds.y1)
    scaleX = @MaxSurface.x/width
    scaleY = @MaxSurface.y/height
    # Pick the best scale
    scaleFinal = if (scaleX > scaleY) then (if (height * scaleX > @MaxSurface.y) then scaleY else scaleX) else (if (width * scaleY > @MaxSurface.x) then scaleX else scaleY)
    Delta.Math.Transform.Translate(bounds.x1,bounds.y1).Scale(scaleFinal,scaleFinal)

  onSetup: (setup)->
    @SetTransform(@TransformFromBounds(@MaxBounds))

  SetTransform: (transform)->
    @Transform = transform
    @Raise("CameraChanged", {Transform: @Transform, MaxBounds: @MaxBounds, MaxScale:@MaxScale })

  AnimateToTransform: (transform)->
    @SetTransform(transform)
    
Delta.Graphics.Style = class Style

Delta.Graphics.Art = class Art

  constructor: ()->
    @Frames = {}

  Name: "base"

  Size: new Vector(128,128)

  Frames: null
  FrameInfo:
    Main: { Count:1 }

  Render: (name, frameInfo, num)->
    canvas = document.createElement("canvas")
    canvas.width = @Size.x
    canvas.height = @Size.y
    context = canvas.getContext("2d")
    if (@["Draw"+name]?)
      @["Draw"+name](context,num/frameInfo.Count)
    else
      @Draw(context,name,num/frameInfo.Count)
    return canvas

  Draw: (canvas,name,num)->
    null

  Frame: (name,num)->
    if (not @Frames[name]?)
      if (not @FrameInfo[name]?)
        alert("Frame '#{name}' not found for #{Delta.Utility.Class(this)} '#{@Name}'")
        return null
      @Frames[name] = (null for n in [1..@FrameInfo[name].Count])
    if (not @Frames[name][num]?)
      @Frames[name][num] = @Render(name, @FrameInfo[name], num)
    return @Frames[name][num]

Delta.Graphics.DeferredCanvas = class DeferredCanvas
    
Delta.Graphics.ImageLoader = class ImageLoader
  constructor: (@Url)->
    @Image = new Image()
    @Image.onload = @Loaded
    @Image.src = @Url

  IsLoaded: false

  Loaded: ()=>
    @IsLoaded = true
  
  Filter: (func)->
    if not @IsLoaded then return null

    # Create a canvas and grab all the pixels
    canvas = document.createElement('canvas')
    canvas.width = @Image.width
    canvas.height = @Image.height
    context = canvas.getContext('2d')
    context.drawImage(@Image,0,0)
    pixels = context.getImageData(0,0,canvas.width,canvas.height)
    # Process the pixels with our filter
    filtered = (func(new Vector(p[0],p[1],p[2],p[3])) for p in ([ pixels.data[n*4], pixels.data[n*4+1], pixels.data[n*4+2],pixels.data[n*4+3] ] for n in [0..(pixels.data.length-1)/4]))
    for f,n in filtered
      pixels.data[n*4] = Math.floor(f.x)
      pixels.data[n*4+1] = Math.floor(f.y)
      pixels.data[n*4+2] = Math.floor(f.z)
      pixels.data[n*4+3] = Math.floor(f.u)

    context.putImageData(pixels,0,0) #,canvas.width,canvas.height)
    return canvas

Delta.Graphics.TextureFill = class TextureFill

  constructor: (@Texture, @Colour)->
    if not (@Colour instanceof Vector)
      @Colour = new Vector(@Colour)
    if typeof @Texture is "string"
      @Texture = new ImageLoader(@Texture)
    else
      @Texture = 
        IsLoaded: true
        Image: @Texture

  Fill: (canvas, bounds)->
    return if @Texture?.IsLoaded then @Texture.Image else @Colour
    ###
    if bounds instanceof Delta.Math.Quad
      if @Texture?.IsLoaded
        canvas.drawImage(@Texture.Image,bounds.x1,bounds.y1,bounds.x2-bounds.x1,bounds.y2-bounds.y1)
      else
        canvas.fillStyle = "rgb(#{@Colour.x*255},#{@Colour.y*255},#{@Colour.z*255})"
        canvas.fillRect(bounds.x1,bounds.y1,bounds.x2-bounds.x1,bounds.y2-bounds.y1)
    ###
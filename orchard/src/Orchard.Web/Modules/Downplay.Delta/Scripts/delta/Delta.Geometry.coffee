###
Delta.Geometry

Defines functions relating to shape and spacial concerns
###

Delta = @Delta = @Delta ? {}
Delta.Geometry = Delta.Geometry ? {}

Vector = Delta.Math.Vector

# Box2D imports
{b2MassData, b2PolygonShape, b2CircleShape} = Box2D.Collision.Shapes

Shape = class Delta.Geometry.Shape
  Draw: (draw)->
    null

  # Convert to Box2D shape
  Box2D: ()->
    null

class Delta.Geometry.CircleShape extends Shape
  Radius: 1

  constructor: (radius)->
    if radius?
      @Radius = radius

  Draw: (draw)->
    draw.Circle(@Radius)

  DrawCanvas: (canvas)->
    # Simple circle path
    canvas.arc(0, 0, @Radius, 0 , 2 * Math.PI, false)

  Box2D: ()->
    new b2CircleShape(@Radius)

class Delta.Geometry.PolygonShape extends Shape
  Draw: (draw)->
    draw.Path(@GetVertices())

  DrawCanvas: (canvas)->
    path = @GetVertices()
    canvas.moveTo(path[0].x,path[0].y)
    for n in [1..(path.length-1)]
      canvas.lineTo(path[n].x,path[n].y)
    canvas.lineTo(path[0].x,path[0].y)

  GetVertices: ()->
    if not @_cachedVertices?
      @_cachedVertices = @MakeVertices()
    @_cachedVertices # return

  MakeVertices: () -> []

  Box2D: ()->
    shape = new b2PolygonShape()
    vertices = @GetVertices()
    shape.SetAsArray(vertices,vertices.length)
    shape

# Constructs a regular polygon with specified # of faces
Delta.Geometry.QuadShape = class QuadShape extends Delta.Geometry.PolygonShape
  constructor: (@Bounds)->
  
  MakeVertices: () ->
    [new Vector(@Bounds.x1,@Bounds.y1),
     new Vector(@Bounds.x1,@Bounds.y2),
     new Vector(@Bounds.x2,@Bounds.y2),
     new Vector(@Bounds.x2,@Bounds.y1)]

Delta.Geometry.RegularPolygonShape = class RegularPolygonShape extends Delta.Geometry.PolygonShape
  constructor: (@Radius,@Faces)->

  MakeVertices: () ->
    (new Vector(Math.sin(Math.PI*n*2/@Faces),Math.cos(Math.PI*n*2/@Faces)).Multiply(@Radius) for n in [(@Faces-1)..0])

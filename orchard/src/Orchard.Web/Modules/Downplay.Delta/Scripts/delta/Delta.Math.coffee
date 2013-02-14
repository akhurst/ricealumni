Delta = @Delta = @Delta ? {}
Delta.Math = Delta.Math ? {}

Delta.Math.Vector = class Vector

  @PropertyNames: ['x','y','z','u','v','w']

  Values: []
  Dimensions: 0
  constructor: (values...)->
    @SetValues(values...)

  toString: ()->
    "<"+@Values.join(", ")+">"

  SetValues: (values...)->
    if values.length is 1 and values[0] instanceof Array  # TODO: That instanceof will fail
      values = values[0]
    @Values = values
    @Dimensions = @Values.length
    for p in Delta.Math.Vector.PropertyNames
      @[p] = null
    for n in [1..@Dimensions]
      @[Delta.Math.Vector.PropertyNames[n-1]] = @Values[n-1]

  Multiply: (factor)->
    if factor instanceof Matrix
      new Vector(new Matrix(@Values).Multiply(factor).Values[0])
    else if factor instanceof Vector
      new Vector(v*(factor.Values[n] ? 1) for v,n in @Values)
    else
      new Vector(v*factor for v in @Values)

  Add: (factor)->
    if factor instanceof Delta.Math.Vector
      new Vector(v+factor.Values[n] for v,n in @Values)
    else
      new Vector(v+factor for v in @Values)

  Subtract: (factor)->
    if factor instanceof Delta.Math.Vector
      vec = new Vector((v - factor.Values[n] for v,n in @Values))
    else
      new Vector((v - factor) for v in @Values)

  Normalize: ()->
    length = @Length()
    if length == 0
      return this
    @Multiply(1/length)

  Length: ()->
    total = 0
    (total += v*v) for v in @Values
    return Math.sqrt(total)

  @Fill: (value,count)->
    new Vector(value for n in [1..count])

Delta.Math.Matrix = class Matrix
Delta.Math.Transform = class Transform

  a: 1
  b: 0
  c: 0
  d: 1
  e: 0
  f: 0

  toString: ()->
    [@a,@b,@c,@d,@e,@f].join(" ")

  @Identity: ()->
    new Transform()

  @Translate: (x,y)->
    @Identity().Translate(x,y)
 
  @Scale: (x,y)->
    @Identity().Scale(x,y)

  Inverse: ()->
    t = new Transform()
    t.a = 1/@a
    t.b = @b
    t.c = @c
    t.d = 1/@d
    t.e = - @e
    t.f = - @f
    t

  Translate: (x,y)->
    t = new Transform()
    t.a = @a
    t.b = @b
    t.c = @c
    t.d = @d
    t.e = @e + x
    t.f = @f + y
    t

  Scale: (x,y)->
    t = new Transform()
    t.a = @a * x
    t.b = @b
    t.c = @c
    t.d = @d * y
    t.e = @e
    t.f = @f
    t

  ApplyToCanvas: (context)->
    context.transform(@a,@b,@c,@d,@e,@f)

  ApplyToVector: (vector)->
    new Vector(vector.x * @a + @e, vector.y * @d + @f)

Delta.Math.Quad = class Quad
  constructor: (@x1,@y1,@x2,@y2)->
    if @x1 instanceof Vector and @y1 instanceof Vector
      [v1,v2] = [@x1,@y1]
      [@x1,@y1,@x2,@y2] = [v1.x,v1.y,v2.x,v2.y]

  width: ()->
    @x2-@x1

  height: ()->
    @y2-@y1

  Intersects: (quad)->
    if (missX = (@x1 < quad.x1 and @x2 < quad.x1) or (@x1 > quad.x2 and @x2 > quad.x2)) then return false
    missY = (@y1 < quad.y1 and @y2 < quad.y1) or (@y1 > quad.y2 and @y2 > quad.y2)
    not missY


Delta.Math.Random = class Random
  



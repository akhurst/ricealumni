Delta = @Delta = @Delta ? {}
Delta.Time = Delta.Time ? {}

Delta.Time.Ticker = class Ticker extends Delta.Entities.BaseEntity
  
  StartTime: 0
  TotalTime: 0
  Time: 0
  Frequency: 1000/30

  constructor: (@Frequency = 1000/30) ->
    super()
  onStartTimer: (timer)->
    #onPlay: (play) ->
    @StartInterval()
    
  onStopTimer: (timer)->
    #onPause: (pause) ->
    @StopInterval()
    
  _IntervalRef:null

  StartInterval: ()->
    if not @_IntervalReference?
      date = new Date()
      @Time = date.getTime()
      @_IntervalReference = setInterval((()=>@HandleInterval()), @Frequency)

  StopInterval: ()->
    if @_IntervalReference?
      window.clearInterval(@_IntervalReference )

  HandleInterval: () ->
    date = new Date()
    time = date.getTime()
    delta = time - @Time
    @Time = time
    total = (@TotalTime + delta)
    tick = 
      Delta: delta/1000
      Total: total/1000
    # @Raise("Ticking",tick)
    @Raise("Tick",tick)
    @Time = time
    @TotalTime = total
    # @Raise("Ticked",tick)

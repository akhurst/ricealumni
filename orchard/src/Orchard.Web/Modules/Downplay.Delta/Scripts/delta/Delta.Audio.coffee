Delta = @Delta = @Delta ? {}
Delta.Audio = Delta.Audio ? {}

Delta.Audio.Sound = class Sound
  @AudioTypes:
    mp3: ["mp3", "audio/mpeg"]
    wav: ["wav", "audio/wav"]

  constructor: (@Name,@File)->
    if (Delta.Flags.Cordova)
      @Media = new Media("/android_asset/www/"+@File+".mp3", @MediaSuccess)
    else
      @Media = document.createElement("audio")
      type = Delta.Audio.Sound.AudioTypes.mp3
      # Trust Firefox to be the oddball
      if $.browser.mozilla
        type = Delta.Audio.Sound.AudioTypes.wav

      @Media.src = @File+"."+type[0]
      @Media.type = type[1]  # TODO: Provide multiple versions of audio for different platforms
      @Media.addEventListener("ended", @MediaEnded, false)

  MediaEnded: ()=>
    @IsPlaying = false
  MediaSuccess: (m)=>

  play: ()->
    @stop()
    @Media.play()
    @IsPlaying = true

  stop: ()->
    if @IsPlaying
      if Delta.Flags.Cordova
        @Media.stop()
      else
        @Media.currentTime = 0
        @Media.pause()
      @IsPlaying = false

(function() {
  var Delta, Sound, _ref, _ref1,
    __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Audio = (_ref1 = Delta.Audio) != null ? _ref1 : {};

  Delta.Audio.Sound = Sound = (function() {

    Sound.name = 'Sound';

    Sound.AudioTypes = {
      mp3: ["mp3", "audio/mpeg"],
      wav: ["wav", "audio/wav"]
    };

    function Sound(Name, File) {
      var type;
      this.Name = Name;
      this.File = File;
      this.MediaSuccess = __bind(this.MediaSuccess, this);

      this.MediaEnded = __bind(this.MediaEnded, this);

      if (Delta.Flags.Cordova) {
        this.Media = new Media("/android_asset/www/" + this.File + ".mp3", this.MediaSuccess);
      } else {
        this.Media = document.createElement("audio");
        type = Delta.Audio.Sound.AudioTypes.mp3;
        if ($.browser.mozilla) {
          type = Delta.Audio.Sound.AudioTypes.wav;
        }
        this.Media.src = this.File + "." + type[0];
        this.Media.type = type[1];
        this.Media.addEventListener("ended", this.MediaEnded, false);
      }
    }

    Sound.prototype.MediaEnded = function() {
      return this.IsPlaying = false;
    };

    Sound.prototype.MediaSuccess = function(m) {};

    Sound.prototype.play = function() {
      this.stop();
      this.Media.play();
      return this.IsPlaying = true;
    };

    Sound.prototype.stop = function() {
      if (this.IsPlaying) {
        if (Delta.Flags.Cordova) {
          this.Media.stop();
        } else {
          this.Media.currentTime = 0;
          this.Media.pause();
        }
        return this.IsPlaying = false;
      }
    };

    return Sound;

  })();

}).call(this);

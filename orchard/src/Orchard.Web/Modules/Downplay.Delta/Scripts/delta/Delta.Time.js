(function() {
  var Delta, Ticker, _ref, _ref1,
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  Delta = this.Delta = (_ref = this.Delta) != null ? _ref : {};

  Delta.Time = (_ref1 = Delta.Time) != null ? _ref1 : {};

  Delta.Time.Ticker = Ticker = (function(_super) {

    __extends(Ticker, _super);

    Ticker.name = 'Ticker';

    Ticker.prototype.StartTime = 0;

    Ticker.prototype.TotalTime = 0;

    Ticker.prototype.Time = 0;

    Ticker.prototype.Frequency = 1000 / 30;

    function Ticker(Frequency) {
      this.Frequency = Frequency != null ? Frequency : 1000 / 30;
      Ticker.__super__.constructor.call(this);
    }

    Ticker.prototype.onStartTimer = function(timer) {
      return this.StartInterval();
    };

    Ticker.prototype.onStopTimer = function(timer) {
      return this.StopInterval();
    };

    Ticker.prototype._IntervalRef = null;

    Ticker.prototype.StartInterval = function() {
      var date,
        _this = this;
      if (!(this._IntervalReference != null)) {
        date = new Date();
        this.Time = date.getTime();
        return this._IntervalReference = setInterval((function() {
          return _this.HandleInterval();
        }), this.Frequency);
      }
    };

    Ticker.prototype.StopInterval = function() {
      if (this._IntervalReference != null) {
        return window.clearInterval(this._IntervalReference);
      }
    };

    Ticker.prototype.HandleInterval = function() {
      var date, delta, tick, time, total;
      date = new Date();
      time = date.getTime();
      delta = time - this.Time;
      this.Time = time;
      total = this.TotalTime + delta;
      tick = {
        Delta: delta / 1000,
        Total: total / 1000
      };
      this.Raise("Tick", tick);
      this.Time = time;
      return this.TotalTime = total;
    };

    return Ticker;

  })(Delta.Entities.BaseEntity);

}).call(this);

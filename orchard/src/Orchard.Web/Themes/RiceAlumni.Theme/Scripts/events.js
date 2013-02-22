/// <reference path="knockout.js"/>

function Event(detail) {
    var self = this;

    self.Title = detail.Title;
    self.Description = detail.Description;
    self.Date = detail.StartDate;
    self.LinkTarget = detail.LinkTarget;
    self.LinkText = detail.LinkText;
    self.visible = ko.observable();

}

function EventViewModel() {
    var self = this;

    self.events = ko.observableArray();

    self.loadInitialEvents = function () {
        
        var onComplete = function (data) {
            self.events($($.parseJSON(data.responseText)).map(function (eventDetail) {
                return new Event(eventDetail);
            }
            ));
        };
        
        $.ajax({dataType:'json', url: 'calendar/getevents?from=12-12-99&to=01-01-2016', type: 'POST', complete: onComplete });
    };

    self.loadMoreEvents = function () {

    };

    self.showEvent = function (event) {
        event.visible = true;
    };

    self.toggleShow = function (event) {
        self.hideEvents();
        self.showEvent(event);
    };

    self.hideEvents = function () {
        for (var i = 0; i < self.events().length; i++) {
            self.events()[i].visible(false);
        }
    };
}

var eventsVm = new EventViewModel();

$(function () { eventsVm.loadInitialEvents(); });

ko.applyBindings(eventsVm);
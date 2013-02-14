using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Downplay.Quanta.Signalling.Models;
using Orchard.Data;
using Downplay.Quanta.Signalling.Settings;
using Downplay.Quanta.Signalling.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Handlers
{
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class CreateNotificationPartHandler : ContentHandler
    {

        private readonly IContentNotificationService _notificationService;

        public CreateNotificationPartHandler(IRepository<CreateNotificationPartRecord> repository, IContentNotificationService notificationService)
        {
            _notificationService = notificationService;

            Filters.Add(StorageFilter.For(repository));

            OnCreated<CreateNotificationPart>((context, part) =>
                {
                    var settings = part.Settings.GetModel<CreateNotificationTypePartSettings>();
                    if (settings == null) return;

                    _notificationService.SendNotification(settings, part.ContentItem);
                });

        }


    }
}
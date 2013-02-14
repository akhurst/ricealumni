using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Settings
{
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public abstract class NotificationTypePartSettings
    {

        public bool NotifyOwner { get; set; }
        public bool NotifyContainerOwner { get; set; }
        public string UsersToNotify { get; set; }

        public abstract string MessageType { get; }
    }
}
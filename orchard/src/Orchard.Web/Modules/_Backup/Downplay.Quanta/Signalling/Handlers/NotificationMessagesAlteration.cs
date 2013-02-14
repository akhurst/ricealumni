using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Messaging.Events;
using Orchard.ContentManagement;
using Orchard.Settings;
using Orchard.Localization;
using Downplay.Quanta.Signalling.Models;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Handlers
{
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class NotificationMessagesAlteration : IMessageEventHandler
    {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public NotificationMessagesAlteration(IContentManager contentManager, ISiteService siteService)
        {
            _contentManager = contentManager;
            _siteService = siteService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Sending(global::Orchard.Messaging.Models.MessageContext context)
        {
            if (context.MessagePrepared)
                return;

            switch (context.Type)
            {
                    // Notify content published
                case MessageTypes.PublishNotification:
                    context.MailMessage.Subject = T("{0} published",context.Properties["ContentTypeName"]).Text;
                    context.MailMessage.Body =
                        T("<p>A new <b>{0}</b> has been published: \"{2}\".<br/>You can view it at: <a href=\"{1}\">{1}</a>.</p>",
                            context.Properties["ContentTypeName"], context.Properties["PublishedUrl"], context.Properties["Title"]).Text;
                    context.MessagePrepared = true;
                    break;
                case MessageTypes.CreateNotification:
                    context.MailMessage.Subject = T("{0} created",context.Properties["ContentTypeName"]).Text;
                    context.MailMessage.Body =
                        T("<p>A new <b>{0}</b> has been created: \"{2}\".<br/>You can view it at: <a href=\"{1}\">{1}</a>.</p>",
                            context.Properties["ContentTypeName"], context.Properties["PublishedUrl"], context.Properties["Title"]).Text;
                    context.MessagePrepared = true;
                    break;
            }
        }

        public void Sent(global::Orchard.Messaging.Models.MessageContext context)
        {
        }
    }
}
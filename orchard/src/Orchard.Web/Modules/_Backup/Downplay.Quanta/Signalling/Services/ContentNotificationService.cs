using System;
using Downplay.Quanta.Signalling.Models;
using Orchard;
using Orchard.Messaging.Services;
using Orchard.Security;
using Orchard.ContentManagement;
using Downplay.Quanta.Signalling.Settings;
using System.Collections.Generic;
using System.Linq;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Mvc.Html;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Orchard.ContentManagement.Aspects;
using Orchard.Mvc.Extensions;
using Orchard.Utility.Extensions;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Services
{
	/// <summary>
	/// Description of ContentNotificationService.
	/// </summary>
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class ContentNotificationService : IContentNotificationService
	{
		
		private readonly IOrchardServices _services;
		private readonly IMembershipService _membershipService;
		private readonly IMessageManager _messageManager;
		
		public ContentNotificationService(IOrchardServices services, IMembershipService membershipService, IMessageManager messageManager)
		{
			_services = services;
			_messageManager = messageManager;
			_membershipService = membershipService;
		}
		
		public void SendNotification(NotificationTypePartSettings settings, ContentItem subject) {
                List<IUser> users = String.IsNullOrWhiteSpace(settings.UsersToNotify)
                                    ? new List<IUser>()
                                    : settings.UsersToNotify.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => _membershipService.GetUser(s)).ToList();
                
                if (settings.NotifyOwner)
                {
                    users.Add(subject.As<CommonPart>().Owner);
                }
                if (settings.NotifyContainerOwner)
                {
                    // Ignore this for now
                    var common = subject.As<ICommonPart>();
                    var container = common.Container;
                    if (container!=null) {
                        users.Add(container.As<ICommonPart>().Owner);
                    }
                }
                // Set up some values before we perform send
            var routable = subject.As<IRoutableAspect>();
            var title = routable==null?"...":routable.Title;
            var props = new Dictionary<string, string> {
                { "PublishedUrl",GetItemDisplayUrl(subject) },
                { "ContentTypeName", subject.ContentType },
                { "Title", title }
            };
                foreach ( var recipient in users ) {
                    if (recipient==null) {
                        continue;
                    }
                        _messageManager.Send(recipient.ContentItem.Record, settings.MessageType,
                            // TODO: Support other channels
                    	                     "email" , props);
                }
		}

        protected string GetItemDisplayUrl(IContent item)
        {
            // Was particularly complex working out how to do this :)
            var helper = new UrlHelper(new RequestContext(
                new HttpContextWrapper(HttpContext.Current),
                new RouteData()), RouteTable.Routes);
            var siteUrl = helper.RequestContext.HttpContext.Request.ToRootUrlString();
            return siteUrl + helper.ItemDisplayUrl(item);
        }
    }
}

/*
 * Created by SharpDevelop.
 * User: Pete
 * Date: 24/03/2011
 * Time: 15:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Downplay.Quanta.Signalling.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Messaging.Services;
using Downplay.Quanta.Signalling.Services;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Drivers
{
	/// <summary>
	/// Description of PublishNotificationPartDriver.
	/// </summary>
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class CreateNotificationPartDriver : ContentPartDriver<CreateNotificationPart>
	{
		private readonly IContentNotificationService _notificationService;

        public CreateNotificationPartDriver(IContentNotificationService notificationService)
		{
			_notificationService = notificationService;
		}

        protected override string Prefix
        {
            get
            {
                return "CreateNotification";
            }
        }

        //POST
        protected override DriverResult Editor(CreateNotificationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            // This line updates your part from the POST fields.
            updater.TryUpdateModel(part, Prefix, null, null);
            
            //_notificationService.SendNotification(part.UserNames, part.ContentItem);
            
            // Now just display the same editor as before
            return Editor(part, shapeHelper);
        }
	}
}

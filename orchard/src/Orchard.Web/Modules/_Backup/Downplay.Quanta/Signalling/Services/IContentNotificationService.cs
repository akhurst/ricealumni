/*
 * Created by SharpDevelop.
 * User: Pete
 * Date: 24/03/2011
 * Time: 15:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Orchard;
using Orchard.ContentManagement;
using Downplay.Quanta.Signalling.Settings;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Services
{
    /// <summary>
    /// Description of IContentNotificationService.
    /// </summary>
    public interface IContentNotificationService : IDependency
    {
        void SendNotification(NotificationTypePartSettings settings, ContentItem contentItem);
    }
}

/*
 * Created by SharpDevelop.
 * User: Pete
 * Date: 24/03/2011
 * Time: 15:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Models
{
	/// <summary>
	/// Description of PublishNotificationPart.
	/// </summary>
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class CreateNotificationPart : ContentPart<CreateNotificationPartRecord>
	{
        public CreateNotificationPart()
		{
		}
	}
}

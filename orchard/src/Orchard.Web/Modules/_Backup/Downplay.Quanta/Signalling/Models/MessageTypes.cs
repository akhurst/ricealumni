/*
 * Created by SharpDevelop.
 * User: Pete
 * Date: 24/03/2011
 * Time: 15:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Models
{
	/// <summary>
	/// Description of MessageTypes.
	/// </summary>
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public static class MessageTypes
	{
		public const string PublishNotification = "DOWNPLAY_CONTENTNOTIFICATIONS_PUBLISH";
        public const string CreateNotification = "DOWNPLAY_CONTENTNOTIFICATIONS_CREATE";
    }
}

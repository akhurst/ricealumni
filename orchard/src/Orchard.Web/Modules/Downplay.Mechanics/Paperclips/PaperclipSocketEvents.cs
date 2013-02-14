using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Settings;
using Orchard;

namespace Downplay.Mechanics.Paperclips
{
    /// <summary>
    /// Hijack the socket events to push socket display to zones
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipSocketEvents : SocketHandler
    {

        public PaperclipSocketEvents(
            IOrchardServices services
            )
        {
            Services = services;
        }

        public IOrchardServices Services { get; set; }

        protected override void Preparing(SocketDisplayContext context)
        {
            if (context.Left.DisplayType == "Detail")
            {
                var clipPart = context.Connector.Definition.Parts.FirstOrDefault(p => p.PartDefinition.Name == "PaperclipPart");
                if (clipPart != null)
                {
                    // Get actual part model from item
                    var settings = clipPart.Settings.GetModel<PaperclipTypePartSettings>();
                    if (!String.IsNullOrWhiteSpace(settings.DefaultPlacement))
                    {
                        context.LayoutPlacement = settings.DefaultPlacement;
                    }

                    if (!String.IsNullOrWhiteSpace(settings.DefaultDisplayType))
                    {
                        context.Connector.DisplayType = settings.DefaultDisplayType;
                    }

                }
            }
        }

    }
}
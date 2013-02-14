using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.ViewModels;
using Downplay.Mechanics.Services;
using Orchard;
using Orchard.ContentManagement;
using System.Web.Mvc;
using Downplay.Mechanics.Models;
using Orchard.Localization;
using Downplay.Mechanics.Drivers;
using Downplay.Mechanics.Filters;

namespace Downplay.Mechanics.Handlers
{
    public class DefaultSocketHandler : SocketHandler
    {
        
        private readonly IMechanicsService _mechanics;

        public DefaultSocketHandler(IMechanicsService mechanics, IOrchardServices services)
        {
            _mechanics = mechanics;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        protected override void Filtering(SocketEventContext context)
        {
            // If title is present on the connector, sort on it. Titles will automatically get copied up.
            // TODO: Need an easy way to disable title editing on the connector (and always persist changes), so this can still be used as a sorting convenience
            // in situations where it makes no sense to alter/override the title.
            if (context.Connector.Definition.Parts.Any(p => p.PartDefinition.Name == "TitlePart"))
            {
                context.SocketSorters.Add(new TitleSortingFilter());
            }
        }
        protected override void Displaying(SocketDisplayContext context)
        {
            // Set a paradigm for empty sockets
            if (context.Query.TotalCount == 0)
            {
                context.Paradigms.Add("Empty");
                if (!context.Paradigms.Has("ShowEmpty")) {
                    context.RenderSocket = false;
                }
                // TODO: A pure-placement method didn't work because Displaying happens after Placement. But we don't want to execute the count for every single connector type
                // because a lot will already be decided against in advance. Either a) cache the current count on SocketsPartRecord or b) find another way, e.g. a delegate that
                // will only run if placement hasn't already been denied.
//                context.Paradigms.Add("Empty");
            }

            // Do some default display type mapping
            // TODO: Could be done in placement now?
            if (String.IsNullOrWhiteSpace(context.Connector.DisplayType))
            {
                switch (context.Left.DisplayType)
                {
                    case "Detail":
                        context.Connector.DisplayType = "Summary";
                        break;
                    case "Summary":
                        context.Connector.DisplayType = "SummaryTiny";
                        context.Paradigms.Add("ConnectorsFlatten");
                        break;
                    case "SummaryAdmin":
                        context.Connector.DisplayType = "Link";
                        context.Paradigms.Add("ConnectorsFlatten");
                        break;
                    case "SummaryTiny":
                        // Prevent further recursion by default
                        context.RenderSocket = false;
                        break;
                    case "Link":
                        // Prevent further recursion by default
                        context.RenderSocket = false;
                        break;
                    default:
                        // Any other situtations we don't understand, just use links.
                        context.Connector.DisplayType = "Link";
                        break;
                }
            }
        }

        protected override void Editing(SocketDisplayContext context)
        {
            context.Left.DisplayType="Editor";
            context.Connector.DisplayType = "EditorConnector";

            if (context.Query.TotalCount == 0) {
                context.Paradigms.Add("Empty");
            }
            
            // Check box list or drop down list?
            context.Paradigms.Add(context.Connector.Settings.AllowMany ? "Many" : "One");
            context.Paradigms.Add(context.Connector.Settings.AllowDuplicates ? "DuplicatesPossible" : "Unique");

            // Outright hide editor. Typically used when we have an inverse connector and we only
            // actually want to work with one end of the connector in UI.
            if (context.Paradigms.Has("Hidden")) {
                context.RenderSocket = false;
            }
        }

   }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using ClaySharp.Implementation;

using Orchard;
using Orchard.Mvc.Html;
using Orchard.UI.Zones;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement.Descriptors;

using Downplay.Mechanics.Services;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Framework;
using Orchard.Localization;
using Orchard.DisplayManagement.Shapes;


namespace Downplay.Mechanics.Providers
{
    public class MechanicsShapes : IShapeTableProvider
    {

        private readonly Lazy<IMechanicsService> _mechanics;
        private readonly IOrchardServices _orchardServices;

        public MechanicsShapes(IOrchardServices orchardServices, Lazy<IMechanicsService> mechanics)
        {
            _orchardServices = orchardServices;
            _mechanics = mechanics;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Content")
                .OnDisplaying(displaying =>
                    {
                        if (displaying.ShapeMetadata.DisplayType == "SummaryAdminConnector")
                        {
                            displaying.ShapeMetadata.Alternates.Add("Content_SummaryAdminConnector");
                        }
                    });
            // This is just the dummy shape that gets created when displaying site/user paperclips. But it needs zone holding behavior so it works like Content.
            builder.Describe("Sockets")
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.New.Zone(), null)))
                .OnCreated(created =>
                    {
                        var sockets = created.Shape;
                        sockets.ItemClasses = new List<string>();
                        sockets.ItemAttributes = new Dictionary<string, string>();
                    });
            builder.Describe("Socket_Edit")
                // Zone holding allows us to use placement over the shape
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.New.Zone(), null)));

            builder.Describe("Socket")
                // Zone holding allows us to use placement over the shape
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.ShapeFactory.Create("ContentZone", Arguments.Empty()), null)))
                .OnCreated(created =>
                {

                })
                .OnDisplaying(displaying =>
                {
                    displaying.ShapeMetadata.OnDisplaying(
                        displaying2 =>
                        {
                            var socket = displaying2.Shape;
                            ShapeMetadata metadata = socket.Metadata;
                            metadata.Alternates = new string[]{
                                "Socket_" + socket.Metadata.DisplayType,
                                "Socket__" + socket.ConnectorType,
                                "Socket_" + socket.Metadata.DisplayType + "__" + socket.ConnectorType
                            }.Concat(metadata.Alternates).ToList();
                        });

                });
            builder.Describe("Connector_Edit")
                // Zone holding allows us to use placement over the shape
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.New.Zone(), null)));

            builder.Describe("Connector")
                // Zone holding allows us to use placement over the shape
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.ShapeFactory.Create("ContentZone", Arguments.Empty()), null)))
                .OnDisplaying(displaying =>
                {
                    displaying.ShapeMetadata.OnDisplaying(
                        displaying2 =>
                        {
                            var connector = displaying2.Shape;
                            string connectorType = connector.ConnectorType;
                            string displayType = displaying2.ShapeMetadata.DisplayType;
                            displaying.ShapeMetadata.Alternates.Add("Connector_" + displayType);
                            displaying.ShapeMetadata.Alternates.Add("Connector__" + connectorType);

                        });
                });
            builder.Describe("Connector_Edit")
                // Zone holding allows some interesting positioning scenarios
                .OnCreating(creating => creating.Behaviors.Add(new ZoneHoldingBehavior(() => creating.New.Zone(), null)))
                .OnDisplaying(displaying =>
                {
                    displaying.ShapeMetadata.OnDisplaying(
                        displaying2 =>
                        {
                            var connector = displaying2.Shape;
                            string connectorType = connector.ConnectorType;
                            string displayType = displaying2.ShapeMetadata.DisplayType;
                            // Connector.RelatedContent
                            displaying.ShapeMetadata.Alternates.Add("Connector_Edit_" + displayType);
                            displaying.ShapeMetadata.Alternates.Add("Connector_Edit__" + connectorType);
                        });
                });

            builder.Describe("Graph")
                .OnCreated(created =>
                    {
                        // List connectors
                        string connectorType = created.Shape.ConnectorType;
                        // Build graph
                        var graph = _mechanics.Value.BuildGraph(connectorType);

                    })
                .OnDisplaying(displaying =>
                    {
                        // TODO: Separate feature
                        displaying.ShapeMetadata.Alternates.Add("Graph_SVG");
                    });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Display"></param>
        /// <param name="Output"></param>
        /// <param name="Items"></param>
        /// <param name="Tag"></param>
        /// <param name="Id"></param>
        /// <param name="Classes"></param>
        /// <param name="Attributes"></param>
        /// <param name="ItemClasses"></param>
        /// <param name="ItemAttributes"></param>
        [Shape]
        public void SeparatorList(
            dynamic Display,
            TextWriter Output,
            IEnumerable<dynamic> Items,
            string Tag,
            string ItemTag,
            string Id,
            string Separator,
            IEnumerable<string> Classes,
            IDictionary<string, string> Attributes,
            IEnumerable<string> ItemClasses,
            IDictionary<string, string> ItemAttributes) {

            if (Items == null)
                return;

            var count = Items.Count();
            if (count < 1)
                return;

            var outerTag = !string.IsNullOrEmpty(Tag);
            var itemTagName = string.IsNullOrEmpty(ItemTag) ? "span" : ItemTag;

            TagBuilder listTag = null;
            if (outerTag) {
                listTag = GetTagBuilder(Tag, Id, Classes, Attributes);
                Output.Write(listTag.ToString(TagRenderMode.StartTag));
            }

            var separator = String.IsNullOrEmpty(Separator) ? ", " : Separator;

            var index = 0;
            foreach (var item in Items) {
                var itemTag = GetTagBuilder(itemTagName, null, ItemClasses, ItemAttributes);
                if (index == 0)
                    itemTag.AddCssClass("first");
                if (index == count - 1)
                    itemTag.AddCssClass("last");

                Output.Write(itemTag.ToString(TagRenderMode.StartTag));
                Output.Write(Display(item));
                Output.Write(itemTag.ToString(TagRenderMode.EndTag));

                // Write separator
                // TODO: Localize, IHtmlString, even inject a shape ... This feels very hard-coded (although: from shape table events can probably change it)
                if (index < count - 1)
                    Output.Write(separator);

                ++index;
            }

            if (outerTag) {
                Output.Write(listTag.ToString(TagRenderMode.EndTag));
            }
        }

        /// <summary>
        /// Generates a link to any route values. Note: Text should be localized before passing in; sometimes it will have come from a content item which should already have been localized.
        /// TODO: Test localization scenarios with this!
        /// TODO: Support target, domain, absolute, other options
        /// </summary>
        /// <param name="Display"></param>
        /// <param name="Output"></param>
        /// <param name="Url"></param>
        /// <param name="Text"></param>
        /// <param name="Values"></param>
        /// <param name="Tag"></param>
        /// <param name="Id"></param>
        /// <param name="Classes"></param>
        /// <param name="Attributes"></param>
        [Shape]
        public void Link(
            dynamic Display,
            TextWriter Output,
            UrlHelper Url,
            string Text,
            RouteValueDictionary Values,
            string Tag,
            string Id,
            IEnumerable<string> Classes,
            IDictionary<string, string> Attributes
            ) {
            var linkTagName = string.IsNullOrEmpty(Tag) ? "a" : Tag;
            TagBuilder linkTag = GetTagBuilder(linkTagName, Id, Classes, Attributes);

            linkTag.MergeAttribute("href", Url.RouteUrl(Values), true);

            Output.Write(linkTag.ToString(TagRenderMode.StartTag));
            Output.Write(Text);
            Output.Write(linkTag.ToString(TagRenderMode.EndTag));
        }

        static TagBuilder GetTagBuilder(string tagName, string id, IEnumerable<string> classes, IDictionary<string, string> attributes) {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(attributes, false);
            foreach (var cssClass in classes ?? Enumerable.Empty<string>())
                tagBuilder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(id))
                tagBuilder.GenerateId(id);
            return tagBuilder;
        }
    }
}
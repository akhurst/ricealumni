using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement.Aspects;
using Downplay.Mechanics.Framework;
using Orchard.Utility.Extensions;

namespace Downplay.Mechanics.Services
{
    public static class MechanicsExtensions
    {
        /// <summary>
        /// Load two content items by ID and create the connector
        /// </summary>
        /// <param name="mechanics"></param>
        /// <param name="leftId"></param>
        /// <param name="rightId"></param>
        /// <param name="connectorType"></param>
        /// <returns></returns>
        public static IEnumerable<IConnector> CreateConnector(this IMechanicsService mechanics, int leftId, int rightId, string connectorType, bool ignorePermissions = false)
        {
            // Pull content items
            var leftItem = mechanics.Services.ContentManager.Get(leftId);
            if (leftItem == null) throw new Exception(String.Format("Failed to create connector: Left Item not found with Id: {0}", leftId));
            return mechanics.CreateConnector(leftItem, rightId,connectorType,ignorePermissions);
        }
        /// <summary>
        /// Loads right item by Id and creates the connector
        /// </summary>
        /// <param name="mechanics"></param>
        /// <param name="leftId"></param>
        /// <param name="rightId"></param>
        /// <param name="connectorType"></param>
        /// <returns></returns>
        public static IEnumerable<IConnector> CreateConnector(this IMechanicsService mechanics, IContent leftItem, int rightId, string connectorType, bool ignorePermissions = false)
        {
            var rightItem = mechanics.Services.ContentManager.Get(rightId);
            if (rightItem == null) throw new Exception(String.Format("Failed to create connector: Right Item not found with Id: {0}", rightId));
            return mechanics.CreateConnector(leftItem, rightItem, connectorType, ignorePermissions);
        }
        /*
        /// <summary>
        /// Check for Connector of named type(s) from a right item
        /// </summary>
        public static IContentQuery<ConnectorPart, ConnectorPartRecord> LeftConnectors(this IMechanicsService mechanics, IContent right, params string[] connectorTypes)
        {
            var rightId = right.Id;
            return mechanics.Connectors(connectorTypes).Where<ConnectorPartRecord>(c => c.RightContentItem_id == rightId);
        }
        /// <summary>
        /// Check for Connector of named type(s) with specified parts from a right item
        /// </summary>
        public static IContentQuery<TPart> LeftConnectors<TPart>(this IMechanicsService mechanics, IContent right, params string[] connectorTypes)
            where TPart : IContent
        {
            return mechanics.LeftConnectors(right,connectorTypes).ForPart<TPart>();
        }
        /*
        /// <summary>
        /// Check for Connector of named type(s) between two specific items
        /// </summary>
        public static IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(this IMechanicsService mechanics, IContent left, IContent right, params string[] connectorTypes)
        {
            return mechanics.Connectors(left.Id, right.Id, connectorTypes);
        }*/
        /*
        public static IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(this IMechanicsService mechanics, int leftId, params string[] connectorTypes)
        {
            return mechanics.Connectors(connectorTypes).Where(c => c.LeftContentItem_id == leftId);
        }
        public static IContentQuery<ConnectorPart, ConnectorPartRecord> LeftConnectors(this IMechanicsService mechanics, int rightId, params string[] connectorTypes)
        {
            return mechanics.Connectors(connectorTypes).Where(c => c.RightContentItem_id == rightId);
        }

        public static IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(this IMechanicsService mechanics, int leftId, int rightId, params string[] connectorTypes)
        {
            return mechanics.Connectors(connectorTypes).Where(c => c.LeftContentItem_id == leftId && c.RightContentItem_id == rightId);
        }
        /// <summary>
        /// Check whether given connection exists
        /// </summary>
        public static bool ConnectorExists(this IMechanicsService mechanics, IContent leftItem, IContent rightItem, string connectorType)
        {
            return (mechanics.Connectors(leftItem.Id, rightItem.Id, connectorType).Count() > 0);
        }
        */

        public static string GetTitle(this IContent target)
        {
            // First and best choice
            var titled = target.ContentItem.As<ITitleAspect>();
            if (titled != null)
            {
                return titled.Title;
            }

            // Use Id or Routable title
            // TODO: Localization of title here perhaps
            var title = target.ContentItem.ContentType + " " + target.Id.ToString();
            dynamic content = target.ContentItem;

            // Site name?
            // ... Hmm ...

            // User name? - Is really messy due to no null check available on dynamic
            // content item parts (and avoiding assembly reference)
            if (target.ContentItem.Parts.Any(p=>p.PartDefinition.Name=="UserPart")) // content.UserPart != null)
            {
                title = content.UserPart.UserName;
            }

            return title;
        }

        public static string GetBody(this IContent target)
        {
            string desc = null;

            // TODO: Localization of desc here perhaps
            dynamic content = target.ContentItem;

            /*
            if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "MetaPart"))
            {
                desc = content.MetaPart.Description;
            }
            if (desc == null)
            {*/
                // Fall thru to concatenated body
                if (target.ContentItem.Parts.Any(p => p.PartDefinition.Name == "BodyPart"))
                {
                    desc = ((string)content.BodyPart.Text); //.RemoveTags().Ellipsize(255);
                }
                else
                {
                    desc = target.GetTitle();
                }
//            }

            return desc;
        }

        public static void With<T>(this IContent content, Action<T> action) where T : IContent
        {
            T with = content.As<T>();
            if (with != null)
            {
                action.Invoke(with);
            }
        }

        public static void WithPart<T>(this IEnumerable<ContentItem> items, Action<T> action) where T : IContent
        {
            foreach (var content in items.AsPart<T>())
            {
                action.Invoke(content);
            }
        }
        public static ContentTypeDefinition ConnectorTypeDefinition(this IMechanicsService mechanics, string connectorType)
        {
            return mechanics.ConnectorTypeDefinitions().FirstOrDefault(def => def.Name == connectorType);
        }
        public static ContentTypePartDefinition ConnectorPartDefinition(this IMechanicsService mechanics, string connectorType)
        {
            var connector = ConnectorTypeDefinition(mechanics, connectorType);
            if (connector == null) return null;
            return connector.Parts.FirstOrDefault(p => p.PartDefinition.Name == "ConnectorPart");
        }
        public static ConnectorTypePartSettings ConnectorSettings(this IMechanicsService mechanics, string connectorType)
        {
            var part = ConnectorPartDefinition(mechanics,connectorType);
            if (part == null) return null;
            return part.Settings.TryGetModel<ConnectorTypePartSettings>();
        }
    }
}
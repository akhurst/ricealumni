using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Services
{
    public interface IMechanicsService : IDependency
    {
        IOrchardServices Services { get; }

        /// <summary>
        /// Connectors and settings allowed for specific content item
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        IEnumerable<ConnectorDescriptor> AllowableConnectorTypes(IContent part);

        /// <summary>
        /// All type part definitions with connector parts
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContentTypeDefinition> ConnectorTypeDefinitions();

        /// <summary>
        /// Gets all Connector items of a specified type. You have to examine the ConnectorPart.RightContentItem to get the item *related* to it.
        /// </summary>
        IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(params string[] connectorTypes);
        IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(IContent left, ConnectorCriteria criteria = ConnectorCriteria.Auto, params string[] connectorTypes);
        IContentQuery<ConnectorPart, ConnectorPartRecord> LeftConnectors(IContent right, ConnectorCriteria criteria = ConnectorCriteria.Auto, params string[] connectorTypes);

        /// <summary>
        /// Creates a Connector from content item left to content item right, using Connector content type
        /// </summary>
        /// <param name="leftId"></param>
        /// <param name="rightId"></param>
        /// <param name="connectorType"></param>
        /// <param name="ignorePermissions">Optionally ignore permissions for the current user; for if you are automating creation of connections and user's role is irrelevant</param>
        /// <returns>Connector item(s) created - could include inverse and other required connectors</returns>
        IEnumerable<IConnector> CreateConnector(IContent left, IContent right, string connectorType,bool ignorePermissions=false);

        /// <summary>
        /// Deletes a connector and any matching inverse connector
        /// </summary>
        /// <param name="left">Left-hand content to delete from (required for version tracking and safety)</param>
        /// <param name="connector">The connector to be deleted</param>
        /// <param name="ignorePermissions">Optionally ignore permissions for the current user; for if you are automating creation of connections and user's role is irrelevant</param>
        /// <returns>Number of connectors deleted (could include inverse connector)</returns>
        int DeleteConnector(IContent left, IContent content, bool ignorePermissions = false, bool deleteInverseIfPossible = true, bool definitelyDelete = false);
        
        /// <summary>
        /// Experimental building graphs of connector networks for more customised rendering and other processing
        /// </summary>
        /// <param name="connectorType"></param>
        /// <returns></returns>
        Graph BuildGraph(string connectorType);

        IEnumerable<SocketsPart> RightItemsFromConnectors(IEnumerable<ConnectorPart> connectors, string[] types = null, bool getDrafts = false);
        IEnumerable<SocketsPart> LeftItemsFromConnectors(IEnumerable<ConnectorPart> connectors, string[] types = null, bool getDrafts = false);

        /// <summary>
        /// Recursively scans connectors (both left and right) from the supplied item, and yields all hits. Could be slow. High chance of hitting every content item
        /// in the database with complex connector graphs. Storing the whole graph would be a good candidate for cache optimisation, could do a lot of things pretty quickly
        /// with an in-memory graph.
        /// </summary>
        /// <param name="iContent"></param>
        /// <returns></returns>
        // IEnumerable<IContent> ConnectorScanRecursive(IContent content);

        ConnectorDescriptor DescribeConnector(string key);
    }
}

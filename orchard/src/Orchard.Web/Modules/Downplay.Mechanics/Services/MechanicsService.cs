using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using System.Xml.Linq;
using Orchard.ContentManagement.Records;
using Downplay.Mechanics.Framework;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.Logging;
using System.Diagnostics;
using Orchard.Core.Common.Models;

namespace Downplay.Mechanics.Services
{
    public class MechanicsService : IMechanicsService
    {
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; } 
        private IContentDefinitionManager _contentDefinitionManager;
        private Lazy<IEnumerable<ISocketHandler>> _socketHandlers;
        private Lazy<IEnumerable<IConnectorHandler>> _connectorHandlers;

        public MechanicsService(
            IContentDefinitionManager contentDefinitionManager,
            IOrchardServices orchardServices,
            Lazy<IEnumerable<ISocketHandler>> socketHandlers,
            Lazy<IEnumerable<IConnectorHandler>> connectorHandlers
            )
        {
            _contentDefinitionManager = contentDefinitionManager;
            _socketHandlers = socketHandlers;
            _connectorHandlers = connectorHandlers;
            Services = orchardServices;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IEnumerable<ContentTypeDefinition> ConnectorTypeDefinitions()
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(d=>d.Parts.Any(p=>p.PartDefinition.Name==typeof(ConnectorPart).Name));
        }

        public ConnectorDescriptor DescribeConnector(string ConnectorType) {
            if (String.IsNullOrWhiteSpace(ConnectorType)) {
                throw new ArgumentException("ConnectorType is empty, can't make connector", "ConnectorType");
            }
            var ConnectorDef = _contentDefinitionManager.GetTypeDefinition(ConnectorType);
            if (ConnectorDef == null) throw new Exception(String.Format("Failed to create connector: Type definition not found: {0}", ConnectorType));
            return new ConnectorDescriptor(ConnectorDef);
        }

        public IContentQuery<ConnectorPart,ConnectorPartRecord> Connectors(params string[] connectorTypes)
        {
            var query = Services.ContentManager.Query<ConnectorPart,ConnectorPartRecord>(connectorTypes);
            // Use query hints for singular connector types
            if (connectorTypes.Count() == 1) {
                query = query.WithQueryHintsFor(connectorTypes.First());
            }
            return query;
        }
        /// <summary>
        /// Check for Connector of named type(s) from a left item
        /// </summary>
        public IContentQuery<ConnectorPart, ConnectorPartRecord> Connectors(IContent left, ConnectorCriteria criteria = ConnectorCriteria.Auto, params string[] connectorTypes)
        {
            var leftId = left.Id;
            var query = Connectors(connectorTypes);
            // Always get published items
            query = query.Where<ConnectorPartRecord>(c => c.LeftContentItem_id == leftId);
            query = ApplyConnectorCriteria(left, criteria, query);

            return query;

            /*
            // Version id we're looking for
            var leftVersionId = left.ContentItem.VersionRecord.Id;
            query = query.ForVersion(VersionOptions.AllVersions).Where<ConnectorPartRecord>(c =>
                c.LeftContentItem_id == leftId
                && !c.DeleteWhenLeftPublished
                && (c.LeftContentVersionId==null || c.LeftContentVersionId == leftVersionId));
            return query;
             */
        }

        private IContentQuery<ConnectorPart, ConnectorPartRecord> ApplyConnectorCriteria(IContent left, ConnectorCriteria criteria, IContentQuery<ConnectorPart, ConnectorPartRecord> query) {
            if (criteria == ConnectorCriteria.Auto)
                criteria = (!left.IsPublished() && left.ContentItem.VersionRecord != null && left.ContentItem.VersionRecord.Latest) ? ConnectorCriteria.Drafts : ConnectorCriteria.Published;
            switch (criteria) {
                case ConnectorCriteria.Published:
                    // Always show published
                    query = query.ForVersion(VersionOptions.Published);
                    break;
                case ConnectorCriteria.Drafts:
                    // Drafted item, get Latest (but exclude deleted)
                    query = query.ForVersion(VersionOptions.Latest).Where(c => !c.DeleteWhenLeftPublished);
                    break;
                case ConnectorCriteria.DraftsAndDeleted:
                    // Drafted item, get Latest (but exclude deleted)
                    query = query.ForVersion(VersionOptions.Latest);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Check for Connector of named type(s) from a left item
        /// </summary>
        public IContentQuery<ConnectorPart, ConnectorPartRecord> LeftConnectors(IContent right, ConnectorCriteria criteria = ConnectorCriteria.Auto, params string[] connectorTypes) {
            var rightId = right.Id;
            var query = Connectors(connectorTypes);
            // Always get published items
            query = query.Where<ConnectorPartRecord>(c => c.RightContentItem_id == rightId);
            query = ApplyConnectorCriteria(right, criteria, query);
            return query;
        }
        // TODO: Not used, stopped all kinds of stuff working
        private IEnumerable<string> QueryHintRecordsForType(string contentType) {
            var contentItem = Services.ContentManager.New(contentType);
            foreach (var part in contentItem.Parts) {
                var partType = part.GetType().BaseType;
                if (partType.IsGenericType && partType.GetGenericTypeDefinition() == typeof(ContentPart<>)) {
                    var recordType = partType.GetGenericArguments().Single();
                    yield return recordType.Name;
                }
            }
        }

        /// <summary>
        /// Should be available somewhere in core?
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private QueryHints QueryHintsForTypes(params string[] types) {

            var hints = QueryHints.Empty;
            hints = hints.ExpandRecords(types.SelectMany(QueryHintRecordsForType).Distinct());
            return hints;
            /* TODO: Could do something like this if QueryHintRecordsForType() proves slow...
            foreach (var type in types) {
                var def = _contentDefinitionManager.GetTypeDefinition(type);
                foreach (var part in def.Parts) {


                }
            }
            return hints;
             * */
        }

        public IEnumerable<SocketsPart> RightItemsFromConnectors(IEnumerable<ConnectorPart> connectors, string[] types = null, bool getDrafts = false) {
            var rightIds = connectors.Select(c => c.RightContentItemId);
            // TODO: Stuck between a rock and a hard place with QueryHints. WithQueryHintsFor actually generates a new content item in memory to figure out the parts to be expanded.
            // We possibly can't assume this will be optimal in all scenarios. On the other hand, we need to do something fairly complex to get the generic types from all the drivers.
            // So for now only get hints if it's a single type.
            var qh = QueryHints.Empty;
            if (types!= null && types.Length == 1) {
                qh = QueryHintsForTypes(types);
            }
            var content = Services.ContentManager.GetMany<SocketsPart>(rightIds, getDrafts ? VersionOptions.Latest : VersionOptions.Published, qh);
            return content;
        }
        public IEnumerable<SocketsPart> LeftItemsFromConnectors(IEnumerable<ConnectorPart> connectors, string[] types = null, bool getDrafts = false) {
            var leftIds = connectors.Select(c => c.LeftContentItemId);
            // TODO: Stuck between a rock and a hard place with QueryHints. WithQueryHintsFor actually generates a new content item in memory to figure out the parts to be expanded.
            // We possibly can't assume this will be optimal in all scenarios. On the other hand, we need to do something fairly complex to get the generic types from all the drivers.
            // So for now only get hints if it's a single type.
            var qh = QueryHints.Empty;
            if (types != null && types.Length == 1) {
                qh = QueryHintsForTypes(types);
            }
            var content = Services.ContentManager.GetMany<SocketsPart>(leftIds, getDrafts ? VersionOptions.Latest : VersionOptions.Published, qh);
            return content;
        }
/*
 *  Old version prior to versioning simplification:
        public IEnumerable<SocketsPart> RightItemsFromConnectors(IEnumerable<ConnectorPart> connectors, params string[] types)
        {
            var ids = connectors.Select(c => new { VersionId = c.RightContentVersionId, Id = c.RightContentVersionId.HasValue ? null : (int?)c.RightContentItemId }).ToList();
            var rightIds = ids.Where(c=>c.Id.HasValue).Select(c=>c.Id.Value);
            var rightVersionIds = ids.Where(c => c.VersionId.HasValue).Select(c => c.VersionId.Value);
            // TODO: Stuck between a rock and a hard place with QueryHints. WithQueryHintsFor actually generates a new content item in memory to figure out the parts to be expanded.
            // We possibly can't assume this will be optimal in all scenarios. On the other hand, we need to do something fairly complex to get the generic types from all the drivers.
            var qh = QueryHints.Empty; // QueryHintsForTypes(types);
            var content = Services.ContentManager.GetManyByVersionId<SocketsPart>(rightVersionIds, qh)
                .Concat(Services.ContentManager.GetMany<SocketsPart>(rightIds, VersionOptions.Latest, qh));
            return content;
        }
        public IEnumerable<SocketsPart> LeftItemsFromConnectors(IEnumerable<ConnectorPart> connectors, params string[] types)
        {
            var ids = connectors.Select(c => new { VersionId = c.LeftContentVersionId, Id = c.LeftContentVersionId.HasValue ? null : (int?)c.LeftContentItemId }).ToList();
            var leftIds = ids.Where(c => c.Id.HasValue).Select(c => c.Id.Value);
            var leftVersionIds = ids.Where(c => c.VersionId.HasValue).Select(c => c.VersionId.Value);
            // TODO: Stuck between a rock and a hard place with QueryHints. WithQueryHintsFor actually generates a new content item in memory to figure out the parts to be expanded.
            // We possibly can't assume this will be optimal in all scenarios. On the other hand, we need to do something fairly complex to get the generic types from all the drivers.
            var qh = QueryHintsForTypes(types);
            var content = Services.ContentManager.GetManyByVersionId<SocketsPart>(leftVersionIds, qh)
                .Concat(Services.ContentManager.GetMany<SocketsPart>(leftIds, VersionOptions.Latest, qh));
            return content;
        }
        */
        public IEnumerable<ConnectorDescriptor> AllowableConnectorTypes(IContent content) {
            // Enforce sockets part.
            // TODO: Throw an error - this shouldn't be called on anything without sockets
            var part = content.As<SocketsPart>();
            if (part == null) {
                return Enumerable.Empty<ConnectorDescriptor>();
            }

            var ConnectorTypes = ConnectorTypeDefinitions();
            // TODO: PERF: This can't be optimal with a lot of content types floating around, need a lookup.
            return ConnectorTypes.Select(d => {
                var s = d.Parts.First(p => p.PartDefinition.Name == typeof(ConnectorPart).Name).Settings.GetModel<ConnectorTypePartSettings>();
                var allowTypes = s == null ? Enumerable.Empty<string>() : s.ListAllowedContentLeft();
                return new {
                    Settings = s,
                    Definition = d,
                    Allowed = (s!=null && (!allowTypes.Any() || allowTypes.Any(t => t == part.TypeDefinition.Name)))
                };
            }).Where(s=>s.Allowed).Select(s=>{
                var def = new ConnectorDescriptor(s.Definition, s.Settings);
                return def;
            });
        }

        public IEnumerable<IConnector> CreateConnector(IContent left, IContent right, string ConnectorType, bool ignorePermissions = false)
        {
            var conn = DescribeConnector(ConnectorType);
            return CreateConnector(left, right, conn, true, ignorePermissions);
        }

        protected IEnumerable<IConnector> CreateConnector(IContent left, IContent right, ConnectorDescriptor ConnectorDef, bool createInverseIfPossible, bool ignorePermissions = false)
        {
            // Check sockets
            var leftSock = left.As<SocketsPart>();
            var rightSock = right.As<SocketsPart>();

            // TODO: SocketsPart doesn't need any data so we could consider automatically welding it to any content that has valid connectors. It's 
            // very rare that we want a connector type to apply to anything so maybe should disable the wildcard.
            if (leftSock == null || rightSock == null) throw new OrchardException(T("Attempted to create connector between non-socket content %0 (%1) and %2 (%3). You must add SocketsPart to content to participate in connections.", left.ContentItem.ContentType, leftSock == null ? "hasn't" : "has", right.ContentItem.ContentType, rightSock == null ? "hasn't" : "has"));

            // Build a new item
            var connector = Services.ContentManager.New<ConnectorPart>(ConnectorDef.Name);
            
            // Check security
            var createContext = new ConnectorCreateContext(left, right, ConnectorDef);
            createContext.ConnectorContent = connector;
            createContext.SiblingConnectors = leftSock.Sockets[ConnectorDef.Name].ConnectorQuery;

            if (!ignorePermissions && !Services.Authorizer.Authorize(Permissions.PublishContent, connector, T("Cannot create connector"))) {
                return Enumerable.Empty<IConnector>();
            }

            // Store left and right items
            connector.LeftContentItemId = left.ContentItem.Id;
            connector.RightContentItemId = right.ContentItem.Id;

            // Handle versioning
            if (left.ContentItem.VersionRecord != null)
            {
                connector.LeftContentVersionId = left.ContentItem.VersionRecord.Id;
            }
            if (right.ContentItem.VersionRecord != null)
            {
                connector.RightContentVersionId = right.ContentItem.VersionRecord.Id;
            }

            // Invoke Creating event
            _connectorHandlers.Value.Invoke(c=>c.Creating(createContext),Logger);
            if (createContext.Cancel) return Enumerable.Empty<IConnector>();

            // Create the item and invoke Created
            Services.ContentManager.Create(connector,((left.ContentItem.VersionRecord!=null)? (left.ContentItem.VersionRecord.Published ? VersionOptions.Published : VersionOptions.Draft) : VersionOptions.Published));
            _connectorHandlers.Value.Invoke(c=>c.Created(createContext),Logger);

            IEnumerable<IConnector> returnList = new[] { connector };

            if (ConnectorDef.PartDefinition != null)
            {
                var settings = ConnectorDef.Settings;
                // Can we create an inverse?
                if (createInverseIfPossible && !String.IsNullOrWhiteSpace(settings.InverseConnectorType)) {
                    var inverseDef = DescribeConnector(settings.InverseConnectorType);


                    var inverseContext = new ConnectorCreateContext(right, left, inverseDef);
                    inverseContext.InverseConnectorContent = createContext.ConnectorContent;
                    _connectorHandlers.Value.Invoke(c => c.CreatingInverse(inverseContext), Logger);
                    if (!inverseContext.Cancel) {
                        // Create it
                        var inverseCreate = CreateConnector(right, left, inverseDef, false, ignorePermissions);
                        var inverseFirst = inverseCreate.FirstOrDefault().As<ConnectorPart>();

                        // Exchange Ids
                        if (inverseFirst != null) {
                            connector.InverseConnector = inverseFirst;
                            inverseFirst.InverseConnector = connector;
                        }
                        returnList = returnList.Concat(inverseCreate);
                    }

                }
            }
            return returnList.ToArray();
        }

        public int DeleteConnector(IContent left, IContent content, bool ignorePermissions = false, bool deleteInverseIfPossible = true, bool definitelyDelete = false)
        {
            var count = 0;
            var connector = content.As<ConnectorPart>();
            if (connector == null) return count;
            var connectorPart = connector.TypePartDefinition;
            if (connectorPart != null)
            {
                // Security check
                if (!ignorePermissions)
                {
                    if (!Services.Authorizer.Authorize(Permissions.DeleteContent, content, T("Could not delete connector")))
                    {
                        return count;
                    }
                }

                if (connector.LeftContentItemId != left.Id)
                {
                    throw new OrchardException(T("Attempted to delete connector from wrong join"));
                }
                
                count++;

                // Versioning. If we delete the connector immediately from a draft then it'll also disappear from the published version.
                // Can't delete connectors from old versions.
                if (left.ContentItem.VersionRecord != null ) {
                    if (left.IsPublished() || definitelyDelete)
                    {
                        // Do a proper remove
                        Services.ContentManager.Remove(content.ContentItem);

                        // TODO: Events to hook into CRUD operations on connectors (so we could delete other related things for example)
                        // TODO: Actually this as well as Create inverse should be done in content handler so we can hook into all normal
                        // content events...?
                        // Check for existing inverse Connector
                        // Note: We don't check permissions on inverse connector, if they can delete one way they have to be able to delete the other.
                        if (deleteInverseIfPossible && connector.InverseConnector != null) {
                            Services.ContentManager.Remove(connector.InverseConnector.ContentItem);
                            count++;
                        }
                    }
                    else {
                        // Flag to delete when published
                        connector.DeleteWhenLeftPublished = true;
                    }
                }
            }
            return count;
        }

        #region Graph visualization (not yet implemented)

        public Graph BuildGraph(string connectorType)
        {
            throw new NotImplementedException();
            // TODO: Allowing other objects to hook into the graph building process. Maybe this needs Origami. But it solves a variety
            //       of problems quite neatly instead of mangling the ContentItem display process. Although that could be useful as well
            //       e.g. if we wanted to push certain connection types out into zones for "related content" approach.
     /*       var graph = new Graph();
            if (String.IsNullOrWhiteSpace(connectorType))
            {
                var connector = (from def in _contentDefinitionManager.ListTypeDefinitions()
                                 where def.Parts.Any(p => p.PartDefinition.Name == "ConnectorPart")
                                 orderby def.Name
                                 select def).FirstOrDefault();
                if (connector == null) return graph;
                connectorType = connector.Name;
            }
            // Grab all connectors
            var connectors = Services.ContentManager.Query<ConnectorPart>(connectorType).List();

            foreach (var connector in connectors)
            {
                AddConnectorToGraph(graph, connector);
            }
            return graph;*/
        }
        /*
        protected void AddConnectorToGraph(Graph graph, ConnectorPart connector)
        {
            // Check for LHS node
            var leftNode = graph.GetNode(connector.LeftContentItemId);
            if (leftNode == null)
            {
                var leftItem = Services.ContentManager.Get(connector.LeftContentItemId);
                leftNode = graph.MakeNode(leftItem);
            }
            var rightNode = graph.GetNode(connector.RightContentItemId);
            if (rightNode == null)
            {
                var rightItem = Services.ContentManager.Get(connector.RightContentItemId);
                rightNode = graph.MakeNode(rightItem);
            }
        }
        */
        #endregion
        /*
        public IEnumerable<IContent> ConnectorScanRecursive(IContent content)
        {
            // Initialize tracking list so we never scan same item twice
            var ids = new List<int>();
            ids.Add(content.Id);

            foreach (var item in ConnectorScanRecursive(content, ids))
            {
                yield return item;
            }
        }

        private IEnumerable<IContent> ConnectorScanRecursive(IContent content, List<int> ids)
        {
            // Scan right
            var right = RightItemsFromConnectors(this.Connectors(content).Where<ConnectorPartRecord>(c => !ids.Contains(c.RightContentItem_id)).List());
            ids.AddRange(right.Select(r => r.Id));
            foreach (var r in right)
            {
                yield return r;
                // Subscan
                foreach (var sub in ConnectorScanRecursive(r, ids))
                {
                    yield return sub;
                }
            }

            var left = LeftItemsFromConnectors(this.LeftConnectors(content).Where<ConnectorPartRecord>(c => !ids.Contains(c.LeftContentItem_id)).List());
            ids.AddRange(left.Select(r => r.Id));
            foreach (var l in left)
            {
                yield return l;
                // Subscan
                foreach (var sub in ConnectorScanRecursive(l, ids))
                {
                    yield return sub;
                }
            }
        }
        */
   }
}
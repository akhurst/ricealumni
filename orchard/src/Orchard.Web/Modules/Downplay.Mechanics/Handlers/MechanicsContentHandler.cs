using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Downplay.Mechanics.Models;
using Orchard.Data;
using Downplay.Mechanics.Services;
using Orchard.Caching;
using Downplay.Mechanics.Drivers;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard;
using Downplay.Mechanics.Framework;
using Orchard.Logging;
using Orchard.ContentManagement.Aspects;

namespace Downplay.Mechanics.Handlers
{
    public class MechanicsContentHandler : ContentHandler
    {

        private readonly Lazy<IMechanicsService> _mechanics;
        private readonly ISignals _signals;
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }
        public MechanicsContentHandler(
            IRepository<SocketsPartRecord> repository,
            IRepository<ConnectorPartRecord> connectorRepository, 
            Lazy<IMechanicsService> mechanics,
            IOrchardServices services,
            ISignals signals)
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;

            _mechanics = mechanics;
            _signals = signals;
            Services = services;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(StorageFilter.For(connectorRepository));

        }

        #region Generic handler overrides

        protected override void  Activated(ActivatedContentContext context)
        {
            var connector = context.ContentItem.As<ConnectorPart>();
            if (connector != null) {
                ConnectorPropertySetHandlers(context, connector);
            }
            var socket = context.ContentItem.As<SocketsPart>();
            if (socket != null) {
                LazyLoadSocketHandlers(socket);
            }
        }

        protected override void Creating(CreateContentContext context)
        {
 	         base.Creating(context);
        }

        protected override void  Created(CreateContentContext context)
        {
            var socket = context.ContentItem.As<SocketsPart>();
            if (socket!=null) {
                 // TODO: But we perhaps don't need to do this since ConnectorCollection tracks removed and added objects,
                // ideally we can drop this and let everything happen at the end of transaction scope.
                // If updating, perform connector persistence
                socket.Sockets.AllSockets.Invoke(c=>{c.Connectors.Flush(_mechanics.Value);}, Logger);
            }
  	        base.Created(context);
        }
        protected override void Loading(LoadContentContext context)
        {
            if (context.ContentItem.Is<ConnectorPart>()) {
 	            ConnectorPropertyGetHandlers(context.ContentItem.As<ConnectorPart>());
            }
        }
        protected override void  Publishing(PublishContentContext context)
        {
            base.Publishing(context);
        }
        protected override void  Published(PublishContentContext context)
        {
            // Check inverse of connector
            var part = context.ContentItem.As<ConnectorPart>();
            if (part!=null) {
                if (part.InverseConnector != null) {
                    // Make sure inverse has the correct version
                    part.InverseConnector.RightContentVersionId = part.LeftContentVersionId;
                    // Publish the inverse
                    if (!part.InverseConnector.IsPublished()) {
                        Services.ContentManager.Publish(part.InverseConnector.ContentItem);
                    }
                }
            } 
            // Publish all connectors of socket
            var socket = context.ContentItem.As<SocketsPart>();
            if (socket!=null) {
                socket.Sockets.Flush();
                PublishConnectors(socket);
                // TODO: PERF: Could end up with a lot of overhead and triggering all the time when, say, adding comments or any old thing. Actually only need to do this for
                // specific things like menus.
                Trigger(socket);
            }
        }

        protected override void  Updating(UpdateContentContext context)
        {
 	         base.Updating(context);
        }
        protected override void  Updated(UpdateContentContext context)
        {
            var socket = context.ContentItem.As<SocketsPart>();
            if (socket!=null) {
                // TODO: Perhaps there's a way not to do this since ConnectorCollection tracks removed and added objects,
                // ideally we can drop this and let everything happen at the end of transaction scope.
                // When using the API a call to flush is manually needed except when Creating/Publishing.
                socket.Sockets.Flush();
            }
        }
        protected override void  Unpublishing(PublishContentContext context)
        {
            var part = context.ContentItem.As<SocketsPart>();
            if (part!=null)
                UnpublishSocket(part);
        }
        protected override void  Unpublished(PublishContentContext context)
        {
 	        base.Unpublished(context);
        }
        protected override void  Removing(RemoveContentContext context)
        {
        }
        protected override void  Removed(RemoveContentContext context)
        {
            // When an item has been deleted, remove any connectors
            var part = context.ContentItem.As<SocketsPart>();
            if (part != null)
                RemoveSocket(part);

 	         base.Removed(context);
        }
        protected override void Versioning(VersionContentContext context) {
            VersioningSocket(context);
        }

        private void VersioningSocket(VersionContentContext context) {
            var partOld = context.ExistingContentItem.As<SocketsPart>();
            var partNew = context.BuildingContentItem.As<SocketsPart>();
            if (partOld != null && partNew != null) {
                // Generate drafts for all the connectors
                var connectors = partOld.Sockets.AllSockets.SelectMany(q=>q.ConnectorItems);
                /* TODO: Lazy version ...
                var drafts = new Lazy<IEnumerable<ConnectorPart>>(() => 
                    connectors.Value.Select(c=>{
                    var draft = Services.ContentManager.GetDraftRequired<ConnectorPart>(c.ContentItem.Id);
                    // Store the version id of the new version
                    draft.As<ConnectorPart>().LeftContentVersionId = partNew.ContentItem.VersionRecord.Id;
                    return draft;
                    }));
                var inverses = new Lazy<IEnumerable<ConnectorPart>>(() => {

                });
                */
                var inverses = new List<ConnectorPart>();
                // TODO: This needs some revision; e.g. on a blog with 1000s of posts, it seems a bit crazy to create connector drafts for all of them,
                // perhaps only ones we even display an editor for, and in fact only ones that are even edited. The big problem is how to be aware of this.
                var drafts = new List<ConnectorPart>();
                foreach (var c in connectors) {
                    var draft = Services.ContentManager.GetDraftRequired<ConnectorPart>(c.ContentItem.Id);
                    if (draft == null) continue;
                    // Store the version id of the new version
                    draft.LeftContentVersionId = partNew.ContentItem.VersionRecord.Id;
                    drafts.Add(draft);

                    if (c.InverseConnector != null) {
                        // Make inverse draft
                        var inverse = Services.ContentManager.GetDraftRequired<ConnectorPart>(c.InverseConnector.ContentItem.Id);
                        inverse.RightContentVersionId = partNew.ContentItem.VersionRecord.Id;
                        ConnectorPropertyGetHandlers(draft, () => partNew, null, () => inverse);
                        ConnectorPropertyGetHandlers(inverse, null, () => partNew, () => draft);
                    }
                    else {
                        ConnectorPropertyGetHandlers(draft, () => partNew, null, null);
                    }
                }

                LazyLoadSocketHandlers(partNew,drafts);
            }
        }
        protected override void  Versioned(VersionContentContext context)
        {
     	    base.Versioned(context);
        }
        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            // Move all sockets onto their own Settings page.
            var sockets = context.ContentItem.As<SocketsPart>();
            if (sockets != null) {
                var types = sockets.Sockets.Allowed;
                foreach (var t in types) {
                    if (!String.IsNullOrWhiteSpace(t.Settings.SocketGroupName)) {
                        context.Metadata.EditorGroupInfo.Add(new GroupInfo(T(t.Settings.SocketGroupName)));
                    }
                }
            }
            // Look for a connector with a title
            var connector = context.ContentItem.As<ConnectorPart>();
            if (connector != null) {

                var title = connector.As<ITitleAspect>();
                if (title != null) {
                    // If empty title, take it from the RHS
                    // TODO: Consider doing this for all connectors, not just ones with title...

                    if (String.IsNullOrWhiteSpace(title.Title)) {
                        context.Metadata.DisplayText = connector.ContentItem.ContentManager.GetItemMetadata(connector.RightContent).DisplayText;
                    }
                }
            }

        }
        #endregion
        #region Sockets handling

        /// <summary>
        /// Instance lazy loaded handlers
        /// </summary>
        /// <param name="loading"></param>
        /// <param name="part"></param>
        protected void LazyLoadSocketHandlers(SocketsPart part, IEnumerable<ConnectorPart> draftedConnectors = null) {
            part.Endpoint = new SocketEndpoint(part);
            part.Sockets = new SocketFactory(_mechanics.Value);
            part.Sockets.AllowedLoader(() => _mechanics.Value.AllowableConnectorTypes(part));
            // TODO: We could DI the factory but would this really help anything?
            part.Sockets.SocketsLoader(new SocketQueryFactory(Services, _mechanics.Value, part.Endpoint, draftedConnectors));
        }

        private IEnumerable<ConnectorPart> AllConnectors(SocketsPart part) {
            var allowed = part.Sockets.Allowed.ToList();
            return allowed.SelectMany(d => part.Sockets.Socket(d.Name).Connectors.List()).Select(c=>c.As<ConnectorPart>());
        }

        private void PublishConnectors(SocketsPart part)
        {
            if (part.ContentItem.VersionRecord != null) {
                // Make sure we get drafts; otherwise we might just get the last published ones now the left item is published
                foreach (var item in _mechanics.Value.Connectors(part, ConnectorCriteria.DraftsAndDeleted).List())
                {
                    // Handle connections flagged for deletion
                    if (item.DeleteWhenLeftPublished) {
                        // Ignore permissions because we already had to get thru the permissions check to set DeleteWhenLeftPublished
                        // (left item is now published so we'll get it definitely deleted this time)
                        _mechanics.Value.DeleteConnector(part, item, true);
                    }
                    else {
                        // Delete orphans
                        if (item.RightContent == null) {
                            _mechanics.Value.DeleteConnector(part, item, true);
                        }
                        else if (!item.IsPublished()
                            // Only publish if RHS already published
                            && item.RightContent.ContentItem.IsPublished()) {

                            // Inverse will get published automatically by the handler
                            Services.ContentManager.Publish(item.ContentItem);
                            // TODO: Some hook in case the right-item needs updating at this point?
                        }
                    }
                }
            }
        }
        private void UnpublishSocket(SocketsPart part) {
            // Trigger item
            _signals.Trigger(new ContentItemSignal(part.Id));
            // Signal anything connected
            Trigger(part);
            // And remove all connectors
            var list =
                // Rights
                _mechanics.Value.Connectors(part, ConnectorCriteria.Published).List()
                // Lefts
                .Union(_mechanics.Value.LeftConnectors(part, ConnectorCriteria.Published).List());
            foreach (var connector in list) {
                Services.ContentManager.Unpublish(connector.ContentItem);
                if (connector.InverseConnector != null) {
                    Services.ContentManager.Unpublish(connector.ContentItem);
                }
            }
        }
        private void RemoveSocket(SocketsPart part)
        {
            // Trigger item
            _signals.Trigger(new ContentItemSignal(part.Id));
            // Signal anything connected
            Trigger(part);
            // And remove all connectors
            // This time we need to remove the current version
            // TODO: Still need to double check this is doing the right thing. Need more control over acquiring draft/published/latest on connectors.
            var list =
                // Rights
                AllConnectors(part)
                // Lefts
                .Union(_mechanics.Value.LeftConnectors(part).List());
            foreach (var connector in list)
            {
                // TODO: If someone has permission to delete an item, they should have permission to remove connectors;
                // but we need to handle versioning in this scenario, and actually also prevent items being deleted
                // if the user doesn't have permission to
                // TODO: Additionally there sometimes will need to be some further recursion to e.g. remove comments. This needs to be an
                // additional connector setting (cascade delete).
                _mechanics.Value.DeleteConnector(part, connector, true, true, true);
            }
        }

        private void Trigger(IContent content)
        {
            _signals.Trigger("Mechanics_Cache_AllContent");
        }
        #endregion
        #region Connector handlers

        protected void ConnectorPropertyGetHandlers(ConnectorPart part, Func<SocketsPart> knownLeft = null, Func<SocketsPart> knownRight = null, Func<ConnectorPart> knownInverse = null) {
            // Add handlers that will load content for id's just-in-time (although sometimes we'll have special rules e.g. drafts)
            part.LeftContentField.Loader(knownLeft ?? (() => LoadSocketContent(part,part.Record.LeftContentItem_id, part.Record.LeftContentVersionId)));
            part.RightContentField.Loader(knownRight ?? (() => LoadSocketContent(part, part.Record.RightContentItem_id, part.Record.RightContentVersionId)));
            part.InverseConnectorField.Loader(knownInverse ?? (() => LoadConnectorContent(part, part.Record.InverseConnector_id, part.Record.InverseConnectorVersionId)));
        }

        private SocketsPart LoadSocketContent(ConnectorPart part, int leftId, int? leftVersionId) {
            if (leftId != 0) {
                if (part.ContentItem.VersionRecord != null && part.ContentItem.VersionRecord.Latest && !part.ContentItem.VersionRecord.Published) {
                    return Services.ContentManager.Get<SocketsPart>(leftId, VersionOptions.Latest);
                }
                return Services.ContentManager.Get<SocketsPart>(leftId, VersionOptions.Published);
            }
            return null;
        }

        private ConnectorPart LoadConnectorContent(ConnectorPart part, int? leftId, int? leftVersionId) {
            if (leftId.HasValue) {
                if (part.ContentItem.VersionRecord != null && part.ContentItem.VersionRecord.Latest && !part.ContentItem.VersionRecord.Published) {
                    return Services.ContentManager.Get<ConnectorPart>(leftId.Value, VersionOptions.Latest);
                }
                return Services.ContentManager.Get<ConnectorPart>(leftId.Value, VersionOptions.Published);
            }
            return null;
        }

        protected static void ConnectorPropertySetHandlers(ActivatedContentContext context, ConnectorPart part) {
            // add handlers that will update records when part properties are set
            part.LeftContentField.Setter(sock => {
                part.Record.LeftContentItem_id = sock == null
                    // TODO: Throw exception for null socket?
                    ? 0
                    : sock.ContentItem.Id;
                part.Record.LeftContentVersionId = sock == null
                    ? 0
                    : (sock.ContentItem.VersionRecord == null ? (int?)null : sock.ContentItem.VersionRecord.Id);
                return sock;
            });
            part.RightContentField.Setter(sock => {
                part.Record.RightContentItem_id = sock == null
                    // TODO: Throw exception for null socket?
                    ? 0
                    : sock.ContentItem.Id;
                part.Record.RightContentVersionId = sock == null
                    ? 0
                    : (sock.ContentItem.VersionRecord == null ? (int?)null : sock.ContentItem.VersionRecord.Id);
                return sock;
            });
            part.InverseConnectorField.Setter(conn => {
                part.Record.InverseConnector_id = conn == null
                    // TODO: Throw exception for null socket?
                    ? 0
                    : conn.ContentItem.Id;
                part.Record.InverseConnectorVersionId = conn == null
                    ? 0
                    : (conn.ContentItem.VersionRecord == null ? (int?)null : conn.ContentItem.VersionRecord.Id);
                return conn;
            });
        }

        #endregion
    }
}
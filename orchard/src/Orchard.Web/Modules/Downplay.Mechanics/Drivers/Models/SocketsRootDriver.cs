using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Models;
using Downplay.Origami.Services;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Framework;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Notify;
using Orchard.Logging;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using Orchard.Caching;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// For every socket (i.e. allowed content type) applies a SocketEventContext model over the root shape, which should be the Content
    /// shape the SocketsPart is rendering in...
    /// </summary>
    public class SocketsRootDriver : ModelDriver<SocketsModel>
    {
        private readonly Lazy<IMechanicsService> _mechanics;
        private readonly Lazy<IOrigamiService> _origami;
        private readonly Lazy<IEnumerable<ISocketHandler>> _socketHandlers;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        public SocketsRootDriver(
            Lazy<IMechanicsService> mechanics,
            Lazy<IOrigamiService> origami,
            Lazy<IEnumerable<ISocketHandler>> socketHandlers,
            IOrchardServices services,
            ICacheManager cacheManager,
            ISignals signals
            )
        {
            _origami = origami;
            _mechanics = mechanics;
            _socketHandlers = socketHandlers;
            _cacheManager = cacheManager;
            _signals = signals;
            Services = services;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        protected override string Prefix
        {
            get { return "Sockets"; }
        }

        protected override void Update(SocketsModel model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
            // Handled in Build
        }
 
        protected override ModelDriverResult Build(SocketsModel model, dynamic shapeHelper, ModelShapeContext context) {

            // HACK: Check for pre-populated sockets
            // We need a better way to get day into here, we might want to populate other stuff, will need a custom controller
            string prepopulateSocket = null;
            int? prepopulateId = null;
            if (Services.WorkContext.HttpContext != null) {
                if (Services.WorkContext.HttpContext.Request.QueryString["socket_populate_name"] != null) {
                    prepopulateSocket = Services.WorkContext.HttpContext.Request.QueryString["socket_populate_name"];
                    prepopulateId = Services.WorkContext.HttpContext.Request.QueryString["socket_populate_id"].ParseInt();
                }
            }

            // Get all types of Connector
            var result = new List<ModelDriverResult>();

            foreach (var t in model.LeftContent.As<SocketsPart>().Sockets.Allowed)
            {
                // TODO: Even this constructor performs some non-trivial work; since it could run a lot of times for a detailed request
                // we should simplify it a lot
                var socketContext = new SocketDisplayContext(model.LeftContent, t, context.DisplayType, model) { ModelContext = context };
                if (!String.IsNullOrWhiteSpace(socketContext.Connector.Settings.DefaultParadigms)) {
                    socketContext.Paradigms.Add(socketContext.Connector.Settings.DefaultParadigms.Split(new[]{',',' '},StringSplitOptions.RemoveEmptyEntries));
                }
                else {
                    socketContext.Paradigms.Add(context.Paradigms.All());
                }
                socketContext.Paradigms.Add("Socket");

                _socketHandlers.Value.Invoke(s => s.Preparing(socketContext), Logger);
                if (!socketContext.RenderSocket) continue;

                // HACK: The way this delegate works is pretty awful and needs revision. It can also get called multiple times (e.g. rendering two different types of socket contents)
                // which is why I'm clearing the filters. Really it needs to work completely differently so we can sort and filter directly on the SocketQuery.
                socketContext.Filtering = () => {
                    socketContext.SocketFilters.Clear();
                    socketContext.SocketSorters.Clear();
                    _socketHandlers.Value.Invoke(s => s.Filtering(socketContext), Logger);
                };

                // From this point on, all the handling needs to happen in a factory so it'll only get executed if placement says so
                // Otherwise, we start running into exponential performance problems as we get more and more connections happening.

                // Place in layout?
                // TODO: LayoutPlacement is completely redundant now we have ZoneProxy and Alchemy methods. Paperclip could still be used for the placement-per-item scenario.
                // TODO: Cache therefore needs fixing to work on normal sockets.
                if (!String.IsNullOrWhiteSpace(socketContext.LayoutPlacement))
                {
                    dynamic display;
                    if (socketContext.CacheSocket) {
                        var key = new SocketCacheKey(model.LeftContent.Id, t.Name, context.DisplayType, Services.WorkContext.HttpContext.Request.Path);
                        var cacheResult = _cacheManager.Get<SocketCacheKey, SocketCacheResult>(key, ctx => {
                            // Set up cache terms
                            ctx.Monitor(_signals.When(new ContentItemSignal(key.Id)));
                            ctx.Monitor(_signals.When("Mechanics_Cache_AllContent"));

                            // Build shape
                            var socket = BuildDisplayDelegate(socketContext, context);

                            // Sockets can still not render at this point and we want to avoid an error in core shapes
                            return new SocketCacheResult() {
                                Display = socket
                            };
                        });
                        display = shapeHelper.SocketCache(Cache: cacheResult);
                    }
                    else {
                        display = BuildDisplayDelegate(socketContext, context);
                    }
                    // Figure out exact placement
                    var placementBits = socketContext.LayoutPlacement.Split(':');
                    if (placementBits.Length > 1)
                    {
                        // Zone with position
                        Services.WorkContext.Layout.Zones[placementBits[0]].Add(display, placementBits[1]);
                    }
                    else
                    {
                        // Zone without position
                        Services.WorkContext.Layout.Zones[placementBits[0]].Add(display);
                    }
                }
                else
                {
                    var socketResult = new SocketsDriverResult("Socket", "" /*FullPrefix(context)*/, socketContext, BuildDisplayDelegate).Differentiator(t.Name);
                    // TODO: Hack for Site Settings to show sockets on group pages. We can do more with groups than this; support group-by-placement and have tabbed group zones
                    if (model.LeftContent.ContentItem.ContentType == "Site") {
                        // TODO: Allow this (and other settings) to be mutated during the handler pipeline
                        var groupId = socketContext.Connector.Settings.SocketGroupName;
                        socketResult.OnGroup(groupId);
                    }
                    result.Add(socketResult);
                }
            }

            return Combined(result.ToArray());
        }

        private dynamic BuildDisplayDelegate(SocketDisplayContext socketContext, ModelShapeContext context)
        {
            var prefix = FullPrefix(context, socketContext.Connector.Definition.Name);

            // Set up display text
            socketContext.SocketMetadata.SocketTitle =
                String.IsNullOrWhiteSpace(socketContext.Connector.Settings.SocketDisplayName)
                ? (socketContext.Connector.Definition.DisplayName + (socketContext.Connector.Settings.AllowMany ? "s" : ""))
                : socketContext.Connector.Settings.SocketDisplayName;
            socketContext.SocketMetadata.DisplayHint = socketContext.Connector.Settings.SocketDisplayHint;
            socketContext.SocketMetadata.EditorHint = socketContext.Connector.Settings.SocketEditorHint;

            // Content querying and filtration process
            // TODO: Fully review and profile all this (and places where the lazies are accessed)
            socketContext.Query = socketContext.Left.ContentPart.Sockets.Socket(socketContext.Name);
            
            // Build a socket shape
            dynamic socket;
            if (context.Mode == "Editor") {
                _socketHandlers.Value.Invoke(s => s.Editing(socketContext), Logger);
                if (!socketContext.RenderSocket) return null;
                socket = context.New.Socket_Edit(Prefix: prefix, ConnectorType: socketContext.Connector.Definition.Name, ContentItem: socketContext.Left.ContentItem);
            }
            else {
                _socketHandlers.Value.Invoke(s => s.Displaying(socketContext), Logger);
                if (!socketContext.RenderSocket) return null;
                socket = context.New.Socket()
                .ConnectorType(socketContext.Connector.Name)
                .ContentItem(socketContext.Left.ContentItem);
            };

            string displayType = socketContext.Left.DisplayType;
            if (!String.IsNullOrWhiteSpace(socketContext.Connector.DisplayType)) {
                displayType = socketContext.Connector.DisplayType;
            }

            socket.Metadata.DisplayType = displayType;

            if (context.Mode == "Editor") {
                // Edit/update handlers
                if (context.Updater == null) {
                    _socketHandlers.Value.Invoke(s => s.Editor(socketContext), Logger);
                }
                else {
                    _socketHandlers.Value.Invoke(s => s.Updating(socketContext), Logger);
                }
                if (!socketContext.RenderSocket) return null;

                // Build the editor shape
                var builder1 = _origami.Value.BuildEditorShape(socketContext, context.Updater, prefix, displayType, "Socket", socketContext.Left.ContentType, context)
                    .WithParadigms(socketContext.Paradigms);
                _origami.Value.Build(builder1, socket);

                if (context.Updater != null) {
                    _socketHandlers.Value.Invoke(s => s.Updated(socketContext), Logger);
                }
            }
            else {
                // Build the display shape
                var builder = _origami.Value.BuildDisplayShape(socketContext, prefix, displayType, "Socket", socketContext.Left.ContentType, context)
                    .WithParadigms(socketContext.Paradigms);
                _origami.Value.Build(builder, socket);
            }
            return socket;
        }

   }
}
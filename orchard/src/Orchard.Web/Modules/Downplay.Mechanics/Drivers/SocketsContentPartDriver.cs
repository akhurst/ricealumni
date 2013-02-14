using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard;
using Downplay.Mechanics.Services;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement.MetaData;
using Orchard.Environment;
using Downplay.Origami.Drivers;

namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// We're implementing the entire IContentPartDriver interface because the generic ContentPartDriver doesn't give us everything we need from the context.
    /// </summary>
    public class SocketsContentPartDriver : IContentPartDriver
    {
        private readonly Work<IMechanicsDisplay> _mechanicsDisplay;

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        public SocketsContentPartDriver(IOrchardServices services, 
            Work<IMechanicsDisplay> display)
        {
            Services = services;
            _mechanicsDisplay = display;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public DriverResult BuildDisplay(Orchard.ContentManagement.Handlers.BuildDisplayContext context)
        {
            var sockets = context.ContentItem.As<SocketsPart>();
            if (sockets != null) {
                _mechanicsDisplay.Value.ApplyDisplay(context);
            }
            // Shapes are now being applied directly in Origami (since this is all that normally happens on return from driver anyway)
            return new DriverResult();
        }

        public DriverResult BuildEditor(Orchard.ContentManagement.Handlers.BuildEditorContext context)
        {
            var sockets = context.ContentItem.As<SocketsPart>();
            if (sockets != null)
            {
                var origamiContext = context as BuildContentEditorContext;
                if (origamiContext != null) {
                    SocketParentContext parentContext = null;
                    if (origamiContext.ParentContext != null) {
                        origamiContext.ParentContext.With<SocketParentContext>(s => {
                            parentContext = s;
                        });
                    }
                    _mechanicsDisplay.Value.ApplyEditors(context, parentContext);
                }
            }
            // Shapes are now being applied directly in Origami (since this is all that normally happens on return from driver anyway)
            return new DriverResult();
        }

        public DriverResult UpdateEditor(Orchard.ContentManagement.Handlers.UpdateEditorContext context)
        {
            var sockets = context.ContentItem.As<SocketsPart>();
            if (sockets != null)
            {
                var origamiContext = context as UpdateContentEditorContext;
                if (origamiContext != null) {
                    SocketParentContext parentContext = null;
                    if (origamiContext.ParentContext != null) {
                        origamiContext.ParentContext.With<SocketParentContext>(s => {
                            parentContext = s;
                        });
                    }
                    _mechanicsDisplay.Value.ApplyEditors(context, parentContext);
                }
            }
            // Shapes are now being applied directly in Origami (since this is all that normally happens on return from driver anyway)
            return new DriverResult();
        }

        public void Importing(Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
            // TODO: Possibly don't need anything for import/export ... But we could take the opportunity to hook up to connectors based on Identity?
        }

        public void Imported(Orchard.ContentManagement.Handlers.ImportContentContext context)
        {
        }

        public void Exporting(Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
        }

        public void Exported(Orchard.ContentManagement.Handlers.ExportContentContext context)
        {
        }

        public IEnumerable<Orchard.ContentManagement.MetaData.ContentPartInfo> GetPartInfo()
        {

            var contentPartInfo = new[] {
                new ContentPartInfo {
                    PartName = typeof (SocketsPart).Name,
                    Factory = typePartDefinition => new SocketsPart {TypePartDefinition = typePartDefinition}
                }
            };

            return contentPartInfo;
        }

        public void GetContentItemMetadata(Orchard.ContentManagement.Handlers.GetContentItemMetadataContext context)
        {
            var part = context.ContentItem.As<SocketsPart>();
            if (part != null)
                GetContentItemMetadata(part, context.Metadata);
        }
        protected virtual void GetContentItemMetadata(SocketsPart part, ContentItemMetadata metadata) { return; }
    }
}
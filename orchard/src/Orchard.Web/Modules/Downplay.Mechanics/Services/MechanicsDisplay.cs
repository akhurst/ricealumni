using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment;
using Downplay.Mechanics.Framework;
using Orchard;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.DisplayManagement;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Origami.Services;
using Orchard.Localization;
using Orchard.UI.Notify;
using Downplay.Mechanics.ViewModels;
using Downplay.Mechanics.Settings;
using Orchard.Logging;
using Orchard.ContentManagement.Handlers;
using Downplay.Origami.Drivers;

namespace Downplay.Mechanics.Services
{
    public class MechanicsDisplay : IMechanicsDisplay
    {
        private readonly IOrigamiService _origami;
        private readonly IMechanicsService _mechanics;
        private readonly IShapeFactory _shapeFactory;

        public MechanicsDisplay(
            IOrigamiService origami,
            IShapeFactory shapeFactory,
            IMechanicsService mechanics
            )
        {
            _origami = origami;
            Shape = _shapeFactory = shapeFactory;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            _mechanics = mechanics;
        }

        public dynamic Shape { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        private bool CheckRecursion(IContent content, SocketParentContext parentContext, string displayType)
        {
            // Check the same item hasn't been rendered with the same display type further up the tree
            if (parentContext.Sockets.LeftContent.Id == content.Id && parentContext.Sockets.DisplayType==displayType)
            {
                return true;
            }
            // Move up chain
            if (parentContext.Parent != null)
            {
                return CheckRecursion(content, parentContext.Parent,displayType);
            }
            return false;
        }

        #region Display methods

        // TODO: Slight changes made to ApplyDisplay need applying to Editor/Update also
        public void ApplyDisplay(BuildDisplayContext driverContext) {
            var rootShape = driverContext.Shape;
            var content = driverContext.Content;
            var displayType = driverContext.DisplayType;
            var groupId = driverContext.GroupId;
            SocketParentContext parentContext = null;

            var origamiContext = driverContext as BuildContentDisplayContext;
            if (origamiContext != null) {
                if (origamiContext.ParentContext != null) {
                    origamiContext.ParentContext.With<SocketParentContext>(s => {
                        parentContext = s;
                    });
                }
            }

            // Discover if this item has been displayed further up the chain. This prevents recursion.
            if (parentContext!=null && CheckRecursion(content,parentContext,displayType)) {
                var builder1 = _origami.BuildDisplayShape(new RecursionPreventedModel(content), "", displayType, "Sockets", content.ContentItem.ContentType, parentContext.ModelContext);
                _origami.Build(builder1, rootShape);
                return;
            }

            var prefix = "Sockets";
            if (parentContext != null && !String.IsNullOrWhiteSpace(parentContext.Prefix))
            {
                prefix = parentContext.Prefix + "." + prefix;
            }

            // Build Sockets model
            var model = new SocketsModel(content, displayType, parentContext);

            var builder = _origami.Builder(model)
                .WithMode("Display")
                .WithUpdater(null, prefix)
                .WithDisplayType(displayType)
                .WithStereotype("Sockets")
                .WithContentType(content.ContentItem.ContentType)
                    .WithGroup(groupId);
            if (origamiContext != null) {
                builder.WithParent(origamiContext.ParentContext);
                builder.WithParadigms(origamiContext.ParentContext.Paradigms);
            }

            _origami.Build(builder, rootShape);
        }

#endregion

        #region Edit methods
        public void ApplyEditors(BuildEditorContext driverContext, SocketParentContext parentContext = null) {
            var rootShape = driverContext.Shape;
            var content = driverContext.Content;
            var displayType = "";
            var groupId = driverContext.GroupId;
            var updateContext = driverContext as UpdateEditorContext;
            var updater = updateContext == null ? null : updateContext.Updater;
            
            // Discover if this item has been displayed further up the chain. This prevents recursion.
            if (parentContext != null && CheckRecursion(content, parentContext,displayType))
            {
                _origami.Build(
                    _origami.Builder(new RecursionPreventedModel(content))
                    .WithMode("Display")
                    .WithDisplayType(displayType)
                    .WithStereotype("Sockets")
                    .WithContentType(content.ContentItem.ContentType)
                    .WithParent(parentContext.ModelContext)
                    .WithParadigms(parentContext.ModelContext.Paradigms)
                    , rootShape);
                return;
            }

            var context = new SocketsModel(content, displayType, parentContext);

            var prefix = "Sockets";
            if (parentContext != null && !String.IsNullOrWhiteSpace(parentContext.Prefix))
            {
                prefix = parentContext.Prefix + "." + prefix;
            }
//            ApplyParadigms(rootShape, context);

  //          var paradigms = context..DefaultParadigms.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var builder = _origami.Builder(context)
                .WithMode("Editor")
                .WithUpdater(updater, prefix)
                .WithDisplayType(displayType)
                .WithStereotype("Sockets")
                .WithContentType(content.ContentItem.ContentType)
                .WithGroup(groupId);

            if (parentContext!=null) {
                builder.WithParent(parentContext.ModelContext)
                    .WithParadigms(parentContext.ModelContext.Paradigms);
            }
            _origami.Build(builder,rootShape);
        }

        #endregion
    }
}
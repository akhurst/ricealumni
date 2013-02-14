using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Origami.Services {
    /// <summary>
    /// Replace default content display with an Origami-friendly version (so we get all the new Placement goodness)
    /// </summary>
    [OrchardSuppressDependency("Orchard.ContentManagement.DefaultContentDisplay")]
    public class OrigamiContentDisplay : IContentDisplay {

        private readonly IOrigamiService _origami;

        public OrigamiContentDisplay(
            IOrigamiService origami
            ) {
                _origami = origami;
        }

        public dynamic BuildDisplay(IContent content, string displayType = "", string groupId = "") {
            var shape = _origami.ContentShape(content, displayType);
            _origami.Build(_origami.ContentBuilder(content).WithDisplayType(String.IsNullOrWhiteSpace(displayType)?"Detail":displayType).WithGroup(groupId), shape);
            return shape;
        }

        public dynamic BuildEditor(IContent content, string groupId = "") {
            var shape = _origami.ContentShape(content, "", true);
            var builder = _origami.ContentBuilder(content).WithMode("Editor");
            if (!String.IsNullOrWhiteSpace(groupId)) builder.WithGroup(groupId);
            _origami.Build(builder, shape);
            return shape;
        }

        public dynamic UpdateEditor(IContent content, IUpdateModel updater, string groupId = "") {
            var shape = _origami.ContentShape(content, "", true);
            var builder = _origami.ContentBuilder(content).WithMode("Editor").WithUpdater(updater, "");
            if (!String.IsNullOrWhiteSpace(groupId)) builder.WithGroup(groupId);
            _origami.Build(builder, shape);
            return shape;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Downplay.Origami.Services;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;

namespace Downplay.Origami.Drivers {
    public class UpdateContentEditorContext : UpdateEditorContext {

        public ModelShapeContext ParentContext { get; set; }
        public String DisplayType { get; set; }
        public UpdateContentEditorContext(IShape model, IContent content, string displayType, IUpdateModel updater, string groupId, IShapeFactory shapeFactory, ShapeTable shapeTable, ModelShapeContext parentContext = null)
            : base(model, content, updater, groupId, shapeFactory, shapeTable)
        {
            DisplayType = displayType;
            ParentContext = parentContext;
        }
    }
}
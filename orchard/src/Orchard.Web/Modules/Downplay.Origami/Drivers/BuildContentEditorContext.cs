using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Downplay.Origami.Services;

namespace Downplay.Origami.Drivers
{
    public class BuildContentEditorContext : BuildEditorContext
    {
        public ModelShapeContext ParentContext { get; set; }
        public String DisplayType { get; set; }
        public BuildContentEditorContext(IShape model, IContent content, string displayType, string groupId, IShapeFactory shapeFactory, ModelShapeContext parentContext = null)
            : base(model, content, groupId, shapeFactory)
        {
            DisplayType = displayType;
            ParentContext = parentContext;
        }

    }
}

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
    public class BuildContentDisplayContext : BuildDisplayContext
    {
        public ModelShapeContext ParentContext { get; set; }

        public BuildContentDisplayContext(IShape model, IContent content, string displayType, string groupId, IShapeFactory shapeFactory, ModelShapeContext parentContext = null)
            : base(model, content, displayType, groupId, shapeFactory)
        {
            ParentContext = parentContext;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;
using Orchard.ContentManagement;
using Downplay.Alchemy.Events;

namespace Downplay.Alchemy.Factories {
    public class ContentModelBuilder : IContextTag<Func<IContent,ModelShapeBuilder>> {
    }
}

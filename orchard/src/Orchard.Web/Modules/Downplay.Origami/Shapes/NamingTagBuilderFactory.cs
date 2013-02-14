using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Environment.Extensions;
using Orchard.DisplayManagement.Shapes;

namespace Downplay.Origami.Shapes {
    [OrchardSuppressDependency("Orchard.DisplayManagement.Shapes.TagBuilderFactory")]
    public class NamingTagBuilderFactory : ITagBuilderFactory {
        public OrchardTagBuilder Create(dynamic shape, string tagName) {
            // Identical to supressed version except following line looks for a tag name in the shape:
            var tagBuilder = new OrchardTagBuilder(shape.TagName==null ? tagName : (string)shape.TagName);
            tagBuilder.MergeAttributes(shape.Attributes, false);
            foreach (var cssClass in shape.Classes ?? Enumerable.Empty<string>())
                tagBuilder.AddCssClass(cssClass);
            if (!string.IsNullOrEmpty(shape.Id))
                tagBuilder.GenerateId(shape.Id);
            return tagBuilder;
        }
    }
}
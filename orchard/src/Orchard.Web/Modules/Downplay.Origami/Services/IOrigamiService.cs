using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;

namespace Downplay.Origami.Services
{
    public interface IOrigamiService : IDependency
    {
        ModelShapeBuilder Builder(object model);
        ModelShapeBuilder ContentBuilder(IContent content);
        void Build(ModelShapeBuilder builder, dynamic rootShape);
        /// <summary>
        /// Creates a default "{Stereotype}" shape to render an item in
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        dynamic ContentShape(IContent content,string displayType, bool editor=false, string prefix=null);
        /// <summary>
        /// Create a Content shape *without* an actual content item...
        /// </summary>
        /// <param name="stereotype"></param>
        /// <param name="displayType"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        dynamic ContentShape(String stereotype, string displayType, bool editor = false, string prefix=null);
    }
}

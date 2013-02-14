using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement.Drivers;

namespace Downplay.Origami.Services
{
    public interface IModelDriver : IDependency
    {
        /*
        ModelDriverResult BuildDisplay(ModelShapeContext context);
        ModelDriverResult BuildEditor(ModelShapeContext context);
        ModelDriverResult UpdateEditor(ModelShapeContext context);
        */

        ModelDriverResult Run(ModelShapeContext context);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;

namespace Downplay.Origami.Placement
{
    public static class PlacementExtensions
    {

        public static T Model<T> (this ShapePlacementContext ctx)
            where T:class
        {
            var ctx2 = ctx as ModelShapePlacementContext;
            if (ctx2 == null) return default(T);

            return ctx2.ModelContext.Model as T;
        }

    }
}
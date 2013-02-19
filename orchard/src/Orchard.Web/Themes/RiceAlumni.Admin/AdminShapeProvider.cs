using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;

namespace RiceAlumni.Admin
{
    public class AdminShapeProvider : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Parts_Containable_Edit").OnDisplaying(displaying=>
                {
                    ContentItem item = displaying.Shape.ContentItem;
                    if (item.ContentType == "StaffGroup")
                        displaying.ShapeMetadata.Alternates.Add("Containable_StaffGroup");
                });
        }
    }
}
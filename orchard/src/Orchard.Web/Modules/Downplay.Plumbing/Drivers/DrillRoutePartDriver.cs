using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement;
using Orchard;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment.Extensions;
using Downplay.Plumbing.Models;

namespace Downplay.Plumbing.Drivers
{
    /// <summary>
    /// Driver used for displaying and editing an individual Connector node
    /// </summary>
    public class DrillRoutePartDriver : ContentPartDriver<DrillRoutePart>
    {
        public IOrchardServices Services { get; set; }

        public DrillRoutePartDriver(IOrchardServices orchardServices)
        {
            Services = orchardServices;
        }

        protected override string Prefix
        {
            get
            {
                return "DrillRoute";
            }
        }

        protected override DriverResult Display(DrillRoutePart part, string displayType, dynamic shapeHelper)
        {
            return new DriverResult();
        }

        protected override DriverResult Editor(DrillRoutePart part, dynamic shapeHelper)
        {
            return Editor(part, (IUpdateModel)null, shapeHelper);
        }

        protected override DriverResult Editor(DrillRoutePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return new DriverResult(); 
        }

    }
}
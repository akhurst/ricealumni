using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;

namespace Downplay.Plumbing.Settings
{
    public class DrillRouteTypePartSettings
    {
        public DrillRouteTypePartSettings()
        {
            DrilledDisplayType = "Detail";
        }

        /// <summary>
        /// TODO: We don't need this anymore either, right?
        /// </summary>
        public string DrilledDisplayType { get; set; }

    }
}
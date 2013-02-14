using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;

namespace Downplay.Theory.Cartography.Aggregation
{
    [OrchardFeature("Downplay.Theory.Cartography.Aggregation")]
    public class AggregationTypePartSettings
    {

        public AggregationTypePartSettings()
        {
            ExposeFeed = false;
        }

        /// <summary>
        /// Whether to expose a feed on this content type
        /// </summary>
        public bool ExposeFeed { get; set; }

    }
}
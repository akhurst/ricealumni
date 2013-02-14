using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.UI.Resources;

namespace Downplay.Delta.Services {
    public class DeltaInstanceRequirement {
        public string ResourceType { get; set; }

        public string ResourceName { get; set; }

        public Action<RequireSettings> Delegate { get; set; }
    }
}

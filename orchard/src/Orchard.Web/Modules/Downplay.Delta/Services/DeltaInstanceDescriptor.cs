using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Dynamic;
using Orchard.UI.Resources;

namespace Downplay.Delta.Services {
    public class DeltaInstanceDescriptor {

        public DeltaInstanceDescriptor() {
            Properties = new Stuff();
            Requires = new List<DeltaInstanceRequirement>();
        }

        public ICollection<DeltaInstanceRequirement> Requires { get; set; }

        public string Namespace { get; set; }
        public string TypeName { get; set; }
        public dynamic Properties { get; set; }


        public DeltaInstanceDescriptor Require(string scriptName) {
            return Require("script", scriptName);
        }

        public DeltaInstanceDescriptor Require(string resourceType, string resourceName, Action<RequireSettings> modify = null) {
            var d = new DeltaInstanceRequirement() {
                ResourceType = resourceType,
                ResourceName = resourceName,
                Delegate = modify
            };
            Requires.Add(d);
            return this;
        }
    }
}

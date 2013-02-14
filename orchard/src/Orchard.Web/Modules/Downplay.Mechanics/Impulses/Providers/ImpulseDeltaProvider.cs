using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Delta.Services;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Impulses.Providers {
    /// <summary>
    /// TODO: Impulses is looking more and more like a Theory/Prototypes project; it has nothing to do with Mechanics and a lot of dependencies,
    /// mechanics otherwise doesn't need Delta (although there are things we could do with that ...)
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseDeltaProvider : IDeltaInstanceProvider {
        public void Configure(DeltaInstanceConfiguration context) {
            // TODO: What would be neat is a way to switch delta features on or off dynamically without having to enable/disable features.
            // This can already happen to a certain extent due to permissions.
            context.Instances.Add(new DeltaInstanceDescriptor() {
                Namespace = "Science.Impulses",
                TypeName = "ImpulseHijacker"
            }.Require("Science_Impulses"));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Localization;

namespace Downplay.Mechanics.Impulses.Services {
    public class ImpulseDescribeContext {

        public Dictionary<String, ImpulseDescriptor> Impulses = new Dictionary<string, ImpulseDescriptor>();

        public ImpulseDescriptor Impulse(string name, LocalizedString caption=null, LocalizedString description=null) {
            if (!Impulses.ContainsKey(name)) {
                Impulses[name] = new ImpulseDescriptor(name);
            }
            var impulse = Impulses[name];
            if (caption != null) impulse.Caption = caption;
            if (description != null) impulse.Description = description;
            return impulse;
        }
    }
}

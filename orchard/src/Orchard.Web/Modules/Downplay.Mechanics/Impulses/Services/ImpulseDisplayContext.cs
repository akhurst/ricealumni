using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace Downplay.Mechanics.Impulses.Services
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseDisplayContext : IImpulse
    {
        private ImpulseDescriptor _impulseDescriptor;
        public ImpulseDisplayContext(ImpulseDescriptor impulse)
        {
            _impulseDescriptor = impulse;
            Impulses = new List<IImpulse>();
        }

        public IContent Content { get; set; }
        public List<IImpulse> Impulses { get; set; }

        public string DisplayType { get; set; }

        public dynamic Data { get; set; }

        public System.Web.Routing.RouteValueDictionary HrefRoute { get; set; }

        public string Name {
            get { return _impulseDescriptor.Name; }
        }

        public LocalizedString Caption {
            get { return _impulseDescriptor.Caption; }
        }
        public LocalizedString Description {
            get { return _impulseDescriptor.Description; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Models
{
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class EffectiveRolesPartRecord : ContentPartRecord
    {

        public virtual String EffectiveRoles { get; set; }

    }
}

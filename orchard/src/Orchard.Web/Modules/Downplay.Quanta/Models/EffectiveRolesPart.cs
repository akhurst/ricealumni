using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Models
{
    /// <summary>
    /// Part allows permissions to be assigned on a per-content basis.
    /// This part isn't usable, 
    /// </summary>
    [OrchardFeature("Downplay.Quanta.Effectors")]
    public class EffectiveRolesPart : ContentPart<EffectiveRolesPartRecord>
    {

        public IEnumerable<String> ListEffectiveRoles()
        {
            if (String.IsNullOrWhiteSpace(EffectiveRoles)) return new string[]{};
            return EffectiveRoles.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string EffectiveRoles
        {
            get { return Record.EffectiveRoles; }
            set { Record.EffectiveRoles = value; }
        }

    }
}
using Orchard;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;

namespace Downplay.Mechanics.Models {
	[OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipPartRecord : ContentPartRecord {
        public virtual String Placement{ get; set; }
        public virtual String DisplayType{ get; set; }

    }
}
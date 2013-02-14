using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;

namespace Downplay.Mechanics.Models {
	[OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipPart : ContentPart<PaperclipPartRecord> {
        public String Placement {
            get { return Record.Placement; }
            set { Record.Placement = value; }
        }
        public String DisplayType {
            get { return Record.DisplayType; }
            set { Record.DisplayType = value; }
        }

    }
}

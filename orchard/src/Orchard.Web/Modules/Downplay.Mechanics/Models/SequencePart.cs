using Orchard;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Models {
	
    public class SequencePart : ContentPart<SequencePartRecord> {
        public int Sequence {
            get { return Record.Sequence; }
            set { Record.Sequence = value; }
        }

    }
}

using Orchard;
using Orchard.ContentManagement.Records;

namespace Downplay.Mechanics.Models {
	
    public class SequencePartRecord : ContentPartRecord {
        public virtual int Sequence{ get; set; }

    }
}
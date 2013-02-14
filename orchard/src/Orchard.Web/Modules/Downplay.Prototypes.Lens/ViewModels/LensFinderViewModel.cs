using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Prototypes.Lens.ViewModels {

    public class LensFinderViewModel {
        public LensFinderViewModel() {
            ContentTypes = Enumerable.Empty<String>();
        }
        public string Query { get; set; }
        public IEnumerable<String> ContentTypes { get; set; }

        public string SelectedItemIds { get; set; }

        public IEnumerable<dynamic> Results { get; set; }
        public string SocketName { get; set; }
        public int LeftId { get; set; }
    }

}

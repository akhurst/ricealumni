using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Drivers.Models {
    public class SocketRightContentViewModel {
        public Lazy<IEnumerable<IContent>> RightContent { get; set; }
    }
}

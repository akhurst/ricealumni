using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace Downplay.Mechanics.ViewModels {
    public class SocketConnectorsViewModel {
        public Lazy<IEnumerable<IContent>> Connectors { get; set; }
    }
}
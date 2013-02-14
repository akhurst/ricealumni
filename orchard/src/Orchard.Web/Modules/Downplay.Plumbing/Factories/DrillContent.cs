using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Models;
using Downplay.Alchemy.Factories;

namespace Downplay.Plumbing.Factories {
    public class DrillContent : IContextTag {

        public SocketsPart Left { get; set; }
        public SocketsPart Right { get; set; }
        public ConnectorPart Connector { get; set; }

    }
}

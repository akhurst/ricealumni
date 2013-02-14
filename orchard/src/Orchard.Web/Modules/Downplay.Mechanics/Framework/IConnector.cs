using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Framework {
    public interface IConnector : IContent {

        int LeftContentItemId { get; }
        int? LeftContentVersionId { get; set; }
        int RightContentItemId { get; }
        int? RightContentVersionId { get; set; }

        SocketsPart RightContent { get; set; }
        IConnector InverseConnector { get; set; }

    }
}

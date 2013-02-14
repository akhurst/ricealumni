using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Mechanics.Services {
    public enum ConnectorCriteria {

        /// <summary>
        /// Will choose drafts or published based on state of left item
        /// </summary>
        Auto,

        Published,

        Drafts,

        DraftsAndDeleted

    }
}

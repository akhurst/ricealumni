using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;

namespace Downplay.Theory.Cartography.Handlers {
    public class MenuSocketHandler : SocketHandler {

        protected override void Preparing(SocketDisplayContext context) {
            if (context.ModelContext.Mode == "Display" && context.Paradigms.Has("Navigation") && context.ModelContext.DisplayType=="Navigation") {
                context.Connector.DisplayType = "Navigation";
            }
        }

        protected override void Displaying(SocketDisplayContext context) {
         
        }

    }
}
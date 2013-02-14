using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;

namespace Downplay.Theory.Identity.Events
{
    public class IdentitySocketEvents : SocketHandler
    {

        public IdentitySocketEvents() { } 

        protected override void Displaying(SocketDisplayContext context)
        {
            if (context.RootModel.DisplayType == "Detail")
            {
                switch (context.Connector.Name)
                {
                    case "CountryToTown":
                        context.Left.DisplayType = context.Connector.DisplayType = "Directory";
                        // TODO: In migration add to settings
                        context.Paradigms.Add("Count");
                        break;
                    case "TownToAddress":
                        context.Left.DisplayType = context.Connector.DisplayType = "Listing";
                        // TODO: In migration add to settings
                        context.Paradigms.Add("Collapse");
                        break;
                }
            }

            // From DirectoryController, we get a bunch of Address items in Summary mode
            if (context.RootModel.DisplayType == "Summary")
            {
                switch (context.Connector.Name)
                {
                    case "AddressToUser":
                        context.Left.DisplayType = context.Connector.DisplayType = "Listing";
                        break;
                    case "AddressToTown":
                        context.Left.DisplayType = context.Connector.DisplayType = "Listing";
                        break;
                }
            }

            if (context.RootModel.DisplayType == "Listing")
            {
                // ...
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Mechanics.Drivers
{
    public struct SocketCacheKey
    {
        public int Id;
        public String SocketType;
        public string DisplayType;
        public string Url;

        public SocketCacheKey(int contentItemId, string socketType, string displayType, string url)
        {
            Id = contentItemId;
            DisplayType = displayType;
            SocketType = socketType;
            // TODO: The only reason we need URL right now is for the menu, must be a more efficient approach to this.
            Url = url;
        }
    }
}

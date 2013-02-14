using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Framework
{
    /// <summary>
    /// </summary>
    public class SocketsModel
    {

        public SocketsModel(Orchard.ContentManagement.IContent left, string displayType, SocketParentContext parentContext)
        {
            this.LeftContent = left;
            this.DisplayType = displayType;
            ParentContext = parentContext;
        }

        public IContent LeftContent { get; set; }
        public SocketParentContext ParentContext { get; set; } 
        public string DisplayType { get; set; }
    }
}
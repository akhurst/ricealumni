using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics.ViewModels
{
    public class RecursionPreventedModel
    {
        private Orchard.ContentManagement.IContent content;

        public RecursionPreventedModel(Orchard.ContentManagement.IContent content)
        {
            // TODO: Complete member initialization
            this.content = content;
        }
    }
}
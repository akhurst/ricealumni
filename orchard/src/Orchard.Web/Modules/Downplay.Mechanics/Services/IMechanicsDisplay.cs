using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Downplay.Mechanics.Models;
using Orchard.DisplayManagement.Implementation;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace Downplay.Mechanics.Services
{
    public interface IMechanicsDisplay : IDependency
    {
        void ApplyEditors(BuildEditorContext driverContext, SocketParentContext parentContext = null);
        void ApplyDisplay(BuildDisplayContext driverContext);
    }
}
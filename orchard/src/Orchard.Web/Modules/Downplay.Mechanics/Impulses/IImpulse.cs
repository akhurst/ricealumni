using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security.Permissions;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Web.Routing;
using Orchard.Localization;

namespace Downplay.Mechanics.Impulses
{
    public interface IImpulse
    {
        string Name { get; }
        LocalizedString  Caption { get; }
        LocalizedString Description { get; }
        dynamic Data { get; }
        RouteValueDictionary HrefRoute { get; } 
    }
}
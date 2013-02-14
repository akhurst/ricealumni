
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Core.Common.Models;
using Orchard.ContentManagement.Drivers;
using Downplay.Mechanics.Impulses.Services;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Impulses.Drivers
{
    /// <summary>
    /// Implementing a driver for commonpart to inject impulse shape
    /// TODO: Make it a handler or IContentPartDriver instead to avoid dependence on CommonPart
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseCommonDriver : ContentPartDriver<CommonPart>
    {
        private readonly IImpulseService _impulseService;

        public ImpulseCommonDriver(
            IImpulseService impulseService)
        {
            _impulseService = impulseService;
        }

        protected override DriverResult Display(CommonPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("ImpulsePanel", () => {
                var impulses = _impulseService.BuildImpulseShapes(part, displayType).ToList();
                return shapeHelper.ImpulsePanel(Impulses: impulses.ToList());
            });
        }

    }
}
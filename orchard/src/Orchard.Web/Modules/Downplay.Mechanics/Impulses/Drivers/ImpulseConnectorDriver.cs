using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Impulses.Services;

namespace Downplay.Mechanics.Impulses.Drivers
{
    /// <summary>
    /// Adds an impulse panel shape when rendering a connector that combines impulses from both connector and item.
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseConnectorDriver : LegacyModelDriver<ConnectorEventContext>
    {
            private readonly IImpulseService _impulseService;

            public ImpulseConnectorDriver(
            IImpulseService impulseService)
        {
            _impulseService = impulseService;
        }

        protected override string Prefix
        {
            get { return "ConnectorImpulses"; }
        }

        protected override ModelDriverResult Display(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            // TODO: Sequencing of impulses within panel
            // TODO: Hook into Origami for impulse rendering (much more flexible; could even contain form elements (once content prefixes are fixed))

            var impulses = _impulseService.BuildImpulseShapes(model.Right.Content, context.DisplayType).Concat(
                _impulseService.BuildImpulseShapes(model.ConnectorContent, context.DisplayType)).ToList();

            if (impulses.Count > 0)
            {
                var shape = shapeHelper.ImpulsePanel(Impulses: impulses);
                return ModelShape("ImpulsePanel",shape);
            }
            return null;
        }

        protected override ModelDriverResult Editor(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            var impulses = _impulseService.BuildImpulseShapes(model.Right.Content, context.DisplayType).Concat(
                _impulseService.BuildImpulseShapes(model.ConnectorContent, context.DisplayType)).ToList();

            if (impulses.Count > 0)
            {
                var shape = shapeHelper.ImpulsePanel(Impulses: impulses);
                return ModelShape("ImpulsePanel", shape);
            }
            return null;
        }

        protected override ModelDriverResult Update(ConnectorEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            var impulses = _impulseService.BuildImpulseShapes(model.Right.Content, context.DisplayType).Concat(
                _impulseService.BuildImpulseShapes(model.ConnectorContent, context.DisplayType)).ToList();

            if (impulses.Count > 0)
            {
                var shape = shapeHelper.ImpulsePanel(Impulses: impulses);
                return ModelShape("ImpulsePanel", shape);
            }
            return null;            
        }
    }
}
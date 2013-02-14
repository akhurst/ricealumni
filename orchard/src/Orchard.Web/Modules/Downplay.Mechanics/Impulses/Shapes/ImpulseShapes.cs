using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Downplay.Mechanics.Impulses.Services;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Environment;

namespace Downplay.Mechanics.Impulses.Shapes
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseShapes : IShapeTableProvider
    {
        private readonly Work<IImpulseService> _impulseService;
        public ImpulseShapes(Work<IImpulseService> impulseService)
        {
            _impulseService = impulseService;
        }

        public void Discover(ShapeTableBuilder builder)
        {
            bool impulsesAsWrappers = false;
            builder.Describe("Content")
                .OnDisplaying(displaying =>
                {
                    // Have to use 2nd displaying event so the content shape builder has added the wrapper
                    displaying.ShapeMetadata.OnDisplaying(displaying2 =>
                    {
                        if (displaying2.ShapeMetadata != null && displaying2.ShapeMetadata.DisplayType != null 
                            && !displaying2.ShapeMetadata.DisplayType.Contains("Admin")
                            && displaying2.ShapeMetadata.Wrappers != null 
                            && displaying2.ShapeMetadata.Wrappers.Contains("Content_ControlWrapper"))
                        {
                            // Remove original
                            displaying2.ShapeMetadata.Wrappers.Remove("Content_ControlWrapper");
                            if (impulsesAsWrappers)
                            {
                                // Add new
                                // TODO: Sort this out - get impulsesAsWrappers from site settings
                                switch (displaying2.ShapeMetadata.DisplayType)
                                {
                                    case "Link":
                                        displaying2.ShapeMetadata.Wrappers.Add("Content_ControlWrapper_Impulsive_Link");
                                        break;
                                    case "Menu":
                                        displaying2.ShapeMetadata.Wrappers.Add("Content_ControlWrapper_Impulsive_Menu");
                                        break;
                                    case "MenuSub":
                                        displaying2.ShapeMetadata.Wrappers.Add("Content_ControlWrapper_Impulsive_Menu");
                                        break;
                                    default:
                                        displaying2.ShapeMetadata.Wrappers.Add("Content_ControlWrapper_Impulsive");
                                        break;
                                }
                            }
                        }
                    });
                });
            builder.Describe("Widget")
                .OnDisplaying(displaying =>
                {
                        // Have to use 2nd displaying event so the widget shape builder has added the wrapper
                    displaying.ShapeMetadata.OnDisplaying(displaying2 =>
                    {
                        if (displaying2.ShapeMetadata != null && displaying2.ShapeMetadata.DisplayType != null
                            && !displaying2.ShapeMetadata.DisplayType.Contains("Admin")
                            && displaying2.ShapeMetadata.Wrappers != null
                            && displaying2.ShapeMetadata.Wrappers.Contains("Widget_ControlWrapper"))
                        {
                            // Remove original
                            displaying2.ShapeMetadata.Wrappers.Remove("Widget_ControlWrapper");
                            if (impulsesAsWrappers)
                            {
                                // Add new
                                displaying.ShapeMetadata.Wrappers.Add("Widget_ControlWrapper_Impulsive");
                            }
                        }
                    });
                });

            // Set up alternates for impulse templates
            builder.Describe("ImpulsePanel")
                .OnDisplaying(displaying =>
                {
                    if (!String.IsNullOrWhiteSpace(displaying.ShapeMetadata.DisplayType))
                    {
                        displaying.ShapeMetadata.Alternates.Add("ImpulsePanel_" + displaying.ShapeMetadata.DisplayType);
                    }
                });
            builder.Describe("Impulse")
                .OnDisplaying(displaying =>
                {
                    if (displaying.Shape.Impulse != null && displaying.Shape.ContentItem != null)
                    {
                        IImpulse impulse = displaying.Shape.Impulse;
                        IContent content = displaying.Shape.ContentItem;
                        displaying.ShapeMetadata.Alternates.Add("Impulse_" + impulse.Name);
                    }
                });
        }

    }
}
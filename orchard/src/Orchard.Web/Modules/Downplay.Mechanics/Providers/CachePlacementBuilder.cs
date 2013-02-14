using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Placement;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Providers {
    public class CachePlacementBuilder : IPlacementAlterationProvider {
        public void Alter(Orchard.DisplayManagement.Descriptors.PlacementInfo placement, string property, string value) {
            if (property == "cache") {
                // TODO: Could accept more behaviour options than just "true"; this might help us extend validation triggers (altho we can just do that by shape)
                var cache = value == "true";
                var model = placement as ModelPlacementInfo;
                if (model != null) {
                    model.AddMutator(
                        (placementInfo, parentShape, shape, metadata, context) => {
                            var socketModel = context.Model as SocketDisplayContext;
                            socketModel.CacheSocket = cache;
                        });
                }
            }
        }
    }
}
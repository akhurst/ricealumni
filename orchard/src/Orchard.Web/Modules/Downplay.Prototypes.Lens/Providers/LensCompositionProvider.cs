using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Factories;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement.Shapes;
using Downplay.Origami.Services;

namespace Downplay.Prototypes.Lens.Providers {
    public class LensCompositionProvider : ICompositionProvider {

        public void Subscribe(RootBuilder describe) {
            describe.Scope("Search.Ajax", compose => {

                // Chain include content list
                compose.Chain("Ajax");
                compose.Chain("ContentList");

                // Hijack with our own bits
                compose.Parameter<string>("query", query => {
                    query.Mutate<ContentList, ContentListQuery>((q,context) => {
                        q.Query = q.Query.Where<TitlePartRecord>(t => t.Title.StartsWith(query.Get(context)));
                    });
                });
                compose.Mutate<ContentList, ParadigmsContext>((p,context) => {
                    p.Add("LensResult");
                });
                compose.Mutate<ContentList, ShapeMetadata>((metadata,context) => {
                });
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Orchard.Environment.Extensions;
using Downplay.Alchemy.Factories;
using Orchard.Core.Title.Models;

namespace Downplay.Prototypes.Lens.Providers {
    [OrchardFeature("Downplay.Prototypes.Lens.FallbackSearch")]
    public class LensFallbackCompositionProvider : ICompositionProvider {

        public void Subscribe(RootBuilder describe) {

            describe.Scope("Search.Fallback", compose => {

                // Chain include content list
                compose.Chain("ContentList");

                // We get the dud path from the url
                compose.Parameter<string>("path", path => {

                    path.Mutate<ContentList, ContentListQuery>((q, context) => {
                        string param = path.Get(context);
                        string endBit = param;
                        var pos = param.LastIndexOf('/');
                        if (pos >= 0) {
                            endBit = param.Substring(pos + 1);
                        }
                        endBit = endBit.Replace('-', ' ');
                        // TODO: Any other replaces to fix url special chars etc.?

                        q.Query = q.Query.Where<TitlePartRecord>(t => t.Title.Contains(endBit));
                    });
                });
            });

        }
    }
}

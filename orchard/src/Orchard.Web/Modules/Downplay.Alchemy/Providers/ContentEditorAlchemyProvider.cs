using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Orchard.Environment.Extensions;

namespace Downplay.Alchemy.Providers {
    [OrchardFeature("Downplay.Alchemy.ContentEditor")]
    public class ContentEditorAlchemyProvider : ICompositionProvider {

        public void Subscribe(RootBuilder describe) {

            describe.Scope("Content.Edit", compose => {
                compose.Chain("Content");


            });

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Impulses
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add().DefineScript("Science_Impulses").SetUrl("Science.Impulses.js").SetDependencies("Delta_Html");
        }
    }
}
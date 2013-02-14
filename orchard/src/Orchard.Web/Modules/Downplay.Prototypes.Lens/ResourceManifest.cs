using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace Downplay.Prototype.Lens
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add().DefineStyle("ScienceLensAdmin").SetUrl("lens-admin.css");
            builder.Add().DefineStyle("ScienceLensFrontEnd").SetUrl("lens-front-end.css");
            builder.Add().DefineScript("ScienceLensUI").SetUrl("LensUI.js").SetDependencies("jQuery", "jQueryUI");
        }
    }
}
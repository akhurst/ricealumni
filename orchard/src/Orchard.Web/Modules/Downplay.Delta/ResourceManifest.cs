using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Resources;

namespace Downplay.Delta {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {

            // TODO: Can we directly reference .coffee files and get them autocompiled? Would be a nice module. Alternatively can just include the CoffeeScript js
            // and then use text/coffeescript in the script tag

            // jQuery cross-browser AJAX for IE
            builder.Add().DefineScript("jQuery_XDR").SetUrl("jquery.xdr.js").SetDependencies("jQuery"); // TODO: Limit to correct IE versions
            builder.Add().DefineScript("CoffeeScript").SetUrl("coffee-script.js").SetDependencies("jQuery");
            // TODO: Don't need swfobject, there's a Flash plugin in jquery_utils
            builder.Add().DefineScript("SWFObject").SetUrl("swfobject/swfobject.js").SetDependencies("jQuery");

            // Delta. TODO: Move to a separate project / module entirely
            builder.Add().DefineScript("Delta").SetUrl("delta/Delta.js").SetDependencies("jQuery");
            builder.Add().DefineScript("Delta_Math").SetUrl("delta/Delta.Math.js");
            builder.Add().DefineScript("Delta_Entities").SetUrl("delta/Delta.Entities.js").SetDependencies("Delta", "Delta_Math");
            builder.Add().DefineScript("Delta_Time").SetUrl("delta/Delta.Time.js").SetDependencies("Delta_Entities");
            builder.Add().DefineScript("Delta_Html").SetUrl("delta/Delta.Html.js").SetDependencies("Delta_Entities");

        }
    }
}
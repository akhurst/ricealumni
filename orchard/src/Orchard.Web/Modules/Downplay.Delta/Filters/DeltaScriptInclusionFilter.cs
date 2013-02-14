using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Mvc.Filters;
using System.Web.Mvc;
using Downplay.Delta.Services;
using Orchard.UI.Resources;
using Downplay.Alchemy.Dynamic;

namespace Downplay.Delta.Filters {
    /// <summary>
    /// Includes scripts when a result is executing, according to any delta components which are enabled
    /// </summary>
    public class DeltaScriptInclusionFilter : FilterProvider, IResultFilter {

        private readonly IEnumerable<IDeltaInstanceProvider> _deltaInstanceProviders;
        private readonly IResourceManager _resourceManager;

        public DeltaScriptInclusionFilter(
            IEnumerable<IDeltaInstanceProvider> deltaInstanceProviders,
            IResourceManager resourceManager
            ) {
                _deltaInstanceProviders = deltaInstanceProviders;
                _resourceManager = resourceManager;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            
            // Only need to include scripts on normal view results and not in admin
            // TODO: We could set an option in the context if we want the possibility of admin-side delta
            if (!(filterContext.Result is ViewResult) 
                || Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext))
                return;

            var context = new DeltaInstanceConfiguration();

            foreach (var p in _deltaInstanceProviders) {
                p.Configure(context);
            }

            dynamic stuff = new Stuff();

            var defs = context.Instances.Select(i => {

                foreach (var req in i.Requires) {
                    var settings = _resourceManager.Require(req.ResourceType, req.ResourceName);
                    if (req.Delegate != null) req.Delegate(settings);
                }

                return new {
                    Namespace = i.Namespace,
                    Type = i.TypeName,
                    Properties = i.Properties
                };

            });
            stuff.Instances = defs;
            string json = stuff.Json();
            string jscript = "Delta.Ready(function(){Delta.Configure("+json+");});";
            _resourceManager.RegisterFootScript("<script type=\"text/javascript\" >"+jscript+"</script>");
        }
    }
}
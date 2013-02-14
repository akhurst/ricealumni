using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Orchard;
using Orchard.ContentManagement;
using Downplay.Mechanics;
using Orchard.Mvc.Html;
using Downplay.Origami.Services;
using System.Web.Routing;
using System.Web.Mvc;
using Orchard.DisplayManagement.Shapes;
namespace Downplay.Theory.Cartography.Handlers
{
    public class MenuConnectorHandler : ConnectorHandler
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly RequestContext _requestContext;

        public MenuConnectorHandler(
            IWorkContextAccessor workContextAccessor,
            RequestContext requestContext
            ) {
                _workContextAccessor = workContextAccessor;
                _requestContext = requestContext;
        }

        private string _RequestUrl;
        /// <summary>
        /// Cached per instance, which should be once-per-request
        /// </summary>
        protected string RequestUrl { get {
            if (_RequestUrl == null) {
                var work = _workContextAccessor.GetContext();
                _RequestUrl = work.HttpContext.Request.Path.Replace(
                    work.HttpContext.Request.ApplicationPath, string.Empty)
                    .TrimEnd('/').ToUpperInvariant();
            }
            return _RequestUrl;
        } }

        protected override void Display(ConnectorDisplayContext model, dynamic shape, ModelShapeContext context)
        {
            if (context.Paradigms.Has("Navigation")) {
                model.SocketDisplayContext.Paradigms.Add("NavigationChild");
                context.Paradigms.Add("NavigationChild");
                bool isCurrent = false;
                string rightUrl = "";
                // HACK: Better than before but still a bit hackish
                if (model.Right.Content != null) {
                    // TODO: Make absolute 
                    var url = new UrlHelper(_requestContext);
                    rightUrl = url.ItemDisplayUrl(model.Right.Content);
                    // Check if it's a current page or parent
                    var work = _workContextAccessor.GetContext();
                    string modelUrl = rightUrl.Replace(work.HttpContext.Request.ApplicationPath, string.Empty).TrimEnd('/').ToUpperInvariant();
                    isCurrent = ((!string.IsNullOrEmpty(modelUrl) && RequestUrl.StartsWith(modelUrl)) || RequestUrl == modelUrl);
                    // Add Current paradigm so we can modify display
                    if (isCurrent) {
                        context.Paradigms.Add("Current");
                    }
                    else {
                        context.Paradigms.Remove("Current");
                    }
                }
                (shape.Metadata as ShapeMetadata).OnDisplaying(displaying => {
                    displaying.Shape.IsCurrent = isCurrent;
                    // Store display url
                    displaying.Shape.Url = rightUrl;
                });
            }
        }
        
    }
}
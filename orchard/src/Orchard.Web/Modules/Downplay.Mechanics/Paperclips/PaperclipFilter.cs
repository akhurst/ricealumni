using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using System.Web.Mvc;
using Downplay.Mechanics.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.UI.Admin;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Models;
using Orchard.DisplayManagement;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Paperclips
{
    /// <summary>
    /// Filters result to create socket shapes for Site and User
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipFilter : FilterProvider, IResultFilter
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrigamiService _origami;

        public PaperclipFilter(
            IWorkContextAccessor workContextAccessor,
            IOrigamiService origami,
            IShapeFactory shapeFactory
            )
        {
            _workContextAccessor = workContextAccessor;
            _origami = origami;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
            ShapeFactory = Shape = shapeFactory;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
        public IShapeFactory ShapeFactory { get; set; }
        public dynamic Shape { get; set; }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Paperclips should only run on a full view rendering result
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            // Don't run on Admin either
            if (AdminFilter.IsApplied(filterContext.RequestContext))
                return;

            var workContext = _workContextAccessor.GetContext(filterContext);

            // Some standard checks
            if (workContext == null ||
                workContext.Layout == null ||
                workContext.CurrentSite == null ||
                AdminFilter.IsApplied(filterContext.RequestContext))
            {
                return;
            }

            var site = workContext.CurrentSite;

            // Create a dummy sockets container which will be used to build user and site display. We don't really want to display this shape
            // (although it could possibly be useful) but we might want certain of its sockets or connectors to appear in zones, which will happen
            // during display building via socket events.
            var sockets = Shape.Sockets();

            var sitePart = site.As<SocketsPart>();
            if (sitePart != null)
            {
                // Building the display will cause any paperclips to perform push into layout
                // TODO: Probably building loads of inefficient sockets, need to check up on this and eradicate via placement.
                var model = new SocketsModel(sitePart, "Detail", null);
                var builder = _origami.Builder(model).WithDisplayType("Detail").WithMode("Display").WithParadigms(new[]{"Paperclip"});
                _origami.Build(builder,sockets);
            }
            // Same for User
            var user = workContext.CurrentUser;
            if (user != null)
            {
                var userPart = user.As<SocketsPart>();
                if (userPart != null)
                {
                    // Building the display will cause any paperclips to perform push into layout
                    var model = new SocketsModel(userPart, "Detail", null);
                    var builder = _origami.Builder(model).WithDisplayType("Detail").WithMode("Display").WithParadigms(new[] { "Paperclip" });
                    _origami.Build(builder, sockets);
                }
            }

        }
    }
}
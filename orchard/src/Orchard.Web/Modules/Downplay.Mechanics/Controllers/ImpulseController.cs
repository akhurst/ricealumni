using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Downplay.Mechanics.Impulses.Services;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Controllers
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseController : Controller
    {
        private readonly IImpulseService _impulseService;
        private readonly IContentManager _contentManager;
        public ImpulseController(
            IImpulseService impulseService,
            IContentManager contentManager)
        {
            _impulseService = impulseService;
            _contentManager = contentManager;
        }

        public ActionResult Actuate(string name, int? contentId = null, int? contentVersionId = null, string returnUrl = null)
        {
            IContent content = null;
            if (contentId.HasValue) {
                if (contentVersionId.HasValue) {
                    content = _contentManager.Get(contentId.Value, VersionOptions.VersionRecord(contentVersionId.Value));
                }
                else {
                    content = _contentManager.Get(contentId.Value, VersionOptions.Latest);
                }
            }

            var impulse = _impulseService.CheckForImpulse(name, content); // TODO: Extract data from query string
            if (impulse == null)
            {
                return HttpNotFound("Could not actuate impulse");
            }
            var context = new ImpulseContext()
            {
                Impulse = impulse,
                ReturnUrl = returnUrl
            };
            var result = _impulseService.ActuateImpulse(context);
            if (result == ImpulseActuationResult.NotAuthorized)
            {
                return new HttpUnauthorizedResult();
            }
            // Back to origin page (or other as defined in context)
            if (returnUrl!=null)
                return Redirect(context.ReturnUrl);

            return Json(true);
        }

    }
}
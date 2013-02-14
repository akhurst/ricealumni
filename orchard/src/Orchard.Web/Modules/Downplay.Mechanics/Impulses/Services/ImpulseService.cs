using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Security;
using Orchard.Localization;
using Orchard.DisplayManagement;
using System.Web.Routing;

namespace Downplay.Mechanics.Impulses.Services
{
    [OrchardFeature("Downplay.Mechanics.Impulses")]
    public class ImpulseService : IImpulseService
    {
        private readonly IEnumerable<IImpulseProvider> _impulseGenerators;
        private readonly IAuthorizationService _authorizationService;

        public ImpulseService(
            IOrchardServices services,
            IEnumerable<IImpulseProvider> impulseGenerators,
            IAuthorizationService authorizationService,
            IShapeFactory shapeFactory)
        {
            Services = services;
            _impulseGenerators = impulseGenerators;
            _authorizationService = authorizationService;
            T = NullLocalizer.Instance;
            Shape = shapeFactory;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public dynamic Shape { get; set; }
        private Dictionary<String,ImpulseDescriptor> _impulseDescriptors = null;
        public Dictionary<String,ImpulseDescriptor> GetDescriptors() {
            if (_impulseDescriptors == null) {
                var describe = new ImpulseDescribeContext();
                foreach (var p in _impulseGenerators) {
                    p.Describing(describe);
                }
                _impulseDescriptors = describe.Impulses;
            }
            return _impulseDescriptors;
        }
        public ImpulseDescriptor GetDescriptor(string name) {
            var descs = GetDescriptors();
            if (descs.ContainsKey(name)) return descs[name];
            return null;
        }
        public IEnumerable<IImpulse> ListImpulses(IContent content, string displayType, object data = null)
        {
            var impulses = new List<ImpulseDisplayContext>();
            foreach (var impulse in GetDescriptors().Values) {

                if (!CheckDescriptor(impulse, content, data)) {
                    continue;
                }
                int? versionId = null;
                if (content.ContentItem.VersionRecord != null) versionId = content.ContentItem.VersionRecord.Id;
                var display = new ImpulseDisplayContext(impulse) {
                    Content = content,
                    DisplayType = displayType,
                    Data = data,
                    HrefRoute = new RouteValueDictionary(new {
                        action = "Actuate",
                        controller = "Impulse",
                        area = "Downplay.Mechanics",
                        name = impulse.Name,
                        returnUrl = Services.WorkContext.HttpContext.Request.RawUrl,
                        contentId = content.Id,
                        contentVersionId = versionId
                    })
                };
                                
                foreach (var e in impulse.DisplayingHandlers) {
                    e(display);
                }
                impulses.Add(display);

            }
            return impulses;
        }
        protected bool CheckDescriptor(ImpulseDescriptor impulse, IContent content, object data = null) {
            var user = Services.WorkContext.CurrentUser;

            // Check permissions
            if (impulse.Permissions.Any(p => !_authorizationService.TryCheckAccess(p, user, content))) { //, T("Unable to perform action")))) {
                return false;
            }
            if (content != null) {
                if (impulse.ContentTypes.Any() && !impulse.ContentTypes.Contains(content.ContentItem.ContentType)) {
                    return false;
                }
                if (impulse.ContentFilters.Any(f => !f(content))) {
                    return false;
                }
            }
            return true;
        }
        public ImpulseDescriptor CheckForImpulse(string name, IContent content, object data = null)
        {
            var desc = GetDescriptor(name);
            if (desc == null) return null;

            if (!CheckDescriptor(desc, content, data)) return null;

            return desc;
        }

        public ImpulseActuationResult ActuateImpulse(ImpulseContext context)
        {
            // Check permissions
            var impulse = CheckForImpulse(context.ImpulseName, context.SourceContent);
            if (impulse == null) return ImpulseActuationResult.Failed;

            impulse.Actuate(context);

            if (context.Actuated)
            {
                return ImpulseActuationResult.Ok;
            }
            return ImpulseActuationResult.Failed;
        }

        public IEnumerable<dynamic> BuildImpulseShapes(IContent content, string displayType, object data = null)
        {
            var impulses = ListImpulses(content, displayType, data);
            foreach (var impulse in impulses)
            {
                yield return Shape.Impulse(ContentItem: content, Impulse: impulse);
            }
        }
    }
}
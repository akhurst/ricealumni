using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Filters;
using System.Web.Mvc;
using Orchard.UI.Navigation;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.UI.Admin;

namespace Downplay.Theory.Cartography.Providers {
    [OrchardFeature("Downplay.Theory.Cartography.HideCoreMenu")]
    [OrchardSuppressDependency("Orchard.UI.Navigation.MenuFilter")]
    public class MenuFilterSuppression : FilterProvider, IResultFilter {

                private readonly INavigationManager _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly dynamic _shapeFactory;
        private readonly MenuFilter _menuFilter;
        public MenuFilterSuppression(INavigationManager navigationManager,
            IWorkContextAccessor workContextAccessor,
            IShapeFactory shapeFactory) {

            _navigationManager = navigationManager;
            _workContextAccessor = workContextAccessor;
            _shapeFactory = shapeFactory;

            _menuFilter = new MenuFilter(_navigationManager, _workContextAccessor, _shapeFactory);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // Defer to underlying menu filter if in admin (otherwise we lose whole admin menu!)
            if (AdminFilter.IsApplied(filterContext.RequestContext)) {
                _menuFilter.OnResultExecuting(filterContext);
            }
        }
    }
}
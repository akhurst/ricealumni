using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Downplay.Alchemy.Events;
using Orchard;

namespace Downplay.Alchemy.Services {
    public interface IAlchemyService : IDependency {
        // CompositionContext Execute(string identifier, System.Web.Mvc.IValueProvider valueProvider);
        Composition Compose();
        Composition<T> Execute<T>(string id, System.Web.Mvc.IValueProvider ValueProvider);
    }
}

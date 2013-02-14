using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;
using Downplay.Alchemy.Factories;
using Downplay.Origami.Services;

namespace Downplay.Alchemy.Services {
    public class AlchemyService : IAlchemyService {
        private readonly ICompositionProvider _compositionEvents;
        private readonly IOrigamiService _origami;

        public AlchemyService(
            ICompositionProvider compositionEvents,
            IOrigamiService origami
            ) {
            _compositionEvents = compositionEvents;
            _origami = origami;
        }

        public Composition<T> Execute<T>(string identifier, System.Web.Mvc.IValueProvider valueProvider) {
            // TODO: Locally cache describe if we ever want >1 call to Execute
            var describe = new RootBuilder();
            _compositionEvents.Subscribe(describe);
            
            var context = new CompositionContext() {
                Identifier = identifier,
                Values = valueProvider,
                Builder = describe,
            };

            return new Composition<T>(context);
        }

        public Composition Compose() {
            var describe = new RootBuilder();
            _compositionEvents.Subscribe(describe);

            var context = new CompositionContext() {
                Builder = describe,
            };
            return new Composition(context);
        }
    }

    public static class AlchemyExtensions {

        public static Composition Scope(this Composition comp, string namedScope) {
            // comp.Input(new NamedScoped(namedScope));
            return comp;
        }

    }
}

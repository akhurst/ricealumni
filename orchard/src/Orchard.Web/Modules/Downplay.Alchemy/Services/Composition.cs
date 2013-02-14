using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;

namespace Downplay.Alchemy.Services {

    public class Composition {
        protected CompositionContext _context;

        public Composition Factory<T>(Func<CompositionContext, T> defaultFactory) {
            _context.Factory(() => defaultFactory(_context));
            return this;
        }

        public Composition(CompositionContext context) {
            _context = context;
        }

        public CompositionResult Go() {
            return _context.Build();
        }
    }

    public class Composition<T> : Composition {
        public Composition(CompositionContext context) : base(context) {
        }

        public Composition<T> With<TWith>(TWith with) {
            _context.Single(with);
            return this;
        }

        public T Go() {
            // TODO: Something seems wrong here...
            _context.Builder.Build(_context);
            return _context.Get<T>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Events;

namespace Downplay.Alchemy.Services {
    public class CompositionResult {

        protected CompositionContext _context;

        public T Build<T>() {
            return _context.Get<T>();
        }
    }
}

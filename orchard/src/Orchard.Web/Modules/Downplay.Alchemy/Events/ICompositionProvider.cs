using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;

namespace Downplay.Alchemy.Events {
    public interface ICompositionProvider : IEventHandler {

        void Subscribe(RootBuilder describe);

    }
}

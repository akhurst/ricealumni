using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace Downplay.Delta.Services {
    public interface IDeltaInstanceProvider : IDependency {
        void Configure(DeltaInstanceConfiguration config);
    }
}

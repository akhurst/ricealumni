using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace Downplay.Formula.Services {
    public interface IQueryCache : IDependency {

        
        IQuery Query();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;

namespace Downplay.Formula.Services {
    public interface IQueryCacheProvider : IEventHandler {

        void Describe(QueryDescribe describe);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Factories {
    public interface IContextTag {
    }

    public interface IContextTag<T> : IContextTag {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Mechanics {
    public interface ILazyFactory<TKey, TValue> {
        bool TryGetValue(TKey key, out TValue val);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Origami.ViewModels {

    /// <summary>
    /// This interface allows a Pager shape to be attached to an arbitrary model
    /// </summary>
    public interface IPagerQuery {

        int TotalCount { get; }
        void SetPage(int startIndex, int pageSize);

    }
}

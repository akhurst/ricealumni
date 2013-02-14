using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.ViewModels;

namespace Downplay.Mechanics.ViewModels
{
    public class MultipleChoiceListFinderModel : ChoiceListFinderModel
    {
        public MultipleChoiceListFinderModel()
            : base()
        {
            RightItemIds = Enumerable.Empty<int>();
        }

        public IEnumerable<int> RightItemIds { get; set; }
    }
}

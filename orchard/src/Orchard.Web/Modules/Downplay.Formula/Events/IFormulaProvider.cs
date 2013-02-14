using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;

namespace Downplay.Formula.Events
{
    public interface IFormulaProvider : IEventHandler
    {

        void Describe(DescribeForms context);

    }
}

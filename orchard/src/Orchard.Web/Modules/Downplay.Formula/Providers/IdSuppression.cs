using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Formula.Events;

namespace Downplay.Formula.Providers {
    /// <summary>
    /// Suppresses any fields named "Id" from displaying
    /// </summary>
    public class IdSuppression : IFormulaProvider {
        public void Describe(DescribeForms context) {
            context.ForField("Id").Suppress();
        }
    }
}

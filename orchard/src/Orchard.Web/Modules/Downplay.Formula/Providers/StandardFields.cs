using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Formula.Events;

namespace Downplay.Formula.Providers {
    public class StandardFields : IFormulaProvider {
        public void Describe(DescribeForms forms) {

            // Describe some standard value types

            //forms.ForValue<String>()
             //   .Map(s => new StringField(s)).Unmap(f => f.Value);

        }
    }
}

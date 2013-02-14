using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Formula.Events;

namespace Downplay.Formula.Tests {
    public class EconomicsTests : IFormulaProvider {
        public void Describe(DescribeForms context) {

            context.ForModel<SpecialOffer>(
                f => {
                    // Suppress start/end dates
                    f.ForField(m => m.StartDate).Suppress();
                    f.ForField(m => m.EndDate).Suppress();
           //         f.AddField("AppliesOn", 
             //           m => new DateRange(m.StartDate, m.EndDate),
               //         on=>on.Meta("Date range during which the offer takes effect"));
                });


        }
    }
}

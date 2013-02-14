using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class DescribeForModelType : DescribeModel {
        private Func<Type, bool> predicate;

        public DescribeForModelType(DescribeForms describe, Func<Type, bool> predicate) : base(describe) {
            this.predicate = predicate;
        }
    }
}

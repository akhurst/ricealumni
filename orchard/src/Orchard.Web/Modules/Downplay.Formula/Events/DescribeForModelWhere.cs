using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class DescribeForModelWhere<TModel> : DescribeModel {
        private Func<TModel, bool> predicate;

        public DescribeForModelWhere(DescribeForms describe, Func<TModel, bool> predicate) : base(describe) {
            this.predicate = predicate;
        }
    }
}

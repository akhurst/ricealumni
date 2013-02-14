using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    
    /// <summary>
    /// Base class for descriptor context scoped to a generic model delegate
    /// </summary>
    public abstract class DescribeModel : DescribeBase {
        public DescribeForms InnerDescribe { get; set; }

        public DescribeModel(DescribeForms inner) {
            InnerDescribe = inner;
        }
    }

    /// <summary>
    /// Descriptor context scoped to given model type
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DescribeForModel<TModel> : DescribeModel {

        public DescribeForModel(DescribeForms inner) : base(inner) {
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Downplay.Formula.Events {
    public class ForModel<T> {

        public ForModel<T> Property(Func<ForProperty, bool> where, Action<ForProperty> action) {
            // TODO:
            return this;
        }

        public ForModel<T> On<TProperty>(Func<T, TProperty> selector, Action<ForProperty> action) {
            // TODO:
            return this;
        }

        public ForModel<T> Chain<TValue>(string prefix, Func<T, TValue> factory, Action<ForProperty> action) {
            // TODO:
            return this;
        }

    }
}

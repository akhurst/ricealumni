using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Downplay.Formula.Events {
    public static class Extensions {

        public static DescribeForms ForModel<TModel>(this DescribeForms describe, Action<DescribeForModel<TModel>> action) {
            var dfm = new DescribeForModel<TModel>(describe);
            action.Invoke(dfm);
            return describe;
        }

        public static DescribeForModel<TModel> ForModel<TModel>(this DescribeForms describe) {
            return new DescribeForModel<TModel>(describe);
        }

        public static DescribeForModelType ForModelWhereType(this DescribeForms describe, Func<Type, bool> predicate) {
            return new DescribeForModelType(describe,predicate);
        }

        public static DescribeForModelWhere<TModel> ForModelWhere<TModel>(this DescribeForms describe, Func<TModel, bool> predicate) {
            return new DescribeForModelWhere<TModel>(describe, predicate);
        }

        public static DescribeField<TModel, TValue> ForField<TModel, TValue>(this DescribeForModel<TModel> describe, Expression<Func<TModel, TValue>> fieldExpression) {
            return new DescribeField<TModel, TValue>(describe, fieldExpression);
        }
        public static DescribeField ForField(this DescribeBase describe, string fieldName) {
            return new DescribeFieldByName(describe, fieldName);
        }
        public static T Suppress<T>(this T field)
            where T:DescribeField
        {
            field.AddBuilder(new MetaFieldBuilderDelegate(f => f.Meta.With<SuppressField>()));
            return field;
        }

    }
}

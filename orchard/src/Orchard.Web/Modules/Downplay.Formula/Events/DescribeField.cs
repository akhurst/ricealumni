using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Downplay.Formula.Events {

    public abstract class DescribeField : DescribeBase {
        private DescribeBase describe;

        protected DescribeBase InnerDescribe { get; set; }

        public DescribeField(DescribeBase describe) {
            InnerDescribe = describe;
        }

        public abstract void AddBuilder(IMetaFieldBuilder builder);
                
    }

    public class DescribeField<TModel, TValue> : DescribeField {
        private Expression<Func<TModel, TValue>> fieldExpression;

        public DescribeField(DescribeForModel<TModel> describe, Expression<Func<TModel, TValue>> fieldExpression) : base(describe) {
            this.fieldExpression = fieldExpression;
        }

        protected void PopulateFieldMeta(MetaField meta) {
      //      ModelMetadata metadata = ModelMetadata.FromLambdaExpression(fieldExpression, htmlHelper.ViewData);
            
        }

        public override void AddBuilder(IMetaFieldBuilder builder) {
            throw new NotImplementedException();
        }
    }

    public class DescribeFieldByName : DescribeField {
        public DescribeFieldByName(DescribeBase describe, string fieldName)
            : base(describe) {
            this.fieldName = fieldName;
        }


        public string fieldName { get; set; }

        public override void AddBuilder(IMetaFieldBuilder builder) {
            throw new NotImplementedException();
        }
    }
}

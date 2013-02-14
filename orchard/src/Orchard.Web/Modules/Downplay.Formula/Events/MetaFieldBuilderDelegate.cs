using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class MetaFieldBuilderDelegate : IMetaFieldBuilder {
        private readonly Action<MetaField> _builder;

        public MetaFieldBuilderDelegate(Action<MetaField> builder) {
            _builder = builder;
        }

        public void Build(MetaField meta) {
            _builder(meta);
        }

    }
}

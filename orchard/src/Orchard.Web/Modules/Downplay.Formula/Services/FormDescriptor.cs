using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events {
    public class FormDescriptor {
        private object form;
        private object context;

        public FormDescriptor(object form, object context) {
            // TODO: Complete member initialization
            this.form = form;
            this.context = context;
        }

        public IEnumerable<MetaField> Fields { get; set; }

    }
}

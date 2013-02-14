using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;
using Downplay.Formula.Services;

namespace Downplay.Formula.Drivers {
    public class FormulaModelDriver : ModelDriver {

        private readonly IFormulaService _formulaService;

        public FormulaModelDriver(
            IFormulaService formulaService
            ) {
                _formulaService = formulaService;
        }

        protected override string Prefix {
            get { throw new NotImplementedException(); }
        }

        public override ModelDriverResult Run(ModelShapeContext context) {
            var describe = _formulaService.DescribeModel(context.Model);
            var bits = describe.Fields.Select(f =>
                ChainShape(f.Model(context), context.Prefix + "." + f.Name, null));
            return Combined(bits.ToArray());
        }
    }
}

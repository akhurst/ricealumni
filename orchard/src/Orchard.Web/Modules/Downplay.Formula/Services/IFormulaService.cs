using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Formula.Events;

namespace Downplay.Formula.Services
{
    public interface IFormulaService
    {
        FormDescriptor DescribeModel(object model);
    }
}

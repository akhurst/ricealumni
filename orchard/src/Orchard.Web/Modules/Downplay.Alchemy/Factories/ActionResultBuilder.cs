using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Downplay.Alchemy.Factories {
    public class ActionResultBuilder : IContextTag<Func<ControllerBase, object, ActionResult>> {
    }
}

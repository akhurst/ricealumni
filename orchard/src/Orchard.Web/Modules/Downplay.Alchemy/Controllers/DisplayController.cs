using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Downplay.Alchemy.Services;
using Orchard.Themes;
using Downplay.Origami;
using Orchard.Mvc;
using Orchard.DisplayManagement.Shapes;
using ClaySharp;
using Downplay.Alchemy.Dynamic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Web.Script.Serialization;

namespace Downplay.Alchemy.Controllers {
    public class DisplayController : Controller {

        private readonly IAlchemyService _alchemy;

        public DisplayController(
            IAlchemyService alchemy
            ) {
                _alchemy = alchemy;
        }

        /// <summary>
        /// A simple action that renders output in flexible ways
        /// </summary>
        /// <param name="pipe"></param>
        /// <returns></returns>
        [Themed]
        public ActionResult Index(string id) {
            return _alchemy.Execute<ActionResult>(id, ValueProvider).With<ControllerBase>(this).Go();
            /*
            if (result.Shape == null)
                return View("Null");
             */
        }

    }
}

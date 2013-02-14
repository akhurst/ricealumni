using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Prototypes.Lens.ViewModels
{
    public class LensViewModel
    {

        /// <summary>
        /// Display type to render result content items with (if blank will default to same as Lens shape)
        /// </summary>
        public string ResultDisplayType { get; set; }


        public string Query { get; set; }
    }
}

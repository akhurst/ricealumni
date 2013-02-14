using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;

namespace Downplay.Formula.Events {
    public class ForProperty {
        public string Name { get; set; }
        public ParadigmsContext Paradigms { get; set; }

        /// <summary>
        /// TODO: Convert these into extension methods...
        /// </summary>
        /// <param name="suppress"></param>
        /// <returns></returns>
        public ForProperty Suppress(bool suppress=true) {
            if (suppress) {
                Paradigms.Add("Hidden");
            }
            else {
                Paradigms.Remove("Hidden");
            }
            return this;
        }

        public ForProperty Meta(string description) {
            return this;
        }
    }
}

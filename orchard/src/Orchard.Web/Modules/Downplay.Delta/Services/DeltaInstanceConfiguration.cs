using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Delta.Services {
    public class DeltaInstanceConfiguration {

        public DeltaInstanceConfiguration() {
            Instances = new List<DeltaInstanceDescriptor>();
        }

        public ICollection<DeltaInstanceDescriptor> Instances { get; set; }

        /// <summary>
        /// Scope the configurator to a script namespace
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requireScriptName"></param>
        public DeltaInstanceNamespaceConfiguration Namespace(string name, string requireScriptName = null) {
            requireScriptName = requireScriptName ?? name.Replace('.','_');
            return new DeltaInstanceNamespaceConfiguration(name,requireScriptName,this);
        }
    }
}

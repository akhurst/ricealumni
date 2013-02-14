using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Delta.Services {
    public class DeltaInstanceNamespaceConfiguration {
        private string ScopedNamespace;
        private string requireScriptName;
        private DeltaInstanceConfiguration config;

        public DeltaInstanceNamespaceConfiguration(string name, string requireScriptName, DeltaInstanceConfiguration deltaInstanceConfiguration) {
            // TODO: Complete member initialization
            this.ScopedNamespace = name;
            this.requireScriptName = requireScriptName;
            this.config = deltaInstanceConfiguration;
        }

        public DeltaInstanceNamespaceConfiguration Instance(string typeName, object properties = null, Action<DeltaInstanceDescriptor> modify = null) {
            var instance = new DeltaInstanceDescriptor(){
                Namespace = ScopedNamespace,
                TypeName = typeName
            }.Require(requireScriptName);
            if (properties != null)
                instance.Properties = properties;
            if (modify != null) modify(instance);
            config.Instances.Add(instance);
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Settings;
using Orchard.ContentManagement.MetaData.Models;

namespace Downplay.Mechanics.Framework {
    public class ConnectorDescriptor {

        public Orchard.ContentManagement.MetaData.Models.ContentTypeDefinition Definition { get; protected set; }
        public String Name { get { return Definition.Name; } }
        public ConnectorTypePartSettings Settings { get { return _Settings.Value; } }
        public ContentTypePartDefinition PartDefinition { get { return _PartDefinition.Value; } }
        protected Lazy<ConnectorTypePartSettings> _Settings;
        protected Lazy<ContentTypePartDefinition> _PartDefinition;

        public ConnectorDescriptor(Orchard.ContentManagement.MetaData.Models.ContentTypeDefinition connectorDefinition, ConnectorTypePartSettings settings = null) {

            // Store def
            Definition = connectorDefinition;

            // Initialize lazies
            _PartDefinition = new Lazy<ContentTypePartDefinition>(() => {
                var partDef = Definition.Parts.Where(p => p.PartDefinition.Name == "ConnectorPart").FirstOrDefault();
                if (partDef == null) throw new ArgumentException("Connector definition must have ConnectorPart");
                return partDef;
            });
            if (settings != null) {
                _Settings = new Lazy<ConnectorTypePartSettings>(() => settings);
            }
            else {
                _Settings = new Lazy<ConnectorTypePartSettings>(() => PartDefinition.Settings.GetModel<ConnectorTypePartSettings>());
            }
        }
        
        /// <summary>
        /// TODO: This still isn't the place for it; descriptors should exist all over the place
        /// </summary>
        public string DisplayType { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.ViewModels;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement;

namespace Downplay.Mechanics.Settings
{
    public class ConnectorTypePartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "ConnectorPart")
                yield break;
            var model = definition.Settings.GetModel<ConnectorTypePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "ConnectorPart")
                yield break;
            var model = new ConnectorTypePartSettings();
            updateModel.TryUpdateModel(model, "ConnectorTypePartSettings", null, null);
            builder.WithSetting("ConnectorTypePartSettings.AllowMany", model.AllowMany.ToString());
            builder.WithSetting("ConnectorTypePartSettings.AllowNone", model.AllowNone.ToString());
            builder.WithSetting("ConnectorTypePartSettings.AllowDuplicates", model.AllowDuplicates.ToString());
            builder.WithSetting("ConnectorTypePartSettings.AllowedContentLeft", model.AllowedContentLeft);
            builder.WithSetting("ConnectorTypePartSettings.AllowedContentRight", model.AllowedContentRight);
            builder.WithSetting("ConnectorTypePartSettings.InverseConnectorType", model.InverseConnectorType);
            builder.WithSetting("ConnectorTypePartSettings.DefaultParadigms", model.DefaultParadigms);
            builder.WithSetting("ConnectorTypePartSettings.SocketDisplayName", model.SocketDisplayName);
            builder.WithSetting("ConnectorTypePartSettings.SocketEditorHint", model.SocketEditorHint);
            builder.WithSetting("ConnectorTypePartSettings.SocketDisplayHint", model.SocketDisplayHint);
            builder.WithSetting("ConnectorTypePartSettings.SocketDisplayType", model.SocketDisplayType);
            builder.WithSetting("ConnectorTypePartSettings.SocketGroupName", model.SocketGroupName);
            builder.WithSetting("ConnectorTypePartSettings.ConnectorDisplayType", model.ConnectorDisplayType);

            yield return DefinitionTemplate(model);
        }
    }
}

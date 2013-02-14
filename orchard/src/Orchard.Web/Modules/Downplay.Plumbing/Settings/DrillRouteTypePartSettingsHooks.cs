using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.ViewModels;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Downplay.Plumbing.Settings
{
    public class DrillRouteTypePartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "DrillRoutePart")
                yield break;
            var model = definition.Settings.GetModel<DrillRouteTypePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "DrillRoutePart")
                yield break;
            var model = new DrillRouteTypePartSettings();
            updateModel.TryUpdateModel(model, "DrillRouteTypePartSettings", null, null);

            builder.WithSetting("DrillRouteTypePartSettings.DrilledDisplayType", model.DrilledDisplayType);

            yield return DefinitionTemplate(model);
        }
    }
}

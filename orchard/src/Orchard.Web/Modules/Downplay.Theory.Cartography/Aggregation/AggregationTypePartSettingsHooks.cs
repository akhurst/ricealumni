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

namespace Downplay.Theory.Cartography.Aggregation
{
    [OrchardFeature("Downplay.Theory.Cartography.Aggregation")]
    public class AggregationTypePartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "ConnectorPart")
                yield break;
            var model = definition.Settings.GetModel<AggregationTypePartSettings>();
            yield return DefinitionTemplate(model);
        }
        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "ConnectorPart")
                yield break;
            var model = new AggregationTypePartSettings();
            updateModel.TryUpdateModel(model, "AggregationTypePartSettings", null, null);
            builder.WithSetting("AggregationTypePartSettings.ExposeFeed", model.ExposeFeed.ToString());

            yield return DefinitionTemplate(model);
        }

    }
}
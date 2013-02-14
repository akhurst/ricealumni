using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Downplay.Mechanics.Models;
using System;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Paperclips
{
    /// <summary>
    /// Content type settings for SubMenu part
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipTypePartSettings
    {
        public PaperclipTypePartSettings()
        {
            DefaultPlacement = "AsideSecond:20";
            DefaultDisplayType = "Summary";
            AllowChangeDisplayType = false;
            AllowChangeDisplayType = false;
        }

        public String DefaultPlacement {
            get; set;
        }
        public String DefaultDisplayType {
            get; set;
        }
        public bool AllowChangePlacement {
            get; set;
        }
        public bool AllowChangeDisplayType {
            get; set;
        }

    }

    /// <summary>
    /// Overrides default editors to enable putting settings on SubMenu part.
    /// </summary>
    [OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipPartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        /// <summary>
        /// Overrides editor shown when part is attached to content type. Enables adding setting field to the content part
        /// attached.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "PaperclipPart")
                yield break;
            var model = definition.Settings.GetModel<PaperclipTypePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.Name != "PaperclipPart")
                yield break;

            var model = new PaperclipTypePartSettings();

            updateModel.TryUpdateModel(model, "PaperclipTypePartSettings", null, null);
			builder.WithSetting("PaperclipTypePartSettings.DefaultPlacement", model.DefaultPlacement);
			builder.WithSetting("PaperclipTypePartSettings.DefaultDisplayType", model.DefaultDisplayType);
			builder.WithSetting("PaperclipTypePartSettings.AllowChangePlacement", model.AllowChangePlacement.ToString());
            builder.WithSetting("PaperclipTypePartSettings.AllowChangeDisplayType", model.AllowChangeDisplayType.ToString());

            yield return DefinitionTemplate(model);
        }

    }
}
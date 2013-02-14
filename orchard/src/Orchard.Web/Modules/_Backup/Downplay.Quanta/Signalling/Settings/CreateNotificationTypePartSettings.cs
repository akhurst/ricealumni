using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.ViewModels;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Environment.Extensions;
using Downplay.Quanta.Signalling.Models;

namespace Downplay.Quanta.Signalling.Settings
{
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class CreateNotificationTypePartSettings : NotificationTypePartSettings
    {

        public override string MessageType
        {
            get { return MessageTypes.CreateNotification; }
        }
    }

    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class CreateNotificationPartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        /// <summary>
        /// Overrides editor shown when part is attached to content type. Enables adding setting field to the content part
        /// attached.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "CreateNotificationPart")
                yield break;
            var model = definition.Settings.GetModel<CreateNotificationTypePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(global::Orchard.ContentManagement.MetaData.Builders.ContentTypePartDefinitionBuilder builder, global::Orchard.ContentManagement.IUpdateModel updateModel)
        {
            if (builder.Name != "CreateNotificationPart")
                yield break;
            var model = new CreateNotificationTypePartSettings();
            updateModel.TryUpdateModel(model, "CreateNotificationTypePartSettings", null, null);
            builder.WithSetting("CreateNotificationTypePartSettings.NotifyOwner",
                (model.NotifyOwner).ToString());
            builder.WithSetting("CreateNotificationTypePartSettings.NotifyContainerOwner",
                (model.NotifyContainerOwner).ToString());

            builder.WithSetting("CreateNotificationTypePartSettings.UsersToNotify",
                (model.UsersToNotify));

            yield return DefinitionTemplate(model);
        }

    }
}
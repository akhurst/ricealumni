using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.ViewModels;
using Orchard.ContentManagement.MetaData.Models;
using Downplay.Quanta.Signalling.Models;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Signalling.Settings
{
    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class PublishNotificationTypePartSettings : NotificationTypePartSettings
    {

        public override string MessageType
        {
            get { return MessageTypes.PublishNotification; }
        }
    }

    [OrchardFeature("Downplay.Quanta.Signalling")]
    public class PublishNotificationPartSettingsHooks : ContentDefinitionEditorEventsBase
    {

        /// <summary>
        /// Overrides editor shown when part is attached to content type. Enables adding setting field to the content part
        /// attached.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition)
        {
            if (definition.PartDefinition.Name != "PublishNotificationPart")
                yield break;
            var model = definition.Settings.GetModel<PublishNotificationTypePartSettings>();
            yield return DefinitionTemplate(model);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(global::Orchard.ContentManagement.MetaData.Builders.ContentTypePartDefinitionBuilder builder, global::Orchard.ContentManagement.IUpdateModel updateModel)
        {
            if (builder.Name != "PublishNotificationPart")
                yield break;
            var model = new PublishNotificationTypePartSettings();
            updateModel.TryUpdateModel(model, "PublishNotificationTypePartSettings", null, null);
            builder.WithSetting("PublishNotificationTypePartSettings.NotifyOwner",
                (model.NotifyOwner).ToString());
            builder.WithSetting("PublishNotificationTypePartSettings.NotifyContainerOwner",
                (model.NotifyContainerOwner).ToString());

            builder.WithSetting("PublishNotificationTypePartSettings.UsersToNotify",
                (model.UsersToNotify));

            yield return DefinitionTemplate(model);
        }
    
    }
}
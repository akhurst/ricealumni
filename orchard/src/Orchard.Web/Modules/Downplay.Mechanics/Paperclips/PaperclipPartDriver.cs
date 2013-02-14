using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using Downplay.Mechanics.Models;
using Orchard.Environment.Extensions;

namespace Downplay.Mechanics.Paperclips.Drivers
{
    [UsedImplicitly]
	[OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipPartDriver : ContentPartDriver<PaperclipPart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts.Paperclip.Edit";

        public Localizer T { get; set; }

        public PaperclipPartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(PaperclipPart part, string displayType, dynamic shapeHelper)
        {
//            return ContentShape("Parts_Paperclip",
  //              () => shapeHelper.Parts_Paperclip(ContentItem: part.ContentItem));

            return new DriverResult();
        }

        protected override DriverResult Editor(PaperclipPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Paperclip_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(PaperclipPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            // TODO: Per-connector position setting
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
            }
            return Editor(part, shapeHelper);
        }

    }
}
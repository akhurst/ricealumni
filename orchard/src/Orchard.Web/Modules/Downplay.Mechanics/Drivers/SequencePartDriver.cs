using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Drivers
{
    public class SequencePartDriver : ContentPartDriver<SequencePart>
    {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts.Sequence";

        public Localizer T { get; set; }

        public SequencePartDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;
        }

        protected override DriverResult Display(SequencePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Sequence",
                () => shapeHelper.Parts_Sequence(ContentItem: part.ContentItem));
        }

        protected override DriverResult Editor(SequencePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Sequence_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName+".Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(SequencePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
            }
            return Editor(part, shapeHelper);
        }

    }
}
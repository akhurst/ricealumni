using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Downplay.Mechanics.Models;
using Orchard.Environment.Extensions;
using Downplay.Mechanics.Settings;
using Downplay.Mechanics.Paperclips;

namespace Downplay.Mechanics.Handlers
{
    [UsedImplicitly]
	[OrchardFeature("Downplay.Mechanics.Paperclips")]
    public class PaperclipPartHandler : ContentHandler
    {
        public PaperclipPartHandler(IRepository<PaperclipPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            OnCreating<PaperclipPart>((creating,part)=>
            {
                var settings = part.TypePartDefinition.Settings.GetModel<PaperclipTypePartSettings>();
                part.Placement = settings.DefaultPlacement;
                part.DisplayType = settings.DefaultDisplayType;
            });
        }
    }
}

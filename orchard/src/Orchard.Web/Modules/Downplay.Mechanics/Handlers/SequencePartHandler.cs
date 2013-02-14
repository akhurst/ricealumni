using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Handlers
{
    [UsedImplicitly]
    public class SequencePartHandler : ContentHandler
    {
        public SequencePartHandler(IRepository<SequencePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}

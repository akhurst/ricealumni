using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Filters;

namespace Downplay.Mechanics.Handlers
{
    public class SequenceSocketHandler : SocketHandler
    {
        public SequenceSocketHandler()
        {
        }
        protected override void Filtering(SocketEventContext context)
        {
            if (context.Connector.Definition.Parts.Any(p => p.PartDefinition.Name == "SequencePart"))
            {
                context.SocketSorters.Add(new SequenceSortingFilter());
            }

        }

    }
}
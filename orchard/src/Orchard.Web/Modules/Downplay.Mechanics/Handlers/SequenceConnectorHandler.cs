using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Handlers
{
    /// <summary>
    /// Responsible for automatically picking the next sequence number
    /// </summary>
    public class SequenceConnectorHandler : ConnectorHandler
    {
        public SequenceConnectorHandler() { }
        protected override void Created(ConnectorCreateContext context)
        {

            // Determine new sequence number
            var sequence = context.ConnectorContent.As<SequencePart>();
            if (sequence == null) return;

            int? finalSequence = null;
            if (sequence.Sequence>0) {
                finalSequence = sequence.Sequence;
            }

            if (!finalSequence.HasValue)
            {
                var highest = context.SiblingConnectors.ForPart<SequencePart>().Join<SequencePartRecord>().OrderByDescending(s => s.Sequence).Slice(0, 1).FirstOrDefault();
                if (highest != null)
                {
                    finalSequence = highest.Sequence + 1;
                }
            }
            if (!finalSequence.HasValue){
                // Default to 1
                finalSequence = 1;
            }
            // TODO: Allow creating at a different sequence, e.g. if we dropped a content into the middle of a list. Maybe a hidden field on a creator.
            if (finalSequence.HasValue)
            {
                sequence.Sequence = finalSequence.Value;
            }
        }
        protected override void CreatingInverse(ConnectorCreateContext context)
        {
            // When creating inverse try and use the same sequence number
            var sequence = context.ConnectorContent.As<SequencePart>();
            if (sequence==null) return;

            var sequenceInverse = context.InverseConnectorContent.As<SequencePart>();
            if (sequenceInverse != null)
            {
                sequence.Sequence = sequenceInverse.Sequence;
            }
        }
    }
}
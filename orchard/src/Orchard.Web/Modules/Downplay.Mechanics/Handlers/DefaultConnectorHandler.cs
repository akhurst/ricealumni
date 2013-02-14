using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;

using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Title.Models;
using Orchard.Core.Common.Models;
namespace Downplay.Mechanics.Handlers {
    public class DefaultConnectorHandler : ConnectorHandler {

        protected override void Creating(ConnectorCreateContext context) {

            // Populate some basic parts
            var connTitle = context.ConnectorContent.As<TitlePart>();
            if (connTitle != null) {
                connTitle.Title = context.Right.Metadata.DisplayText;
            }

            // TODO: Basic validation (multiplicity, duplicates, etc.) needs moving from UI drivers that currently handle it

        }

        protected override void Displaying(ConnectorDisplayContext model, Origami.Services.ModelShapeContext context) {

            var connTitle = model.ConnectorContent.As<TitlePart>();
            if (connTitle != null && !String.IsNullOrWhiteSpace(connTitle.Title)) {
                context.Paradigms.Add("TitleOverride");
            }
            var connBody = model.ConnectorContent.As<BodyPart>();
            if (connBody != null && !String.IsNullOrWhiteSpace(connBody.Text)) {
                context.Paradigms.Add("BodyOverride");
            }

        }

    }
}
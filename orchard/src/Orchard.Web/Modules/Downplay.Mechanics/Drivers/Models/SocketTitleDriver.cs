using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Downplay.Mechanics.ViewModels;
using Orchard.Localization;

namespace Downplay.Mechanics.Drivers.Models {
    public class SocketTitleDriver : ModelDriver<SocketEventContext> {

        protected override string Prefix {
            get { return "SocketTitle"; }
        }

        protected override ModelDriverResult Build(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context) {
            return Combined(
                ModelShape("Sockets_SocketTitle", () => shapeHelper.Sockets_SocketTitle(SocketTitle: model.SocketMetadata.SocketTitle ?? "", SocketHint: model.SocketMetadata.DisplayHint)),
                ModelShape("Sockets_SocketTitle_Edit", () => shapeHelper.Sockets_SocketTitle_Edit(SocketTitle: model.SocketMetadata.SocketTitle ?? "", SocketHint: model.SocketMetadata.EditorHint))
                );
        }

        protected override void Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Drivers
{
    public class SocketCountDriver : LegacyModelDriver<SocketEventContext>
    {
        protected override string Prefix
        {
            get { return "ItemCount"; }
        }

        protected override ModelDriverResult Display(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return ModelShape("Socket_Count", () => {
                // TODO: Unfortunately this return count *after* paging so we need a separate lazy for this...
                var count = model.Query.TotalCount;
                return shapeHelper.Socket_Count(Count: count, SocketTitle: model.SocketMetadata.SocketTitle);
            });
        }

        protected override ModelDriverResult Editor(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            return null;
        }
    }
}
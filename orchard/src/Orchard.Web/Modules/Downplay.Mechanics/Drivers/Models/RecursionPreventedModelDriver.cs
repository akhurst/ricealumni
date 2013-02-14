using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.ViewModels;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Drivers
{
    public class RecursionPreventedModelDriver : LegacyModelDriver<RecursionPreventedModel>
    {

        protected override string Prefix
        {
            get { return "RecursionPrevented"; }
        }

        protected override ModelDriverResult Display(RecursionPreventedModel model, dynamic shapeHelper, ModelShapeContext context)
        {
            return ModelShape("RecursionPrevented", shapeHelper.RecursionPrevented());
        }

        protected override ModelDriverResult Editor(RecursionPreventedModel model, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Update(RecursionPreventedModel model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
        {
            return null;
        }
    }
}
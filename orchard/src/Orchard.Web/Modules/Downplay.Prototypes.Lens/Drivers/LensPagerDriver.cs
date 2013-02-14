using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.UI.Navigation;
using Downplay.Prototypes.Lens.ViewModels;
using Downplay.Origami.Services;

namespace Downplay.Prototypes.Lens.Drivers
{
    public class LensPagerDriver : ModelDriver<LensViewModel,Pager>
    {
        protected override Pager ViewModel(LensViewModel model, ModelShapeContext context)
        {
            //return new Pager(
            return null;
        }

        protected override string Prefix
        {
            get { return "Pager"; }
        }

        protected override ModelDriverResult Build(LensViewModel model, Pager viewModel, dynamic shapeHelper, ModelShapeContext context) {
            return null;
        }

        protected override void Update(LensViewModel model, Pager viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
        }
    }
}

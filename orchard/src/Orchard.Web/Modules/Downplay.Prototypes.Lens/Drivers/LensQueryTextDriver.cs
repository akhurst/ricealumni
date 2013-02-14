using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;
using Downplay.Prototypes.Lens.ViewModels;

namespace Downplay.Prototypes.Lens.Drivers
{
    public class LensQueryTextDriver : ModelDriver<LensViewModel,LensQueryTextViewModel>
    {
        protected override string Prefix
        {
            get { return "QueryText"; }
        }

        protected override LensQueryTextViewModel ViewModel(LensViewModel model, ModelShapeContext context)
        {
            return new LensQueryTextViewModel();
        }

        protected override ModelDriverResult Build(LensViewModel model, LensQueryTextViewModel viewModel, dynamic shapeHelper, ModelShapeContext context) {
            var prefix = FullPrefix(context);
            return ModelShape("Lens_Filters_QueryText",
                () => shapeHelper.Lens_Filters_QueryText(Model: viewModel, Prefix: prefix));
        }

        protected override void Update(LensViewModel model, LensQueryTextViewModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
            updater.TryUpdateModel(model, FullPrefix(context), null, null);
        }
    }
}

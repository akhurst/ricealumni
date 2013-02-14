using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;
using Downplay.Prototypes.Lens.ViewModels;
using Downplay.Prototypes.Lens.Services;
using Orchard.Environment;

namespace Downplay.Prototypes.Lens.Drivers
{
    /// <summary>
    /// 
    /// </summary>
    public class LensResultsDriver : ModelDriver<LensResultsViewModel>
    {
        private readonly Work<ILensService> _lensService;
        public LensResultsDriver(
            Work<ILensService> lensService
            ) {
                _lensService = lensService;
        }

        protected override string Prefix
        {
            get { return "LensResults"; }
        }

        protected override ModelDriverResult Build(LensResultsViewModel model, dynamic shapeHelper, ModelShapeContext context) {
            return null;
        }

        protected override void Update(LensResultsViewModel model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
            // This model runs after the first LensViewModel, so we've collected filters etc.
        }

    }
}

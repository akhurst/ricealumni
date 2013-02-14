using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions.Models;

namespace Downplay.Origami.Services
{

    /// <summary>
    /// ModelDrivers are very similar to ContentPartDriver but can build arbitrary shapes around any model
    /// This is the legacy version for back-compat prior to the full Paradigms system (0.997).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LegacyModelDriver<T> : ModelDriver
    {
        public LegacyModelDriver() { }

        public override ModelDriverResult Run(ModelShapeContext context) {
            switch (context.Mode) {
                case "Editor":
                    if (context.Updater != null) {
                        return UpdateEditor(context);
                    }
                    return BuildEditor(context);
                case "Display":
                    // TODO: Maybe run BuildDisplay for all non-Editor modes? ... Or just convert legacies ...
                    return BuildDisplay(context);
            }
            return null;
        }

        ModelDriverResult BuildDisplay(ModelShapeContext context)
        {
            if (context.Model is T)
            {
                return Display((T)context.Model, context.New, context);
            }
            return null;
        }

        ModelDriverResult UpdateEditor(ModelShapeContext context)
        {
            if (context.Model is T)
            {
                return Update((T)context.Model, context.New, context.Updater, context);
            }
            return null;
        }

        ModelDriverResult BuildEditor(ModelShapeContext context)
        {
            if (context.Model is T)
            {
                return Editor((T)context.Model, context.New, context);
            }
            return null;
        }

        protected abstract ModelDriverResult Display(T model, dynamic shapeHelper, ModelShapeContext context);
        protected abstract ModelDriverResult Editor(T model, dynamic shapeHelper, ModelShapeContext context);
        protected abstract ModelDriverResult Update(T model, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context);

    }

    /// <summary>
    /// Abstracts a model driver that generates another ViewModel
    /// TODO: We want to factorise the ViewModel(...) call so it happens within the shape factory and is therefore prevented by placement.
    /// I don't know if I have any complicated processing going on in those methods but it's for the greater good :) PH
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class LegacyModelDriver<TModel, TViewModel> : LegacyModelDriver<TModel>
    {
        public LegacyModelDriver() { }

        public abstract TViewModel ViewModel(TModel model, ModelShapeContext context);

        protected override ModelDriverResult Display(TModel model, dynamic shapeHelper, ModelShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Display(model, viewModel, shapeHelper, context);
        }

        protected virtual ModelDriverResult Display(TModel model, TViewModel viewModel, dynamic shapeHelper, ModelShapeContext context)
        {
            return null;
        }

        protected override ModelDriverResult Editor(TModel model, dynamic shapeHelper, ModelShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Editor(model, viewModel, shapeHelper, context);
        }

        protected virtual ModelDriverResult Editor(TModel model, TViewModel viewModel, dynamic shapeHelper, ModelShapeContext context)
        {
            // By default we only have to implement Update and always check whether updater != null ...
            return Update(model, viewModel, shapeHelper, null, context);
        }

        protected override ModelDriverResult Update(TModel model, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context)
        {
            var viewModel = ViewModel(model, context);
            return Update(model, viewModel, shapeHelper, updater, context);
        }

        protected virtual ModelDriverResult Update(TModel model, TViewModel viewModel, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context)
        {
            return null;
        }

    }

}
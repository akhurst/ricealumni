using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement;

namespace Downplay.Origami.Services {

    /// <summary>
    /// Base implementation details for ModelDrivers, provides support for hooking in Placement, and helper methods
    /// to create factory ModelDriverResults.
    /// </summary>
    public abstract class ModelDriver : IModelDriver {

        protected abstract String Prefix { get; }

        public abstract ModelDriverResult Run(ModelShapeContext context);

        protected ModelDriverResult ModelShape(string shapeType, string differentiator, dynamic shape) {
            // NOTE: Even though we're passing the wrong Prefix in here, i.e. not FullPrefix, and it ends up in ShapeMetadata; it doesn't get referenced from ANYwhere
            // that I could find so maybe it's not a problem...
            // TODO: Figure out and clean up the Prefix stuff here and elsewhere because it's super confusing. -PH
            return ContentShapeImplementation(shapeType, Prefix, ctx => shape).Differentiator(differentiator);
        }

        protected ModelDriverResult EditorShape(string shapeType, object model, ModelShapeContext context) {
            return ContentShapeImplementation(shapeType, FullPrefix(context), (ctx) => context.New.EditorTemplate(TemplateName: shapeType.Replace('_', '.'), Model: model, Prefix: FullPrefix(context)));
        }

        protected ModelDriverResult ModelShape(string shapeType, dynamic shape) {
            return ContentShapeImplementation(shapeType, Prefix, ctx => shape);
        }

        protected ModelDriverResult ModelShape(string shapeType, Func<dynamic> factory) {
            return ContentShapeImplementation(shapeType, Prefix, ctx => factory());
        }

        protected ModelDriverResult ModelShape(string shapeType, Func<dynamic, dynamic> factory) {
            return ContentShapeImplementation(shapeType, Prefix, ctx => factory(CreateShape(ctx, shapeType)));
        }

        /// <summary>
        /// Renders a different model across the *same* shape; basically chaining onto a new viewmodel and allowing all drivers to run again
        /// </summary>
        protected ModelDriverResult ChainShape(object chainedModel, string prefix, Action complete) {
            return new ChainModelResult(chainedModel, prefix, ctx => complete());
        }

        /// <summary>
        /// Builds a child shape from an entirely new model. You provide a factory to create the *root* model, and Origami will allow any model drivers for the new model to run.
        /// </summary>
        protected ChildShapeResult ChildShape(string shapeType, object model, string prefix, Func<ModelChainContext, dynamic> shape) {
            return new ChildShapeResult(shapeType, shape, model, prefix);
        }

        private ModelShapeResult ContentShapeImplementation(string shapeType, string prefix, Func<ModelShapeContext, object> shapeBuilder) {
            return new ModelShapeResult(shapeType, prefix, shapeBuilder);
        }

        private static object CreateShape(ModelShapeContext context, string shapeType) {
            IShapeFactory shapeFactory = context.New;
            return shapeFactory.Create(shapeType);
        }

        protected CombinedModelResult Combined(params ModelDriverResult[] results) {
            return new CombinedModelResult(results);
        }

        /// <summary>
        /// Build prefix for template, using parent prefix if available
        /// TODO: Somehow force its usage instead of relying on callers
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected String FullPrefix(ModelShapeContext context, string fieldName = null) {
            var prefix = Prefix;
            if (!String.IsNullOrWhiteSpace(context.Prefix)) {
                prefix = context.Prefix + "." + prefix;
            }
            if (!String.IsNullOrWhiteSpace(fieldName)) {
                prefix = prefix + "." + fieldName;
            }
            return prefix;
        }

    }

    public abstract class ModelDriver<T> : ModelDriver {
        public override ModelDriverResult Run(ModelShapeContext context) {
            if (context.Model is T) {
                var model = (T)context.Model;
                if (Authorize(model, context)) {
                    if (context.Updater != null) {
                        Update(model, context.New, context.Updater, context);
                    }
                    return Build(model, context.New, context);
                }
            }
            return null;
        }

        protected virtual bool Authorize(T model, ModelShapeContext context) { return true; }

        /// <summary>
        /// Same method always builds the view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dynamic"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract ModelDriverResult Build(T model, dynamic shapeHelper, ModelShapeContext context);

        /// <summary>
        /// Update simply performs the update and generates no shapes
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dynamic"></param>
        /// <param name="iUpdateModel"></param>
        /// <param name="context"></param>
        protected abstract void Update(T model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context);
    }

    /// <summary>
    /// Map the model to another ViewModel
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class ModelDriver<TModel, TViewModel> : ModelDriver {

        public override ModelDriverResult Run(ModelShapeContext context) {
            // TODO: Oops. Need to prevent building a viewmodel when shapes aren't placed.
            if (context.Model is TModel) {
                var model = (TModel)context.Model;
                var viewModel = ViewModel(model, context);
                if (Authorize(model,viewModel,context)) {
                    if (context.Updater != null) {
                        Update(model, viewModel, context.New, context.Updater, context);
                    }
                    return Build(model, viewModel, context.New, context);
                }
            }
            return null;
        }

        protected virtual bool Authorize(TModel model, TViewModel viewModel, ModelShapeContext context) { return true; }

        protected abstract TViewModel ViewModel(TModel model, ModelShapeContext context);

        protected abstract ModelDriverResult Build(TModel model, TViewModel viewModel, dynamic shapeHelper, ModelShapeContext context);

        protected abstract void Update(TModel model, TViewModel viewModel, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context);
    }
}
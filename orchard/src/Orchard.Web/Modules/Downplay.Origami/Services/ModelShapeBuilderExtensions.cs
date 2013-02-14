using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Downplay.Origami.Services {
    /// <summary>
    /// Back-compact extensions since origami interface changed
    /// </summary>
    public static class ModelShapeBuilderExtensions {

        public static ModelShapeBuilder WithMode(this ModelShapeBuilder builder,string mode) {
            builder.Context.Mode = mode;
            return builder;
        }

        public static ModelShapeBuilder WithUpdater(this ModelShapeBuilder builder, IUpdateModel updater, string prefix) {
            builder.Context.Updater = updater;
            builder.Context.Prefix = prefix;
            return builder;
        }
        public static ModelShapeBuilder WithDisplayType(this ModelShapeBuilder builder, string displayType) {
            builder.Context.DisplayType = displayType;
            return builder;
        }
        public static ModelShapeBuilder WithStereotype(this ModelShapeBuilder builder, string stereotype) {
            builder.Context.Stereotype = stereotype;
            return builder;
        }
        public static ModelShapeBuilder WithContentType(this ModelShapeBuilder builder, string contentType) {
            builder.ContentType = contentType;
            return builder;
        }
        public static ModelShapeBuilder WithParent(this ModelShapeBuilder builder, ModelShapeContext parent) {
            if (parent == null) {
                builder.Context.ParentContext = null;
                builder.Context.CustomContext = null;
            }
            else {
                builder.Context.ParentContext = parent;
                builder.Context.CustomContext = parent.CustomContext;
            }
            return builder;
        }

        public static ModelShapeBuilder WithParadigms(this ModelShapeBuilder builder, params string[] paradigms)
        {
            builder.Context.Paradigms.Add(paradigms);
            return builder;
        }
        public static ModelShapeBuilder WithParadigms(this ModelShapeBuilder builder, IEnumerable<String> paradigms) {
            builder.Context.Paradigms.Add(paradigms);
            return builder;
        }
        public static ModelShapeBuilder WithParadigms(this ModelShapeBuilder builder, ParadigmsContext paradigms) {
            builder.Context.Paradigms = new ParadigmsContext(paradigms);
            return builder;
        }
        public static ModelShapeBuilder WithGroup(this ModelShapeBuilder builder, string groupId) {
            builder.Context.GroupId = groupId;
            return builder;
        }

        #region Temporary shims to make back-compat a bit easier

        // NOTE: Had to remove the dynamic parameter because it didn't play nice with extension methods

        public static ModelShapeBuilder BuildDisplayShape(this IOrigamiService origami, object model, string prefix, string displayType, string stereotype, string contentType = null, ModelShapeContext parentContext = null) {
            var builder = origami.Builder(model)
                .WithMode("Display")
                .WithUpdater(null, prefix)
                .WithDisplayType(displayType)
                .WithStereotype(stereotype)
                .WithContentType(contentType)
                .WithParent(parentContext);
            return builder;
        }
        public static ModelShapeBuilder BuildEditorShape(this IOrigamiService origami, object model, IUpdateModel updater, string prefix, string displayType, string stereotype, string contentType = null, ModelShapeContext parentContext = null) {
            var builder = origami.Builder(model)
                .WithMode("Editor")
                .WithUpdater(updater, prefix)
                .WithDisplayType(displayType)
                .WithStereotype(stereotype)
                .WithContentType(contentType)
                .WithParent(parentContext);
            return builder;
        }

        #endregion
    }
}
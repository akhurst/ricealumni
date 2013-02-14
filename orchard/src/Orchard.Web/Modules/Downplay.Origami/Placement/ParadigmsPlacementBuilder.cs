using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace Downplay.Origami.Placement {
    public class ParadigmsPlacementPredicateBuilder : IPlacementPredicateProvider {

        private readonly IWorkContextAccessor _workContextAccessor;

        public ParadigmsPlacementPredicateBuilder(
            IWorkContextAccessor workContextAccessor
            ) {
                _workContextAccessor = workContextAccessor;
        }

        public Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> Predicate(KeyValuePair<string, string> term, Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> predicate) {
            var expression = term.Value;
            // TODO: Support wildcards as does traditional placement
            // TODO: Support OR-paradigms and NOT-paradigms (i.e. Foo,Bar,!Car) - roles as well
            if (term.Key == "Paradigm")
            {
                return ctx => {
                    var model = ctx as ModelShapePlacementContext;
                    if (model != null) {
                        return (model.ModelContext.Paradigms.Has(expression)) && predicate(ctx);
                    }
                    return false;
                };
            }
            // Mode (Editor/Display)
            if (term.Key == "Mode") {
                return ctx => {
                    var model = ctx as ModelShapePlacementContext;
                    if (model != null) {
                        return (model.ModelContext.Mode == expression) && predicate(ctx);
                    }
                    return false;
                };
            }

            // Match User roles
            // TODO: Move to quanta?
            if (term.Key=="Role") {
                return ctx => {
                    // Hmm, will it work...
                    var context = _workContextAccessor.GetContext();
                    IEnumerable<string> roles = ((dynamic)context.CurrentUser).UserRolesPart.Roles;
                    return (roles.Contains(expression)) && predicate(ctx);
                };
            }

            return predicate;
        }

    }
    public class ParadigmsPlacementAlterationBuilder : IPlacementAlterationProvider {
        /// <summary>
        /// Allow injecting a new paradigm with ";paradigm=..."
        /// </summary>
        /// <param name="placement"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void Alter(Orchard.DisplayManagement.Descriptors.PlacementInfo placement, string property, string value) {
            var model = placement as ModelPlacementInfo;
            if (model!=null) {
                if (property == "paradigm") {
                    model.AddMutator((place, parentShape, shape, metadata, context) => {
                        context.Paradigms.Add(value);
                    });
                }
                if (property == "group") {
                    model.AddMutator((place, parentShape, shape, metadata, context) => {
                        context.GroupId = value;
                    });
                }
            }
        }
    }
}
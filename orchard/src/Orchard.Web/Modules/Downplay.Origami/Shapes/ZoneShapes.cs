using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement;
using Orchard;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using System.Web.Mvc;
using Orchard.UI;
using System.IO;

namespace Downplay.Origami.Shapes {
    public class ZoneShapes : IShapeTableProvider {

        private readonly Work<ITagBuilderFactory> _tagBuilderFactory;

        public ZoneShapes(Work<ITagBuilderFactory> tagBuilderFactory) {
            _tagBuilderFactory = tagBuilderFactory;
        }

        public void Discover(ShapeTableBuilder builder) {
        }

        [Shape]
        public void Zone(dynamic Display, dynamic Shape, TextWriter Output) {
            var factory = _tagBuilderFactory.Value;
            var zoneWrapper = _tagBuilderFactory.Value.Create(Shape,"div");
            bool hiddenIfEmpty = Shape.HiddenIfEmpty == null ? false : Shape.HiddenIfEmpty;
            bool empty = false;
            if (hiddenIfEmpty) {
                var items = ((IEnumerable<dynamic>)Shape);
                var count = items.Count();
                empty = (count == 0 || (count == 1 && (IsEmptyShape(((IEnumerable<dynamic>)Shape).First()))));
            }
            if (!empty) {
                Output.Write(zoneWrapper.ToString(TagRenderMode.StartTag));
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
                Output.Write(zoneWrapper.ToString(TagRenderMode.EndTag));
            }
        }

        private bool IsEmptyShape(dynamic shape) {
            return shape == null
                || shape.Source == null
                || shape.Source.Metadata == null
                || shape.Source.Metadata.ChildContent == null
                || String.IsNullOrEmpty(shape.Source.Metadata.ChildContent.ToString());
        }

        /// <summary>
        /// TODO: Implement as behavior
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private static IEnumerable<dynamic> ordered_hack(dynamic shape) {
            IEnumerable<dynamic> unordered = shape;
            if (unordered == null || unordered.Count() < 2)
                return shape;

            var i = 1;
            var progress = 1;
            var flatPositionComparer = new FlatPositionComparer();
            var ordering = unordered.Select(item => {
                var position = (item == null || item.GetType().GetProperty("Metadata") == null || item.Metadata.GetType().GetProperty("Position") == null)
                                   ? null
                                   : item.Metadata.Position;
                return new { item, position };
            }).ToList();

            // since this isn't sticking around (hence, the "hack" in the name), throwing (in) a gnome 
            while (i < ordering.Count()) {
                if (flatPositionComparer.Compare(ordering[i].position, ordering[i - 1].position) > -1) {
                    if (i == progress)
                        progress = ++i;
                    else
                        i = progress;
                }
                else {
                    var higherThanItShouldBe = ordering[i];
                    ordering[i] = ordering[i - 1];
                    ordering[i - 1] = higherThanItShouldBe;
                    if (i > 1)
                        --i;
                }
            }

            return ordering.Select(ordered => ordered.item).ToList();
        }
    }
}
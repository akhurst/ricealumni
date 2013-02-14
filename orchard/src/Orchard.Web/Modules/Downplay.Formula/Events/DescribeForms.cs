using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Formula.Events
{
    public class DescribeForms : DescribeBase {

        protected List<IMetaBuilder> Builders = new List<IMetaBuilder>();

        public FormDescriptor BuildMeta(object form,object context) {

            var builders = BuildersForType(form.GetType());

            var meta = new FormDescriptor(form, context);
            foreach (var builder in builders) {
                builder.Build(meta);
            }

            return meta;
        }

        public void AddBuilder(IMetaBuilder builder) {
            Builders.Add(builder);
        }

        private Dictionary<Type, IEnumerable<IMetaBuilder>> CachedBuildersForType = new Dictionary<Type, IEnumerable<IMetaBuilder>>();
        private IEnumerable<IMetaBuilder> BuildersForType(Type type) {
            if (!CachedBuildersForType.ContainsKey(type)) {
                CachedBuildersForType[type] = FilterBuildsByType(type);
            }
            return CachedBuildersForType[type];
        }

        private IEnumerable<IMetaBuilder> FilterBuildsByType(Type type) {
            var list = new List<IMetaBuilder>();
            foreach (var builder in Builders) {
                // If the builder is scoped to type, check it's the correct one
                var typed = builder as IMetaBuilderForModelType;
                if (typed != null) {
                    if (typed.ModelType == type) {
                        list.Add(typed);
                    }
                }
                else {
                    // For un-typed meta builders just add it anyway, applies to all models
                    list.Add(builder);
                }
            }
            return list;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Alchemy.Factories;

namespace Downplay.Alchemy.Events {
    public class CompositionBuilder : ICompositionBuilder {

        public CompositionBuilder() {
            _Builders = new List<ICompositionBuilder>();
            _ReadOnlyBuilders = _Builders.AsReadOnly();
        }

        private List<ICompositionBuilder> _Builders;
        private IList<ICompositionBuilder> _ReadOnlyBuilders;
        private bool _Locked = false;
        public IList<ICompositionBuilder> Builders {
            get {
                return _Locked ? _ReadOnlyBuilders : _Builders;
            }
        }

        public virtual void AddBuilder(ICompositionBuilder builder) {
            Builders.Add(builder);
        }

        public virtual void Build(CompositionContext context) {
            _Locked = true;
            foreach (var builder in Builders) {
                builder.Build(context);
            }
        }

        /// <summary>
        /// Enter a named scope
        /// </summary>
        /// <param name="ident"></param>
        /// <param name="mix"></param>
        public void Scope(string ident, Action<ScopedCompositionBuilder> mix) {
            AddBuilder(new IdentifierScopedBuilder(ident, mix));
        }

        /// <summary>
        /// Enter a type-identified scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ident"></param>
        /// <param name="mix"></param>
/*        public void Scope<T>(string ident, Action<CompositionBuilder> mix)
            where T:IScope{

            AddBuilder(new IdentifierScopedBuilder(ident, mix));
        }*/


    }
}

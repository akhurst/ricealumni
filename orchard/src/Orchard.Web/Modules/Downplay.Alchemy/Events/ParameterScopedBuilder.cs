using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Alchemy.Events {
    public class ParameterScopedBuilder<TParam> : ScopedCompositionBuilder {
        protected string _paramName;
        protected Action<ParameterScopedBuilder<TParam>> _handler;
        protected bool _built = false;

        public ParameterScopedBuilder(string paramName, Action<ParameterScopedBuilder<TParam>> handler) 
            // TODO: Following delegate passed to base method isn't needed with a little more rejigging of class hierarchy
            // ^^ altho, we might need ParameterScopedBuilder<TParam> : ScopedCompositionBuilder<ParameterScopedBuilder<TParam>> which is just crazy...
            : base((builder)=>handler((ParameterScopedBuilder<TParam>)builder)) {
            this._paramName = paramName;
            this._handler = handler;
        }

        /// <summary>
        /// Allows access to the paramater value once you are in Execution
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public TParam Get(CompositionContext context) {
            return context.Param<TParam>(_paramName);
        }

        /// <summary>
        /// TODO: Not called right now unless we sort out the class hierarchy per above comment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsInScope(CompositionContext context) {
            return context.HasParam<TParam>(_paramName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using ClaySharp;
using System.Linq.Expressions;
using ClaySharp.Implementation;

namespace Downplay.Alchemy.Dynamic {
    public class Stuff : Clay {

        public Stuff()
            : base(
//                new ClaySharp.Behaviors.NilBehavior(),
                new ClaySharp.Behaviors.NilResultBehavior(),
                new ClaySharp.Behaviors.InterfaceProxyBehavior(),
                new ClaySharp.Behaviors.PropBehavior(),

                new DictionaryBehavior(),
                new StuffArrayBehavior(),
                new StuffBehavior(),
                new MeldBehavior(),
                new JsonSerializationBehavior(),
                new XmlSerializationBehavior(),
                new SupertypeBehavior()
                ) {
        }

        /// <summary>
        /// Create a stuff and meld various objects straightaway
        /// </summary>
        /// <param name="meld"></param>
        public Stuff(object meld) : this() {
            dynamic me = this;
            me.Set(meld);
        }

        public static dynamic FromJson(string json) {
            return ((dynamic)new Stuff()).Json(json);
        }

        public override string ToString() {
            // Prevent @Model.Foo returning fully qualified type instead of value
            return (string)((dynamic)this);
        }

    }
/* TODO: Experiment with following, see if we get better debugger view
    public class StuffMetaObject : ClayMetaObject {

        public StuffMetaObject(object value, Expression expression)
            : base(value, expression) {
        }

        public StuffMetaObject(object value, Expression expression, Func<Expression, Expression> getClayBehavior) : base(value, expression, getClayBehavior) { }

        public override IEnumerable<string> GetDynamicMemberNames() {
            var members = new Dictionary<string,object>();
            (Value as IClayBehaviorProvider).Behavior.GetMembers(()=>null,Value,members);
            return members.Keys;
        }

    }*/
}

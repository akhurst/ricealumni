using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClaySharp;

namespace Downplay.Origami.Tables {

    /// <summary>
    /// Clay behavior for building table rows
    /// 
    /// This could be applied to a "Table" shape or a "RowGroup" shape
    /// 
    /// Each item you add gets wrapped in a tr
    /// </summary>
    public class RowHoldingBehavior : ClayBehavior{

        private readonly Func<dynamic> _rowFactory;
        private readonly IList<dynamic> _rows;
        /// <summary>
        /// Head/Body
        /// </summary>
        public string RowType { get; set; }

        public RowHoldingBehavior(Func<dynamic> rowFactory) {
            _rowFactory = rowFactory;
            _rows = new List<dynamic>();
        }

        public override object InvokeMember(Func<object> proceed, object self, string name, INamedEnumerable<object> args) {

            // Add a header row
            if (name == "Header") {
                var row = _rowFactory();
            }

            // This function adds a row
            if (name == "Row") {
                var row = _rowFactory();
                foreach (var p in args.Positional) {
                    row.Cell(p);
                }
                foreach (var n in args.Named) {
                    row.Cell(n.Key, n.Value);
                }
                ((dynamic)self).Rows.Add(row);
                return row;
            }

            return proceed();
        }

        public override object GetMember(Func<object> proceed, object self, string name) {
            if (name == "Rows") {

                // Return an activator that can provide a list of rows, indexable rows, etc.


            }
            return proceed();
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Origami.Services
{
    public class CombinedModelResult : ModelDriverResult
    {
        private readonly IEnumerable<ModelDriverResult> _results;

        public CombinedModelResult(IEnumerable<ModelDriverResult> results)
        {
            _results = results.Where(x => x != null);
        }
        public override void Apply(ModelShapeContext context)
        {
            foreach (var result in _results) {
                result.Apply(context);
            }
        }

    }
}
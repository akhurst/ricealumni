using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;

namespace Downplay.Origami.Drivers {

    public class PrefixedUpdateModel : IUpdateModel {
        private Orchard.ContentManagement.IUpdateModel _inner;
        private string _prefix;
        public PrefixedUpdateModel(string prefix, Orchard.ContentManagement.IUpdateModel inner) {
            this._inner = inner;
            this._prefix = prefix + ".";
        }

        public bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {
            return _inner.TryUpdateModel(model, _prefix + prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, Orchard.Localization.LocalizedString errorMessage) {
            _inner.AddModelError(_prefix + key, errorMessage);
        }
    }
}

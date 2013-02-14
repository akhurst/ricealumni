using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using System.Xml.Linq;
using Downplay.Alchemy.Dynamic;

namespace Downplay.Alchemy.Services {
    /// <summary>
    /// Describe content in JSON ... potentially useful
    /// </summary>
    public class ClientContentService {

        private readonly IEnumerable<IContentPartDriver> _contentPartDrivers;

        public ClientContentService(
            IEnumerable<IContentPartDriver> contentPartDrivers
            ) {

                _contentPartDrivers = contentPartDrivers;

        }

        public dynamic Describe(IContent content) {

            var x = new XElement("Content");
            var context = new ExportContentContext(content.ContentItem,x);

            foreach (var driver in _contentPartDrivers) {
                driver.Exporting(context);
            }

            var stuff = new Stuff(x);
            return stuff;

            // Well that was easy
            // TODO: There is sensitive data we want to be able to exclude
        }

    }
}

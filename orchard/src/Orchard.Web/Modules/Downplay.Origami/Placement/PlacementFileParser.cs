﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Orchard.Caching;
using Orchard.FileSystems.WebSite;
using Orchard.Environment.Extensions;
using Orchard.DisplayManagement.Descriptors.ShapePlacementStrategy;

namespace Downplay.Origami.Placement {

    /// <summary>
    /// Parses and caches the Placement.info file contents for a given IWebSiteFolder vdir
    /// </summary>
    [OrchardSuppressDependency("Orchard.DisplayManagement.Descriptors.PlacementFileParser")]
    public class PlacementFileParser : IPlacementFileParser
    {
        private readonly ICacheManager _cacheManager;
        private readonly IWebSiteFolder _webSiteFolder;

        public PlacementFileParser(ICacheManager cacheManager, IWebSiteFolder webSiteFolder) {
            _cacheManager = cacheManager;
            _webSiteFolder = webSiteFolder;
        }

        public PlacementFile Parse(string virtualPath) {
            return _cacheManager.Get(virtualPath, context => {
                context.Monitor(_webSiteFolder.WhenPathChanges(virtualPath));
                var placementText = _webSiteFolder.ReadFile(virtualPath);
                return ParseImplementation(virtualPath, placementText);
            });
        }

        private PlacementFile ParseImplementation(string virtualPath, string placementText) {
            if (placementText == null)
                return null;


            var element = XElement.Parse(placementText);
            return new PlacementFile {
                Nodes = Accept(element).ToList()
            };
        }

        private IEnumerable<PlacementNode> Accept(XElement element) {
            switch (element.Name.LocalName) {
                case "Placement":
                    return AcceptMatch(element);
                case "Match":
                    return AcceptMatch(element);
                case "Place":
                    return AcceptPlace(element);
                case "Push":
                    return AcceptPush(element);
            }
            return Enumerable.Empty<PlacementNode>();
        }


        private IEnumerable<PlacementNode> AcceptMatch(XElement element) {
            if (element.HasAttributes == false) {
                // Match with no attributes will collapse child results upward
                // rather than return an unconditional node
                return element.Elements().SelectMany(Accept);
            }

            // return match node that carries back key/value dictionary of condition,
            // and has child rules nested as Nodes
            return new[]{new PlacementMatch{
                Terms = element.Attributes().ToDictionary(attr=>attr.Name.LocalName, attr=>attr.Value),
                Nodes=element.Elements().SelectMany(Accept).ToArray(),
            }};
        }

        private IEnumerable<PlacementShapeLocation> AcceptPlace(XElement element) {
            // return attributes as part locations
            return element.Attributes().Select(attr => new PlacementShapeLocation {
                ShapeType = attr.Name.LocalName,
                Location = attr.Value
            });
        }

        /// <summary>
        /// TODO: This isn't implemented
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private IEnumerable<PlacementNode> AcceptPush(XElement element)
        {
            return element.Attributes().Select(attr => new PlacementShapePush
            {
                ShapeType = attr.Name.LocalName,
                Location = attr.Value
            });
        }
        
    }
}

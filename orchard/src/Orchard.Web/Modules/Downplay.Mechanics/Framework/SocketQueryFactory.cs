using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Downplay.Mechanics.Services;
using Downplay.Mechanics.Models;
using Orchard.Logging;
using Orchard.Core.Contents.Settings;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
namespace Downplay.Mechanics.Framework {
    public class SocketQueryFactory : ILazyFactory<string, SocketQuery> {

        public IOrchardServices Services { get; set; }
        private readonly IMechanicsService _mechanics;
        private readonly SocketEndpoint _socket;
        private readonly IEnumerable<ConnectorPart> _draftedConnectors;

        public SocketQueryFactory(
            IOrchardServices services,
            IMechanicsService mechanics,
            SocketEndpoint left,
            IEnumerable<ConnectorPart> draftedConnectors = null
            ) {

            Services = services;
            _mechanics = mechanics;
            _socket = left;
            _draftedConnectors = draftedConnectors;
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        public bool TryGetValue(string key, out SocketQuery val) {
            var query = new SocketQuery() {
                Left = _socket
            };
            query.ConnectorsLoader(() => new ConnectorCollection(query));
            query.DescriptorLoader(() => _mechanics.DescribeConnector(key));
            query.ConnectorQueryLoader(() => {
                return _mechanics.Connectors(_socket.ContentPart, ConnectorCriteria.Auto, key).ForPart<ConnectorPart>();
            });
            query.ConnectorItemsLoader(() => {
                var list = _draftedConnectors == null ? query.ConnectorQuery.List() : _draftedConnectors.Where(d=>d.ContentItem.ContentType==key);
                // Map right items to connectors' lazy loaders
                // TODO: Dangerous (recursion etc.) but actually safer than it looks
                var dict = list.ToDictionary<ConnectorPart, ConnectorPart, Func<SocketsPart>>(conn => conn, conn =>
                    () => {
                        // Right item 
                        var part2 = query.RightContent.FirstOrDefault(c => c.Id == conn.RightContentItemId);
                        if (part2 == null) {
                            Logger.Debug("Connector {0} has invalid right item Id {1}. The item may have been deleted.", conn.Id, conn.RightContentItemId);
                        }
                        conn.RightContentField.Loader(() => part2);

                        return part2;
                    }
                );
                foreach (var item in list) {
                    item.RightContentField.Loader(dict[item]);
                };
                return list;
            });
            // TODO: Does liberal use of ToArray() here actually clone the array each time?
            //                query.RightConnectorTypeNames = new Lazy<IEnumerable<string>>(() => socketContext.Connector.Settings.ListAllowedContentRight().ToArray());
            // socketContext.RightConnectorTypes = new Lazy<IEnumerable<ContentTypePartDefinition>>(socketContext.Select(...));
            query.RightContentLoader(() => {
                var connectorRightTypes = query.Descriptor.Settings.ListAllowedContentRight();
                return _mechanics.RightItemsFromConnectors(query.ConnectorItems, connectorRightTypes.ToArray(), !_socket.ContentItem.IsPublished() && _socket.ContentItem.VersionRecord!=null && _socket.ContentItem.VersionRecord.Latest);
            });
            val = query;
            return true;
        }
    }
}

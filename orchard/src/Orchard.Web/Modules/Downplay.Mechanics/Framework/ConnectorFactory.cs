using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.Services;

namespace Downplay.Mechanics.Models {
    public class SocketFactory {
        private readonly IMechanicsService _mechanics;
        public SocketFactory(IMechanicsService mechanics) {
            _mechanics = mechanics;
        }

        protected Lazy<IEnumerable<ConnectorDescriptor>> _Allowed;

        protected LazyDictionary<String, SocketQuery> _LazySockets;
        protected SocketQueryFactory _SocketsLoader;

        /// <summary>
        /// Allowed types of connector from here
        /// </summary>
        public IEnumerable<ConnectorDescriptor> Allowed { get { return _Allowed.Value; } }

        public void AllowedLoader(Func<IEnumerable<ConnectorDescriptor>> loader) {
            _Allowed = new Lazy<IEnumerable<ConnectorDescriptor>>(loader);
        }

        public void SocketsLoader(SocketQueryFactory loader) {
            _SocketsLoader = loader;
            _LazySockets = new LazyDictionary<string, SocketQuery>(_SocketsLoader);
        }

        /// <summary>
        /// Function to get a query for a particular connector type. Note you can just call Sockets[name] instead of Sockets.Socket(name).
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public SocketQuery Socket(string socket) {
            return _LazySockets[socket];
        }

        public SocketQuery this[string socket] {
            get { return Socket(socket); }
        }

        public IEnumerable<SocketQuery> AllSockets {
            get {
                foreach (var c in Allowed) {
                    yield return this[c.Name];
                }
            }
        }

        public bool Has(string socketName) {
            return (Allowed.Any(s => s.Name == socketName));
        }

        public void Flush() {
            foreach (var s in _LazySockets.Values) {
                s.Connectors.Flush(_mechanics);
            }
        }
    }
}

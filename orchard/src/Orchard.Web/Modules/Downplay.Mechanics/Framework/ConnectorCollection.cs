using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;
using Downplay.Mechanics.Services;
using System.Collections.Concurrent;

namespace Downplay.Mechanics.Framework {
    public class ConnectorCollection {
        private readonly SocketQuery Socket;

        public ConnectorCollection(SocketQuery socket) {
            Added = new List<IConnector>();
            Removed = new Dictionary<IConnector,bool>();
            Pending = new List<ConnectorCreator>();
            Socket = socket;
            _lazyCount = new Lazy<int>(_Count);
        }

        protected List<IConnector> Added;
        /// <summary>
        /// Pending creation
        /// </summary>
        protected List<ConnectorCreator> Pending;
        protected Dictionary<IConnector,bool> Removed;
        protected IEnumerable<IConnector> Current;

        protected IEnumerable<IConnector> GetOrQueryCurrent() {
            if (Current == null) {
                Current = Socket.ConnectorQuery.List();
            }
            return Current;
        }

        protected Lazy<int> _lazyCount;
        protected int _Count() {
            return Socket.ConnectorQuery.Count() + Added.Count() - Removed.Count();
        }
        public int Count() {
            return _lazyCount.Value;
        }

        public IEnumerable<IConnector> List() {
            return Added.Concat(Socket.ConnectorItems.Cast<IConnector>().Where(c => !Removed.ContainsKey(c)));
        }

        public IEnumerable<IConnector> List(Func<IContentQuery, IContentQuery> transform) {
            return Added.Concat(transform(Socket.ConnectorQuery).List<IConnector>()).Where(c => !Removed.ContainsKey(c));
        }

        public IEnumerable<IConnector> List(
            Func<IContentQuery, IContentQuery> transform,
            Func<IContentQuery, IEnumerable<IContent>> pager) {
                return Added.Concat(pager(transform(Socket.ConnectorQuery)).Select(c => c.As<IConnector>()).Where(c => !Removed.ContainsKey(c)));
        }
        public void Add(IConnector connector) {
            Added.Add(connector);
        }
        public void Remove(IConnector connector, bool ignorePermissions = false) {
            Removed[connector] = ignorePermissions;
        }

        /// <summary>
        /// Remove all connectors matching the predicate. Note: Causes enumeration!
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(Func<IConnector, bool> predicate, bool ignorePermissions = false) {
            foreach (var c in GetOrQueryCurrent().Where(c => predicate.Invoke(c))) {
                Removed[c] = ignorePermissions;
            }
        }

        public void Remove(int rightContentId, bool ignorePermissions = false) {
            Remove(c=>c.RightContentItemId == rightContentId);
        }

        public void Add(int rightId, Action<IConnector> transform = null, bool ignorePermissions = false) {
            Pending.Add(new ConnectorCreator(Socket.Left.ContentItem, rightId, Socket.Descriptor.Name, transform, ignorePermissions));
        }
        public void Add(IEnumerable<IContent> items, Action<IConnector> transform = null, bool ignorePermissions = false) {
            Pending.AddRange(items.Select(item=>new ConnectorCreator(Socket.Left.ContentItem, item, Socket.Descriptor.Name, transform, ignorePermissions)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any() {
            return Pending.Any() || List().Any();
        }

        /// <summary>
        /// Flushes
        /// </summary>
        /// <param name="iContentManager"></param>
        public void Flush(IMechanicsService _mechanics) {
            foreach (var removal in Removed.ToList()) { // Casting to list so we can remove from it during the loop
                _mechanics.DeleteConnector(Socket.Left.ContentItem, removal.Key,removal.Value);
                Removed.Remove(removal.Key);
            }
            foreach (var builder in Pending.ToList()) { // Casting to list so we can remove from it during the loop
                builder.Create(_mechanics);
                Pending.Remove(builder);
            }
            // Reset lazy
            _lazyCount = new Lazy<int>(_Count);
            Current = null;
        }

        /// <summary>
        /// Meta states on a connector; used mainly for recording if a connector is selected with a checkbox
        /// </summary>
        protected ConcurrentDictionary<IConnector, ConcurrentBag<object>> Meta { get; set; }

        /// <summary>
        /// Attaches a meta state object to a connector; only temporary, and scoped to this collection
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="meta"></param>
        public void Attach(IConnector connector, object meta) {
            // TODO: This and State need testing and documenting. Could be really useful.
            var list = Meta.GetOrAdd(connector, (conn) => new ConcurrentBag<object>());
            list.Add(meta);
        }

        /// <summary>
        /// Query the state dictionary
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public IEnumerable<Tuple<IConnector,TState>> State<TState>() {
            return Meta.SelectMany(m=>m.Value.Where(m2 => m2 is TState).Select(m2 => Tuple.Create(m.Key, (TState)(m2))));
        }

    }
}

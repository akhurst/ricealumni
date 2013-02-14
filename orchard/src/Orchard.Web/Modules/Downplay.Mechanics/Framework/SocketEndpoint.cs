using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Framework {

    /// <summary>
    /// TODO: This is instanced in at least two places; can do better
    /// (Actually why do we even need this class?)
    /// </summary>
    public class SocketEndpoint {
        public SocketEndpoint(SocketsPart content) {
            ContentPart = content;
            _Metadata = new Lazy<ContentItemMetadata>(() => ContentItem.ContentManager.GetItemMetadata(ContentItem));
        }

        public SocketEndpoint(SocketsPart socketsPart, string displayType):this(socketsPart) {
            this.DisplayType = displayType;
        }
        public IContent Content { get { return ContentPart; } }
        public ContentItem ContentItem { get { return ContentPart.ContentItem; } }
        public Models.SocketsPart ContentPart { get; protected set; }
        public string ContentType { get { return ContentItem.ContentType; } }

        protected Lazy<ContentItemMetadata> _Metadata;
        public ContentItemMetadata Metadata { get { return _Metadata.Value; } }

        public string DisplayType { get; set; }
    }
}

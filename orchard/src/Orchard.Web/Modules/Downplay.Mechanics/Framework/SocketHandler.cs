using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Logging;

namespace Downplay.Mechanics.Framework
{
    /// <summary>
    /// Abstract implementation of ISocketHandler; looks a lot like ContentHandler
    /// </summary>
    public abstract class SocketHandler : ISocketHandler
    {

        protected virtual void Preparing(SocketDisplayContext context) { }
        protected virtual void Filtering(SocketEventContext context) { }
        protected virtual void Displaying(SocketDisplayContext context) { }
        protected virtual void Editing(SocketDisplayContext context) { }
        protected virtual void Editor(SocketDisplayContext context) { }
        protected virtual void Updating(SocketDisplayContext context) { }
        protected virtual void Updated(SocketDisplayContext context) { }

        void ISocketHandler.Preparing(SocketDisplayContext context)
        {
            Preparing(context);
        }
        
        void ISocketHandler.Filtering(SocketEventContext context)
        {
            Filtering(context);
        }

        void ISocketHandler.Displaying(SocketDisplayContext context)
        {
            Displaying(context);
        }

        void ISocketHandler.Editing(SocketDisplayContext context)
        {
            Editing(context);
        }

        void ISocketHandler.Editor(SocketDisplayContext context)
        {
            Editor(context);
        }

        void ISocketHandler.Updating(SocketDisplayContext context)
        {
            Updating(context);
        }

        void ISocketHandler.Updated(SocketDisplayContext context)
        {
            Updated(context);
        }
    }
}
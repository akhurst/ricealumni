using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace Downplay.Mechanics.Framework
{

    public interface ISocketHandler : IDependency
    {

        /// <summary>
        /// Always gets called prior to setting up a socket, whether it's for display or edit
        /// </summary>
        /// <param name="context"></param>
        void Preparing(SocketDisplayContext context);

        /// <summary>
        /// Like Preparing, this will be called for all operations, just before content gets read from the database. You can add filters to the collection, to control sorting and querying
        /// of the content.
        /// </summary>
        /// <param name="context"></param>
        void Filtering(SocketEventContext context);

        /// <summary>
        /// Called when display operation is taking place
        /// </summary>
        /// <param name="context"></param>
        void Displaying(SocketDisplayContext context);

        /// <summary>
        /// Called prior to either Editor or Update
        /// </summary>
        /// <param name="context"></param>
        void Editing(SocketDisplayContext context);

        /// <summary>
        /// Called to build the initial editor
        /// </summary>
        /// <param name="context"></param>
        void Editor(SocketDisplayContext context);

        /// <summary>
        /// Called prior to Updated if we are performing an update on a POST action
        /// </summary>
        /// <param name="context"></param>
        void Updating(SocketDisplayContext context);

        /// <summary>
        /// Called after successful update
        /// </summary>
        /// <param name="context"></param>
        void Updated(SocketDisplayContext context);

    }
}

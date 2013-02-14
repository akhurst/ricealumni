using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Logging;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Framework
{
    /// <summary>
    /// Base implementation of IConnectorHandler
    /// </summary>
    public abstract class ConnectorHandler : IConnectorHandler
    {
        protected virtual void Displaying(ConnectorDisplayContext model, ModelShapeContext context) { }
        protected virtual void Display(ConnectorDisplayContext model, dynamic shape, ModelShapeContext context) { }
        protected virtual void Creating(ConnectorCreateContext context) { }
        protected virtual void Created(ConnectorCreateContext context) { }
        protected virtual void CreatingInverse(ConnectorCreateContext context) { }
        protected virtual void Deleting(ConnectorDeleteContext context) { }
        protected virtual void Editing(ConnectorDisplayContext model, ModelShapeContext context) { }

        protected virtual void Edit(ConnectorDisplayContext connectorContext, dynamic shape, ModelShapeContext context) { }
        protected virtual void Updating(ConnectorDisplayContext model, ModelShapeContext context) { }
        protected virtual void Updated(ConnectorDisplayContext model, ModelShapeContext context) { }
        protected virtual void UpdatingInverse(ConnectorDisplayContext model, ModelShapeContext context) { }

        void IConnectorHandler.Displaying(ConnectorDisplayContext model, ModelShapeContext context)
        {
            Displaying(model, context);
        }
        void IConnectorHandler.Display(ConnectorDisplayContext model, dynamic shape, ModelShapeContext context) {
            Display(model, shape, context);
        }

        void IConnectorHandler.Creating(ConnectorCreateContext context)
        {
            Creating(context);
        }

        void IConnectorHandler.Created(ConnectorCreateContext context)
        {
            Created(context);
        }

        void IConnectorHandler.CreatingInverse(ConnectorCreateContext context)
        {
            CreatingInverse(context);
        }

        void IConnectorHandler.Deleting(ConnectorDeleteContext context)
        {
            Deleting(context);
        }

        void IConnectorHandler.Editing(ConnectorDisplayContext model, ModelShapeContext context)
        {
            Editing(model, context);
        }

        void IConnectorHandler.Edit(ConnectorDisplayContext connectorContext, dynamic shape, ModelShapeContext context) {
            Edit(connectorContext, shape, context);
        }
        void IConnectorHandler.Updating(ConnectorDisplayContext model, ModelShapeContext context)
        {
            Updating(model, context);
        }
        void IConnectorHandler.UpdatingInverse(ConnectorDisplayContext model, ModelShapeContext context)
        {
            UpdatingInverse(model, context);
        }

        void IConnectorHandler.Updated(ConnectorDisplayContext model, ModelShapeContext context)
        {
            Updated(model,context);
        }


    }
}
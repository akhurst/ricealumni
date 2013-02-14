using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Downplay.Origami.Services;

namespace Downplay.Mechanics.Framework
{
    public interface IConnectorHandler : IDependency
    {
        void Displaying(ConnectorDisplayContext connectorContext, ModelShapeContext context);
        void Display(ConnectorDisplayContext connectorContext, dynamic shape, ModelShapeContext context);

        void Creating(ConnectorCreateContext createContext);
        void Created(ConnectorCreateContext createContext);
        void CreatingInverse(ConnectorCreateContext inverseContext);

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="createContext"></param>
        void Deleting(ConnectorDeleteContext createContext);

        void Editing(ConnectorDisplayContext connectorContext, ModelShapeContext context);
        void Edit(ConnectorDisplayContext connectorContext, dynamic shape, ModelShapeContext context);
        void Updating(ConnectorDisplayContext connectorContext, ModelShapeContext context);
        void UpdatingInverse(ConnectorDisplayContext connectorContext, ModelShapeContext context);
        void Updated(ConnectorDisplayContext connectorContext, ModelShapeContext context);

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Services
{
    public interface IConnectorBuilder
    {
        IEnumerable<IConnector> Create(IMechanicsService mechanics);
    }
}
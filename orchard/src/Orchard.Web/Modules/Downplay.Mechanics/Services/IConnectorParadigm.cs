using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;

namespace Downplay.Mechanics.Services
{
    /// <summary>
    /// Defines certain high-level behavioral factors for a particular connector
    /// TODO: Could be interesting but we already have socket/connector handlers so what else would this do?
    /// </summary>
    public interface IConnectorParadigm : IDependency
    {

        IEnumerable<String> ForContentTypes();
        IEnumerable<Type> ForContentParts();

    }
}
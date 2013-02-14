using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Security;
using Orchard.Environment.Extensions;

namespace Downplay.Quanta.Quotas
{
    /// <summary>
    /// This authorization handler enforces the "only one" content rule
    /// </summary>
    [OrchardFeature("Downplay.Quanta.Quotas")]
    public class SingletonContentAuthorizationEventHandler : IAuthorizationServiceEventHandler
    {
        public void Checking(CheckAccessContext context)
        {
        }

        public void Adjust(CheckAccessContext context)
        {
            if (!context.Granted) {
                // Are we checking for a "PublishOwn" type permission?
                if (context.Permission.Name.StartsWith("PublishOwn_"))
                {
                    // Hmmm...
                }
            }
        }

        public void Complete(CheckAccessContext context)
        {
        }
    }
}
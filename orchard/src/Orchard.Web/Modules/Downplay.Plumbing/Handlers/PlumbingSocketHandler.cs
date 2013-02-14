using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Downplay.Mechanics;
using Downplay.Mechanics.Framework;
using Downplay.Plumbing.Providers;

namespace Downplay.Plumbing.Handlers
{
    public class PlumbingSocketHandler : SocketHandler
    {
        public PlumbingSocketHandler() { }
        protected override void Preparing(SocketDisplayContext context) {

            context.ModelContext.With<DrillFilterData>(df => {
                if (context.Connector.Name == df.DrillType) {
                    context.Paradigms.Add("DrillSummary");
                    if (df.Id.HasValue) {
                        context.ModelContext.Paradigms.Add("DrillDetail");
                    }
                }
                else {
                    context.Paradigms.Add("DrillExclude");
                }
            });

        }
        protected override void Filtering(SocketEventContext context)
        {
            var context2 = context as SocketDisplayContext;
            if (context2 != null) {
                context2.ModelContext.With<DrillFilterData>(df => {
                    if (context.Connector.Name == df.DrillType && df.Id.HasValue) {
                        context.SocketFilters.Add(new DrillRouteFilter(df.Id.Value));
                        context.Connector.DisplayType = "Detail";
                    }
                });
            }

        }

    }
}
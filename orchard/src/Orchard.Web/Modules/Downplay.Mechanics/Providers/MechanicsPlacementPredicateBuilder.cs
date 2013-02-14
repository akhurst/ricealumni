using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Placement;
using Downplay.Mechanics.Framework;
using Downplay.Origami.Services;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;

namespace Downplay.Mechanics.Predicates
{
    public class MechanicsPlacementPredicateBuilder : IPlacementPredicateProvider
    {
        public Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> Predicate(KeyValuePair<string, string> term, Func<Orchard.DisplayManagement.Descriptors.ShapePlacementContext, bool> predicate)
        {
            var expression = term.Value;
            // TODO: Support wildcards as does traditional placement
            if (term.Key == "LeftContentType")
            {
                return ctx =>
                {
                    var model = ctx.Model<SocketDisplayContext>();
                    if (model != null)
                    {
                        return (model.Left.ContentType == expression) && predicate(ctx);
                    }
                    var model2 = ctx.Model<ConnectorDisplayContext>();
                    if (model2 != null)
                    {
                        return (model2.SocketContext.Left.ContentType == expression) && predicate(ctx);
                    }
                    return false;
                };
            }
            if (term.Key == "RightContentType")
            {
                return ctx =>
                {
                    var model = ctx.Model<ConnectorDisplayContext>();
                    if (model != null)
                    {
                        return (model.Right.Content.ContentItem.ContentType == expression) && predicate(ctx);
                    }
                    return false;
                };
            }
            if (term.Key == "ConnectorContentType"
                || term.Key == "Socket") {
                return ctx => {
                    var model = ctx.Model<SocketDisplayContext>();
                    if (model != null) {
                        return (model.Connector.Name == expression) && predicate(ctx);
                    }
                    return false;
                };
            }

            // Scope can be "Content", "Sockets", "Socket", "Connector"
            if (term.Key == "Scope") {
                return ctx => {
                    if (ctx.Model<SocketsModel>() != null)
                        return ("Sockets" == expression) && predicate(ctx);
                    if (ctx.Model<SocketDisplayContext>() != null)
                        return ("Socket" == expression) && predicate(ctx);
                    if (ctx.Model<ConnectorDisplayContext>() != null)
                        return ("Connector" == expression) && predicate(ctx);
                    var c = ctx.Model<IContent>();
                    if (c != null) {
                        if (c.Has<ConnectorPart>()) return ("ConnectorContent" == expression) && predicate(ctx);
                        if (c.Has<SocketsPart>()) return ("RightContent" == expression) && predicate(ctx);
                        return ("Content" == expression) && predicate(ctx);
                    }
                    // TODO: Maybe some models we use aren't covered
                    // TODO: No way to extend for other scopes (e.g. "MediaSource" for MG), could do many things better with some sort of DesribeContext, and perhaps a key/value dict
                    return false;
                };
            }
            // Not parsed
            return predicate;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Mechanics.Models;
using Downplay.Origami.Services;
using Orchard;
using Orchard.Localization;
using Downplay.Mechanics.Framework;
using Orchard.ContentManagement;
namespace Downplay.Mechanics.Drivers
{
    /// <summary>
    /// Builds the connector item display shape while building the total connector model
    /// </summary>
    public class ConnectorModelItemDriver : ModelDriver<ConnectorEventContext>
    {
        private readonly IOrigamiService _origami;

        public ConnectorModelItemDriver(
            IOrigamiService origami
        )
        {
            _origami = origami;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix
        {
	        get { return "ConnectorItem"; }
        }

        protected override ModelDriverResult Build(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context) {
            return Combined(
                ModelShape("Connectors_Connector", () => {
                    var builder = _origami.ContentBuilder(model.ConnectorContent).WithDisplayType(context.DisplayType).WithParent(context).WithMode("Display");
                    builder.Context.CustomContext[typeof(SocketParentContext)] = new SocketParentContext(model) { ModelContext = context };
                    var shape = _origami.ContentShape(model.ConnectorContent.ContentItem,context.DisplayType,prefix:context.Prefix);
                    _origami.Build(builder,shape);
                    return shape;
                }),
                ModelShape("Connectors_Right", () => {
                    var builder = _origami.ContentBuilder(model.Right.Content).WithDisplayType(context.DisplayType).WithParent(context).WithMode("Display");
                    builder.Context.CustomContext[typeof(SocketParentContext)] = new SocketParentContext(model) { ModelContext = context };
                    var shape = _origami.ContentShape(model.Right.Content.ContentItem, context.DisplayType, prefix: context.Prefix);
                    _origami.Build(builder,shape);
                    return shape;
                })
//                ModelShape("Connectors_Left", () => {
  //                  var builder = _origami.ContentBuilder(model.ConnectorContent).WithDisplayType(context.DisplayType).WithParent(context);
    //            })
                );
        }
        protected override void Update(ConnectorEventContext model, dynamic shapeHelper, IUpdateModel updater, ModelShapeContext context) {
        }
        /*
       protected override ModelDriverResult Display(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
       {
           model.ConnectorContent.As<ConnectorPart>().ParentContext
               = new SocketParentContext()
               {
                   Connector = model,
                   ModelContext = context,
                   Parent = model.SocketContext.SocketsContext.ParentContext,
                   Prefix = "", // FullPrefix(context,Prefix),
                   Socket = model.SocketContext,
                   Sockets = model.SocketContext.SocketsContext
               };
           // TODO: Use Origami BuildDisplay so we can pass in parent context instead of hacking it onto ConnectorPart...
           return ModelShape("Content", ()=>{
               return Services.ContentManager.BuildDisplay(model.ConnectorContent, context.DisplayType);
           });
       }

       protected override ModelDriverResult Editor(ConnectorEventContext model, dynamic shapeHelper, ModelShapeContext context)
       {
           model.RightContent.As<SocketsPart>().ParentContext
               = model.ConnectorContent.As<ConnectorPart>().ParentContext
               = new SocketParentContext()
               {
                   Connector = model,
                   ModelContext = context,
                   Parent = model.SocketContext.SocketsContext.ParentContext,
                   Prefix = "", // FullPrefix(context,Prefix),
                   Socket = model.SocketContext,
                   Sockets = model.SocketContext.SocketsContext
               };
           var results = new List<ModelDriverResult>();
           // Build connector preview
     //      results.Add(ModelShape("Content", () => Services.ContentManager.BuildDisplay(model.ConnectorContent, "SummaryAdminConnector")));
           // Build item preview

           results.Add(ModelShape("Content", () => Services.ContentManager.BuildDisplay(model.RightContent, "SummaryAdminConnector")));

           //results.Add(ModelShape("Content", () => Services.ContentManager.BuildEditor(model.RightContent)));
           return Combined(results.ToArray());
       }

       protected override ModelDriverResult Update(ConnectorEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context)
       {
           model.RightContent.As<SocketsPart>().ParentContext
= model.ConnectorContent.As<ConnectorPart>().ParentContext
               = new SocketParentContext()
               {
                   Connector = model,
                   ModelContext = context,
                   Parent = model.SocketContext.SocketsContext.ParentContext,
                   Prefix = "", // FullPrefix(context,Prefix),
                   Socket = model.SocketContext,
                   Sockets = model.SocketContext.SocketsContext
               };
           var results = new List<ModelDriverResult>();
           // Build connector preview
  //         results.Add(ModelShape("Content", () => Services.ContentManager.BuildDisplay(model.ConnectorContent, "SummaryAdminConnector")));

           // Build item preview
           results.Add(ModelShape("Content", () => Services.ContentManager.BuildDisplay(model.RightContent, "SummaryAdminConnector")));
//            results.Add(ModelShape("Content", () => Services.ContentManager.BuildEditor(model.RightContent)));
           return Combined(results.ToArray());
       }
        * */
   }
}
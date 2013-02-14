using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;
using Downplay.Mechanics.ViewModels;
using Downplay.Mechanics.Services;
using Orchard.ContentManagement.MetaData;

namespace Downplay.Mechanics.Drivers.Creators {
    public class SocketAddNewLinksDriver :ModelDriver<SocketEventContext> {

        private readonly IContentDefinitionManager _contentDefinitionManager;
        
        public SocketAddNewLinksDriver(
            IContentDefinitionManager contentDefinitionManager
        ) {
            _contentDefinitionManager = contentDefinitionManager;
        }

        protected override string Prefix {
            get { return ""; }
        }

        protected override ModelDriverResult Build(SocketEventContext model, dynamic shapeHelper, ModelShapeContext context) {
            return ModelShape("Socket_Creators_AddNewLinks", () => {
                var data = model.Connector.Settings.ListAllowedContentRight().Select(c => _contentDefinitionManager.GetTypeDefinition(c)).Select(d => new {
                    RouteValues = new {
                        area = "Contents",
                        controller = "Admin",
                        action = "Create",
                        socket_populate_name = d.Name,
                        socket_populate_id = model.Left.ContentItem.Id
                    },
                    Name = d.DisplayName
                });
                return shapeHelper.Socket_Creators_AddNewLinks(Links: data);
            });
        }

        protected override void Update(SocketEventContext model, dynamic shapeHelper, Orchard.ContentManagement.IUpdateModel updater, ModelShapeContext context) {
        }
    }
}
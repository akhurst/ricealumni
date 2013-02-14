using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Tokens;
using Orchard.Localization;
using Orchard.ContentManagement;
using Downplay.Mechanics.Models;
using Orchard.ContentManagement.Aspects;

namespace Downplay.Mechanics.Providers {
    public class SocketTokens : ITokenProvider {
        private readonly IContentManager _contentManager;
        public SocketTokens(
            IContentManager contentManager
            ) {
                T= NullLocalizer.Instance;
                _contentManager = contentManager;
        }

        public void Describe(DescribeContext context) {

            // Experimentally supporting some tokens for sockets and content
            // TODO: Find more uses for tokens and see how this can all tie in with Projector
            // Example tokens:
            // {Content.Socket:RelatedContent.Last.Title}    -> title of last related content item
            // {Content.Socket:Thumbnail.First.MediaUrl}      -> image url of first thumbnail relation
            // {Content.Socket:Foo.Count}      -> # of connected items
            // {Content.Socket:Thumbnail.First.ImageUrl:640x480}      -> sized image thumbnail (Media Garden)
            // {Content.Socket:PositionalContent.Connectors[3].Url}      -> url of third connector
/*
            context.For("Content")
                .Token("Socket:*", T("Socket:<socket name>"), T("A socket with the corresponding connector type name"), "Socket")
                ;

            context.For("Socket", T("Socket"), T("A collection of connected content"))
                .Token("Count", T("Count"), T("Number of items connected to the socket"))
                .Token("First", T("First"), T("The first content item connected to this socket"))
                .Token("Connectors", T("Connectors"), T("The proxy Connector content items"))
                ;*/

            context.For("Content")
                .Token("Left", T("Left Item"), T("The left-hand connected item from a connector"))
                .Token("LeftPath", T("Left Path"), T("The left-hand item's path and slug from a connector, with an appended forward slash if non-empty"))
                .Token("Right", T("Right Item"), T("The right-hand connected item from a connector"));
        }

        public void Evaluate(EvaluateContext context) {
            context.For<IContent>("Content")
                .Token("Left", content => _contentManager.GetItemMetadata(Left(content)).DisplayText)
                .Chain("Left", "Content", c => Left(c))
                .Token("LeftPath", (c => {
                    var left = Left(c);
                    var aliasPart = left.As<IAliasAspect>();
                    if (aliasPart==null) return null;

                    string alias = aliasPart.Path;
                    if (String.IsNullOrWhiteSpace(alias)) return "";
                    return alias + "/";
                }))
                .Chain("Right", "Content", c => {
                    return Right(c);
                });
        }

        private IContent Left(IContent content) {
            var conn = content.As<ConnectorPart>();
            if (conn == null) return null;
            return conn.LeftContent;
        }
        private IContent Right(IContent content) {
            var conn = content.As<ConnectorPart>();
            if (conn == null) return null;
            return conn.RightContent;
        }

        public Localizer T { get; set; }
    }
}
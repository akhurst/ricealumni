using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Downplay.Mechanics.Settings
{
    /// <summary>
    /// Defines the type of relationship
    /// </summary>
    public class ConnectorTypePartSettings
    {
        public ConnectorTypePartSettings() {
            AllowMany = true;
            AllowNone = true;
        }

        /// <summary>
        /// Whether to allow join to many
        /// </summary>
        public bool AllowMany { get; set; }

        /// <summary>
        /// Whether it is required to select one or more items of the RHS of the relationship
        /// </summary>
        public bool AllowNone { get; set; }
        /// <summary>
        /// Whether to allow the same item joined many times
        /// </summary>
        public bool AllowDuplicates { get; set; }
        /// <summary>
        /// Comma-separated type Ids allowed on the LHS of the relationship (optional for any)
        /// </summary>
        public String AllowedContentLeft { get; set; }

        /// <summary>
        /// Comma-separated type Ids allowed on the RHS of the relationship (optional for any)
        /// </summary>
        public String AllowedContentRight { get; set; }

        public String InverseConnectorType { get; set; }

        /// <summary>
        /// Paradigms to use either in editor or display
        /// </summary>
        public String DefaultParadigms { get; set; }
        /// <summary>
        /// Paradigms to use just in display
        /// </summary>
        public String DefaultDisplayParadigms { get; set; }
        /// <summary>
        /// Paradigms to use just in editor
        /// </summary>
        public String DefaultEditorParadigms { get; set; }

        /// <summary>
        /// Display heading text to be used when rendering a socket for this connector (was DisplayNamePlural)
        /// </summary>
        public String SocketDisplayName { get; set; }
        public String SocketDisplayType { get; set; }
        public String SocketGroupName { get; set; }
        public String ConnectorDisplayType { get; set; }

        public IEnumerable<String> ListAllowedContentLeft()
        {
            return SplitOnCommaOrSpace(AllowedContentLeft);
        }

        public IEnumerable<String> ListAllowedContentRight()
        {
            return SplitOnCommaOrSpace(AllowedContentRight);
        }

        /// <summary>
        /// TODO: Useful extension method to move somewhere else
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> SplitOnCommaOrSpace(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) return Enumerable.Empty<String>();
            return input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string SocketDisplayHint { get; set; }
        public string SocketEditorHint { get; set; }

    }
}
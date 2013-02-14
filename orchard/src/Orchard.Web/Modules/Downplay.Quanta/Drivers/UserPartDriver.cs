using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Users.Models;
using Orchard.ContentManagement.Drivers;

namespace Downplay.Quanta.Drivers
{
    /// <summary>
    /// Generates a display shape for the user part, since there's no driver in Orchard.Users.
    /// http://orchard.codeplex.com/workitem/17883
    /// Required for Quanta permissions connector list
    /// </summary>
    public class UserPartDriver : ContentPartDriver<UserPart>
    {

        protected override DriverResult Display(UserPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_UserName", () => shapeHelper.Parts_UserName(ContentPart:part, UserName: part.UserName));
        }

    }
}
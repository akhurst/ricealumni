using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Downplay.Theory.Identity.ViewModels;
using Downplay.Theory.Identity.Models;

namespace Downplay.Theory.Identity.Drivers
{
    public class PostalCodeSearchFormPartDriver : ContentPartDriver<PostalCodeSearchFormPart> 
    {
        protected override DriverResult Display(PostalCodeSearchFormPart part, string displayType, dynamic shapeHelper)
        {
            var model = new PostalCodeSearchViewModel();
            return ContentShape("Parts_AddressDirectory_PostalCodeSearchForm",
                                () =>
                                {
                                    var shape = shapeHelper.Parts_AddressDirectory_PostalCodeSearchForm();
                                    shape.ContentPart = part;
                                    shape.ViewModel = model;
                                    return shape;
                                });
        }
    }
}
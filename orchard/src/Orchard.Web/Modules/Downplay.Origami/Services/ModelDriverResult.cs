using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;

namespace Downplay.Origami.Services
{
    public class ModelDriverResult
    {
        public virtual void Apply(ModelShapeContext context) { }
    }
}
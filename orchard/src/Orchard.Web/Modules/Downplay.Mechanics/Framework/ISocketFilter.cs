using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Mechanics.Framework
{
    public interface ISocketFilter
    {
        Orchard.ContentManagement.IContentQuery Apply(Orchard.ContentManagement.IContentQuery query);
    }

    /// <summary>
    /// TODO: Where should this live? Origami?
    /// </summary>
    public interface IPager {


        int Size { get; set; }

        int Start { get; set; }


    }


}

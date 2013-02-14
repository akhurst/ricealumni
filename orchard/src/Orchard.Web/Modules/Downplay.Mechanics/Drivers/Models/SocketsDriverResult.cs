using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Downplay.Origami.Services;
using Downplay.Mechanics.Framework;

namespace Downplay.Mechanics.Drivers
{
    public class SocketsDriverResult : ModelShapeResult
    {
        private SocketDisplayContext SocketContext;

        public SocketsDriverResult(string shapeType, string prefix, Framework.SocketDisplayContext socketContext, Func<SocketDisplayContext, ModelShapeContext, dynamic> factory)
            : base(shapeType,prefix,(c)=>factory(socketContext,c))
        {
            SocketContext = socketContext;
        }
    }
}

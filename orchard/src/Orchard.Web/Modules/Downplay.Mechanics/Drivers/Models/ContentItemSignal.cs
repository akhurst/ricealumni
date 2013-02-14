using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Downplay.Mechanics.Drivers
{
    public struct ContentItemSignal
    {
        public int Id;

        public ContentItemSignal(int id)
        {
            Id = id;
        }

        /*public override bool Equals(object obj)
        {
            return (Id == ((ContentItemSignal)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 7 + 3872817;
        }*/
    }
    public struct ContentItemAllSignal
    {
        /*
        public override bool Equals(object obj)
        {
            return true;
        }
*/
    }
}

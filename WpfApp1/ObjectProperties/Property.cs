using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ObjectProperties
{
    public class Property
    {
        public string Name;
        public object Value;
    }

    public enum PhysicsUnit
    {
        None,
        m,
        s,
        mm,
        km
    }
    public class PysicsProperty : Property
    {
        public PhysicsUnit Unit;
    }

    public class CableLengthProperty : PysicsProperty
    {

    }
    
}

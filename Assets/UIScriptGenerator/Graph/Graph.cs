using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIScript
{
    public abstract class Graph
    {
        public virtual string GetGetterStr() { return string.Empty; }

        public virtual string GetSetterStr() { return string.Empty; }
    }

}

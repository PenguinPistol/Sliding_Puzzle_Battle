using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.PlugStudio.Patterns
{
    interface Builder<T> where T : class
    {
        T Build();
    }
}

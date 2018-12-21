using System.Collections;
using UnityEngine;

public abstract class Decorator : object
{
    public abstract bool Evaluate(ref Hashtable data);
}

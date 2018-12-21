using System.Collections;
using UnityEngine;

public class IsAlertState : Decorator
{
    string m_PropertyName;
    AlertStates m_TargetState;

    public IsAlertState(string PropertyName, AlertStates TargetState)
    {
        m_PropertyName = PropertyName;
        m_TargetState = TargetState;
    }

    public override bool Evaluate(ref Hashtable data)
    {
        if(data.ContainsKey(m_PropertyName))
        {
            return (AlertStates)data[m_PropertyName] == m_TargetState;
        }

        return false;
    }
}

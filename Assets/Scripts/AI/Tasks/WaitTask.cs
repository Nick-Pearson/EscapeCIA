using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaitTask : Task
{
    // property name of the delay
    public string DelayPropertyName;

    private float m_CurDelay;
    private float m_TotalDelay;

    public WaitTask(string PropertyName)
    {
        DelayPropertyName = PropertyName;
    }

    public WaitTask(float Delay)
    {
        DelayPropertyName = "";
        m_TotalDelay = Delay;
    }

    protected override void OnTaskStarted(ref Hashtable data)
    {
        if (DelayPropertyName.Length != 0 && data.ContainsKey(DelayPropertyName))
            m_TotalDelay = (float)data[DelayPropertyName];
        
        m_CurDelay = m_TotalDelay;
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        m_CurDelay -= deltaTime;

        if(m_CurDelay < 0.0f)
        {
            MarkCompleted(true);
        }
    }
}

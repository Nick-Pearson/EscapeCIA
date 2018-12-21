using System.Collections;
using UnityEngine;

public abstract class Task
{
    private bool m_Completed;
    public bool Completed
    {
        get { return m_Completed; }
    }

    private bool m_Successful;
    public bool Successful
    {
        get { return m_Successful; }
    }

    public int i;
    
    public void Start(ref Hashtable data)
    {
        m_Successful = false;
        m_Completed = false;

        OnTaskStarted(ref data);
    }

    protected virtual void OnTaskStarted(ref Hashtable data) { }
    public abstract void UpdateTask(float deltaTime, ref Hashtable data);

    protected void MarkCompleted(bool success)
    {
        m_Completed = true;
        m_Successful = success;
    }
}

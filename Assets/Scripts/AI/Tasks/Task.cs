using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    private bool m_Completed = true;
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
    List<Service> services;

    public Task()
    {
        services = new List<Service>();
    }

    public void Start(ref Hashtable data)
    {
        m_Successful = false;
        m_Completed = false;

        for(int i = 0; i < services.Count; ++i)
        {
            services[i].RunService(ref data);
        }

        OnTaskStarted(ref data);
    }

    protected virtual void OnTaskStarted(ref Hashtable data) { }
    public abstract void UpdateTask(float deltaTime, ref Hashtable data);

    protected void MarkCompleted(bool success)
    {
        m_Completed = true;
        m_Successful = success;
    }

    public void AddService(Service service)
    {
        services.Add(service);
    }
}

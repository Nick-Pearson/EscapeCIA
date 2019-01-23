using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavigateTo : Task
{
    NavMeshAgent m_Agent;
    Transform m_Transform;

    string m_TargetPropertyName;
    Vector3 m_Target;

    public NavigateTo(string TargetName)
    {
        m_TargetPropertyName = TargetName;
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        Vector3 toWaypoint = m_Transform.position - m_Target;
        toWaypoint.y = 0.0f;

        float distanceToWaypoint = toWaypoint.sqrMagnitude;
        if(distanceToWaypoint < 0.2f)
        {
            MarkCompleted(true);
        }
    }

    protected override void OnTaskStarted(ref Hashtable data)
    {
        if(m_Agent == null || m_Transform == null)
        {
            GameObject Owner = (GameObject)data["owner"];
            if (Owner == null)
            {
                Debug.LogError("Owner not set on behavior tree");
                MarkCompleted(false);
                return;
            }

            m_Agent = Owner.GetComponent<NavMeshAgent>();
            m_Transform = Owner.transform;
        }
        
        if(!data.ContainsKey(m_TargetPropertyName))
        {
            Debug.LogWarning("Target not set on NavigateTo");
            MarkCompleted(false);
            return;
        }

        m_Target = (Vector3)data[m_TargetPropertyName];
        m_Agent.SetDestination(m_Target);
    }
}

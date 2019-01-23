using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RotateTowards : Task
{
    Transform m_Transform;
    Quaternion m_From;
    Quaternion m_Target;
    float m_AngularSpeed = 10.0f; // speed nicked off the nav mesh agent to make it consistent
    float lerpValue;
    bool m_XZPlane;

    string m_TargetPropertyName;

    public RotateTowards(string inTargetPropertyName, bool inXZPlane = true)
    {
        m_TargetPropertyName = inTargetPropertyName;
        m_XZPlane = inXZPlane;
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        m_Transform.rotation = Quaternion.Lerp(m_From, m_Target, lerpValue);
        
        lerpValue += (m_AngularSpeed * deltaTime) / Quaternion.Angle(m_Target, m_From);

        if (lerpValue >= 1.0f)
        {
            MarkCompleted(true);
        }
    }

    protected override void OnTaskStarted(ref Hashtable data)
    {
        if(m_Transform == null)
        {
            GameObject Owner = (GameObject)data["owner"];
            if (Owner == null)
            {
                Debug.LogError("Owner not set on behavior tree");
                MarkCompleted(false);
                return;
            }
            
            m_Transform = Owner.transform;
            NavMeshAgent Agent = Owner.GetComponent<NavMeshAgent>();
            if (Agent != null)
            {
                m_AngularSpeed = Agent.angularSpeed;
            }
        }

        lerpValue = 0.0f;

        if (!data.ContainsKey(m_TargetPropertyName))
        {
            Debug.LogWarning("Target not set on RotateTowards");
            MarkCompleted(false);
            return;
        }
        
        m_From = m_Transform.rotation;
        Vector3 Direction = (Vector3)data[m_TargetPropertyName] - m_Transform.position;
        if (m_XZPlane)
        {
            Direction.y = 0.0f;
        }

        m_Target = Quaternion.LookRotation(Direction);

        if(Quaternion.Angle(m_Target, m_From) < 1.0f)
        {
            MarkCompleted(true);
        }
    }
    
}

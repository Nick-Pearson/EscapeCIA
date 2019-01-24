using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveIntoRange : Task
{
    string m_TargetPropertyName;
    string m_RangePropertyName;

    NavMeshAgent m_Agent;
    Transform m_Transform;

    Vector3 Target;
    float TargetDist;

    public MoveIntoRange(string inTargetPropertyName, string inRangePropertyName)
    {
        m_TargetPropertyName = inTargetPropertyName;
        m_RangePropertyName = inRangePropertyName;
    }

    protected override void OnTaskStarted(ref Hashtable data)
    {
        if (m_Agent == null || m_Transform == null)
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

        if (!data.ContainsKey(m_TargetPropertyName))
        {
            Debug.LogWarning("Target not set on MoveIntoRange");
            MarkCompleted(false);
            return;
        }
        else if (!data.ContainsKey(m_RangePropertyName))
        {
            Debug.LogWarning("Range not set on MoveIntoRange");
            MarkCompleted(false);
            return;
        }

        TargetDist = (float)data[m_RangePropertyName] * 0.8f;

        Vector3 toTarget = m_Transform.position - Target;
        toTarget.y = 0.0f;

        float distanceToTarget = toTarget.sqrMagnitude;

        if (distanceToTarget <= TargetDist*TargetDist)
        {
            MarkCompleted(true);
        }
        else
        {
            CalcMoveTarget(ref data);
        }

    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        Vector3 toTarget = m_Transform.position - Target;
        toTarget.y = 0.0f;

        float distanceToTarget = toTarget.sqrMagnitude;

        if(distanceToTarget <= TargetDist*TargetDist)
        {
            MarkCompleted(true);
        }
        else
        {
            CalcMoveTarget(ref data);
        }
    }

    void CalcMoveTarget(ref Hashtable data)
    {
        Vector3 TrueTarget = (Vector3)data[m_TargetPropertyName];
        Vector3 TargetToUs = m_Transform.position - TrueTarget;

        NavMeshHit Hit;
        if (NavMesh.SamplePosition(TrueTarget + (TargetToUs * (TargetDist / TargetToUs.magnitude)), out Hit, 5.0f, 1))
        {
            Target = Hit.position;
        }
        else
        {
            Target = TrueTarget;
        }

        m_Agent.SetDestination(Target);
        m_Agent.isStopped = false;
    }

    protected override void OnTaskAborted()
    {
        m_Agent.isStopped = true;
        base.OnTaskAborted();
    }
}

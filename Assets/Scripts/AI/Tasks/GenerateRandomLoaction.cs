using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenerateRandomLoaction : Service
{
    string m_OutLoactionPropertyName;
    string m_CenterPropertyName;
    float m_Radius;

    public GenerateRandomLoaction(string inOutLocationPropertyName, string inCenterPropertyName, float inRadius)
    {
        m_OutLoactionPropertyName = inOutLocationPropertyName;
        m_CenterPropertyName = inCenterPropertyName;
        m_Radius = inRadius;
    }

    public override void RunService(ref Hashtable data)
    {
        if (!data.ContainsKey(m_CenterPropertyName))
        {
            Debug.LogWarning("Center not set on GenerateRandomLoaction");
            return;
        }

        Vector3 RandomPosition = (Random.insideUnitSphere * m_Radius) + (Vector3)data[m_CenterPropertyName];
        NavMeshHit Hit;
        NavMesh.SamplePosition(RandomPosition, out Hit, m_Radius, 1);
        data[m_OutLoactionPropertyName] = Hit.position;
    }
}

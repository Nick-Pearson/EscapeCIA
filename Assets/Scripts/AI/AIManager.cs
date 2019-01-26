using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// responsible for storing stimuli (noises)
public class AIManager : MonoBehaviour
{
    private List<AIController> AIInstances;

    private void Awake()
    {
        AIInstances = new List<AIController>();
    }

    // notify AI that a noise occurred, loudness equals the distance the noise can be heard from
    public void ReportNoiseEvent(Vector3 position, float loudness)
    {
        // notify all AI that are nearby
        float loudnessSqrd = loudness * loudness;
        if (loudnessSqrd < 0.1f) return;

        foreach (AIController Instance in AIInstances)
        {
            float distSqrd = (Instance.transform.position - position).sqrMagnitude;
            if (distSqrd > loudnessSqrd) continue;

            Instance.ReportNewStimuli(position, loudness + 1.0f);
        }
    }

    public void RegisterAIListener(AIController Instance)
    {
        AIInstances.Add(Instance);
    }

    public void RemoveAIListener(AIController Instance)
    {
        AIInstances.Remove(Instance);
    }
}

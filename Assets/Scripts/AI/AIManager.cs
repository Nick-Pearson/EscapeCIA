using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StimuliType
{
    Sound
}

public struct Stimuli
{
    public Vector3 position;
    public float range;
    public float life;
    public StimuliType type;
}

// responsible for storing stimuli (mainly noises)
public class AIManager : MonoBehaviour
{
    List<Stimuli> m_Stimuli;

    public delegate void StimuliChanged();
    public event StimuliChanged OnStimuliChanged;
    
    public void Awake()
    {
        m_Stimuli = new List<Stimuli>();
    }

    public void ReportNoiseEvent(Vector3 position, float loudness)
    {
        Stimuli newStimuli;
        newStimuli.type = StimuliType.Sound;
        newStimuli.position = position;
        newStimuli.life = 5.0f;
        newStimuli.range = loudness;

        m_Stimuli.Add(newStimuli);
        OnStimuliChanged.Invoke();
    }

    public List<Stimuli> GetAllStimulusFor(Vector3 position)
    {
        List<Stimuli> outStimuli = new List<Stimuli>();

        for(int i = 0; i < m_Stimuli.Count; ++i)
        {
            float Distance = (m_Stimuli[i].position - position).sqrMagnitude;

            if (Distance < m_Stimuli[i].range)
            {
                outStimuli.Add(m_Stimuli[i]);
            }
        }

        return outStimuli;
    }
}

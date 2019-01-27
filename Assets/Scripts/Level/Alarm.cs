using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour {

    public float AlarmLoudness = 20.0f;
    public float AlarmDuration = 5.0f;

    AudioSource m_AudioSource;
    AIManager m_Manager;

    public GameObject[] Lights;

    float m_EndTime;

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Manager = FindObjectOfType<AIManager>();
    }

    void Update()
    {
        if(m_AudioSource.isPlaying)
        {
            foreach (GameObject go in Lights)
            {
                go.SetActive((int)Time.time % 2 == 0);
            }

            if (m_EndTime < Time.time)
                Disable();
        }
    }

    public void Activate()
    {
        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.Play();
        }

        m_Manager.ReportNoiseEvent(transform.position, AlarmLoudness);
        m_EndTime = Time.time + AlarmDuration;
    }

    public void Disable()
    {
        m_AudioSource.Stop();

        foreach(GameObject go in Lights)
        {
            go.SetActive(false);
        }
    }
}

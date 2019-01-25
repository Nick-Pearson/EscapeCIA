using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(AudioSource))]
public class BoomBox : MonoBehaviour {

    public AudioClip MusicTrack;
    public float Loudness = 10.0f;

    public ParticleSystem Particles;

    Interactable m_Interactable;
    AudioSource m_AudioSource;
    AIManager m_AIManager;

    bool m_Playing = false;
    float m_PlayEndTime;

	// Use this for initialization
	void Start ()
    {
        m_Interactable = GetComponent<Interactable>();
        m_Interactable.OnInteract += Play;

        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = MusicTrack;

        m_AIManager = FindObjectOfType<AIManager>();
    }
	
    void Update()
    {
        if(m_Playing && m_PlayEndTime < Time.time)
        {
            m_Playing = false;
            m_Interactable.SetCanInteract(true);

            if (Particles)
                Particles.Stop();
        }
    }

	// Update is called once per frame
	void Play()
    {
        if (m_Playing) return;

        m_Interactable.SetCanInteract(false);
        m_Playing = true;
        m_PlayEndTime = Time.time + MusicTrack.length;

        m_AIManager.ReportNoiseEvent(transform.position, Loudness);
        m_AudioSource.Play();

        if (Particles)
            Particles.Play();
    }
}

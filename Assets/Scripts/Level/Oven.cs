using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Oven : MonoBehaviour {
    public Alarm SmokeAlarm;

    public ParticleSystem SmokeParticles;

    public float Duration = 10.0f;
    public float Wait = 4.0f;

    Interactable m_Interactable;

    void Awake()
    {
        m_Interactable = GetComponent<Interactable>();
        m_Interactable.OnInteract += () => StartCoroutine(OvenSequence());
    }

    IEnumerator OvenSequence()
    {
        m_Interactable.SetCanInteract(false);
        if (SmokeParticles) SmokeParticles.Play();
        
        yield return new WaitForSeconds(Wait);

        if (SmokeAlarm) SmokeAlarm.Activate();

        yield return new WaitForSeconds(Duration);

        if (SmokeParticles) SmokeParticles.Stop();
        m_Interactable.SetCanInteract(true);
    }




}

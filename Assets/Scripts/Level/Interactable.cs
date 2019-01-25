using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void Interact();
    public event Interact OnInteract;

    public string promptText;
    
    bool m_CanInteract = true;
    PlayerController m_Interactor = null;

    void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (!pc) return;

        m_Interactor = pc;

        if(m_CanInteract)
            pc.RegisterInteractable(this);

    }

    void OnTriggerExit(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (!pc) return;

        m_Interactor = null;

        if (m_CanInteract)
            pc.DeregisterInteractable(this);
    }

    void OnDisable()
    {
        if (m_Interactor)
            m_Interactor.DeregisterInteractable(this);
    }

    public void SetCanInteract(bool Value)
    {
        if (Value == m_CanInteract) return;

        m_CanInteract = Value;

        if(m_Interactor)
        {
            if (Value)
                m_Interactor.RegisterInteractable(this);
            else
                m_Interactor.DeregisterInteractable(this);
        }
    }

    public void DoInteract()
    {
        if(OnInteract != null)
            OnInteract.Invoke();
    }
}

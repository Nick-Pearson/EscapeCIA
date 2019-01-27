using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float TimeUntilClose = 3.0f;
    public Transform ChildDoor;
    public bool InvertDirection = false;

    public AudioClip DoorOpenSound;
    public AudioClip DoorCloseSound;

    public Interactable LockedBy;

    AudioSource m_AudioSource;

    int m_CharactersInDoor = 0;
    float m_CloseTime;
    bool m_Open = false;
    bool m_Locked = false;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

        if(LockedBy)
        {
            m_Locked = true;
            LockedBy.OnInteract += UnlockDoor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ControllerBase controller = other.GetComponent<ControllerBase>();
        if (!controller) return;

        m_CharactersInDoor++;
        if (!m_Open)
        {
            OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        ControllerBase controller = other.GetComponent<ControllerBase>();
        if (!controller) return;

        m_CharactersInDoor--;

        if(m_CharactersInDoor == 0)
        {
            m_CloseTime = Time.time + TimeUntilClose;
        }
    }

    void Update()
    {
        if(m_Open && m_CharactersInDoor == 0 && m_CloseTime < Time.time)
        {
            CloseDoor();
        }
    }

    void UnlockDoor()
    {
        m_Locked = false;
        LockedBy.OnInteract -= UnlockDoor;
        LockedBy.SetCanInteract(false);
    }

    void OpenDoor()
    {
        if (m_Open || m_Locked) return;
        m_Open = true;
        ChildDoor.Rotate(new Vector3(0.0f, 0.0f, (InvertDirection ? -1 : 1) * 90.0f));

        if(m_AudioSource && DoorOpenSound)
        {
            m_AudioSource.PlayOneShot(DoorOpenSound);
        }
    }

    void CloseDoor()
    {
        if (!m_Open || m_Locked) return;
        m_Open = false;
        ChildDoor.Rotate(new Vector3(0.0f, 0.0f, (InvertDirection ? 1 : -1) * 90.0f));

        if (m_AudioSource && DoorCloseSound)
        {
            m_AudioSource.PlayOneShot(DoorCloseSound);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed = 1.0f;

    Quaternion m_StartRot;
    float m_StartTime;

    private void Awake()
    {
        m_StartTime = Time.time;
        m_StartRot = transform.rotation;
    }

    void Update ()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.Slerp(m_StartRot, Quaternion.identity, Time.time - m_StartTime);
	}
}

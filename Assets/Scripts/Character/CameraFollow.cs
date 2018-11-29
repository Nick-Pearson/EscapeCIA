using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The Camera Target
    [SerializeField]
    Transform m_PlayerTransform;

    [SerializeField]
    float m_CameraHeight = 10.0f;

    // Use this for initialization
    void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
  {
    float cameraAngle = 90.0f - transform.rotation.eulerAngles.x;
    float deltaZ = m_CameraHeight * Mathf.Tan(cameraAngle * Mathf.Deg2Rad);
    transform.position = new Vector3(m_PlayerTransform.position.x, m_PlayerTransform.position.y + m_CameraHeight, m_PlayerTransform.position.z - deltaZ);
  }
}

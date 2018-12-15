using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpinner : MonoBehaviour {

  [SerializeField]
  float spinSpeed = 1.0f;

  [SerializeField]
  Vector3 spinAxis = new Vector3(0.0f, 1.0f, 0.0f);

	// Update is called once per frame
	void Update ()
  {
    transform.Rotate(spinAxis * spinSpeed * Time.deltaTime);

	}
}

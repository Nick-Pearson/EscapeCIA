using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAudio : MonoBehaviour {
  [SerializeField]
  float FadeTime = 1.0f;

  [SerializeField]
  float TargetVolume = 1.0f;

  private AudioSource target;

  private float curTime = 0.0f;

  void Start()
  {
    target = GetComponent<AudioSource>();
    target.volume = 0.0f;
  }

	// Update is called once per frame
	void Update ()
  {
    curTime += Time.deltaTime;

    target.volume = Mathf.Clamp(TargetVolume * curTime / FadeTime, 0.0f, TargetVolume);

    if(curTime > FadeTime)
    {
      Destroy(this);
    }
	}
}

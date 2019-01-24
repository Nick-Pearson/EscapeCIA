using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSHealthBar : HealthBar
{
  // how long until the health bar disappears if no updates are sent to it
  [SerializeField]
  float lifetime = 5.0f;

  [SerializeField]
  float fadeOutDuration = 1.0f;
    
  private float timeUntilFadeOut;

  // stored as a value so that the UI remains at the correct location if the target is destroyed
  private Vector3 targetPosition;
    

	// Use this for initialization
	public override void Initialise (Health inHealthComp, UIManager inUIManager)
    {
        base.Initialise(inHealthComp, inUIManager);
        timeUntilFadeOut = lifetime;

	}

	public override void UpdateHealthDisplay()
    {
        base.UpdateHealthDisplay();
        timeUntilFadeOut = lifetime;
	}

    void Update()
    {
        timeUntilFadeOut -= Time.deltaTime;
        if(timeUntilFadeOut < 0.0f)
        {
          m_Target.UIListener = null;
          m_UIManager.ReturnHealthBarToPool(this);
        }
        else if(timeUntilFadeOut < fadeOutDuration)
        {
          SetSpriteAlpha(timeUntilFadeOut / fadeOutDuration);
        }

        if(m_Target)
        {
          targetPosition = m_Target.transform.position;
        }

        // project this UI onto the screen
        Vector3 screenPos = m_Camera.WorldToScreenPoint(targetPosition + new Vector3(0.0f, 2.0f, 0.0f));
        m_RectTransform.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
    }
}

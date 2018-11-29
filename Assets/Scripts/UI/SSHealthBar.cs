using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSHealthBar : MonoBehaviour
{
  [SerializeField]
  Sprite HeartIconFull;

  [SerializeField]
  Sprite HeartIconHalf;

  [SerializeField]
  Sprite HeartIconEmpty;

  [SerializeField]
  Image IconPrefab;

  // how long until the health bar disappears if no updates are sent to it
  [SerializeField]
  float lifetime = 5.0f;

  [SerializeField]
  float fadeOutDuration = 1.0f;


  // --------------------------------------------------------------

  private Health m_Target;

  private UIManager m_UIManager;

  private List<Image> m_Sprites = new List<Image>();

  // cached component ref
  private RectTransform m_RectTransform;

  private Camera m_Camera;

  private float m_IconWidth;

  private float timeUntilFadeOut;

  // stored as a value so that the UI remains at the correct location if the target is destroyed
  private Vector3 targetPosition;

  // --------------------------------------------------------------

  void Awake()
  {
    m_RectTransform = GetComponent<RectTransform>();
    m_IconWidth = IconPrefab.GetComponent<RectTransform>().sizeDelta.x;
    m_Camera = FindObjectOfType<Camera>();
  }

	// Use this for initialization
	public void Initialise (Health inHealthComp, UIManager inUIManager)
  {
    m_Target = inHealthComp;
    m_UIManager = inUIManager;
    timeUntilFadeOut = lifetime;

    if(m_Target == null)
    {
      return;
    }

    m_Target.UIListener = gameObject;

    UpdateSprites();
    UpdateHealthDisplay();

    m_Target.OnHealthChanged += new Health.HealthChanged(this.UpdateHealthDisplay);
	}


	public void UpdateHealthDisplay()
  {
    for(int i = 0; i < m_Sprites.Count; i++)
    {
      Sprite newSprite;

      int qryHealth = (i * 2) + 1;

      if(m_Target.CurrentHealth < qryHealth)
      {
        newSprite = HeartIconEmpty;
      }
      else if(m_Target.CurrentHealth - qryHealth == 0)
      {
        newSprite = HeartIconHalf;
      }
      else
      {
        newSprite = HeartIconFull;
      }

      m_Sprites[i].sprite = newSprite;
    }

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

  // calculates the number of sprites required and spawns/deletes them
  private void UpdateSprites()
  {
      int newNumSprites = Mathf.CeilToInt(m_Target.MaxHealth / 2.0f);

      if(newNumSprites > m_Sprites.Count)
      {
        int numToAdd = newNumSprites - m_Sprites.Count;

        for(int i = 0; i < numToAdd; i++)
        {
          int newIndex = m_Sprites.Count;

          Image newSprite = Instantiate(IconPrefab, transform.position, transform.rotation);
          newSprite.transform.SetParent(transform);

          RectTransform newRectTransform = newSprite.GetComponent<RectTransform>();

          Vector2 tmpTransform = newRectTransform.anchoredPosition;

          tmpTransform.x = (newIndex * m_IconWidth) + (m_IconWidth / 2.0f);
          newRectTransform.anchoredPosition = tmpTransform;

          m_Sprites.Add(newSprite);
        }
      }
      else if(newNumSprites < m_Sprites.Count)
      {
        int numToRemove = m_Sprites.Count - newNumSprites;

        for(int i = m_Sprites.Count-1; i >= m_Sprites.Count - numToRemove; i--)
        {
          Destroy(m_Sprites[i].gameObject);
          m_Sprites.RemoveRange(i, 1);
        }
      }

      m_RectTransform.sizeDelta = new Vector2(m_IconWidth * m_Sprites.Count, m_RectTransform.sizeDelta.y);

      SetSpriteAlpha(1.0f);
  }

  private void SetSpriteAlpha(float alpha)
  {
    for(int i = 0; i < m_Sprites.Count; ++i)
    {
      Color curColor = m_Sprites[i].color;
      curColor.a = alpha;

      m_Sprites[i].color = curColor;
    }
  }
}

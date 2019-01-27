using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Sprite HeartIconFull;

    [SerializeField]
    Sprite HeartIconHalf;

    [SerializeField]
    Sprite HeartIconEmpty;

    [SerializeField]
    Image IconPrefab;

    public float ScaleFactor = 1.0f;


    // --------------------------------------------------------------

    protected Health m_Target;

    protected UIManager m_UIManager;

    private List<Image> m_Sprites;
    
    // cached component ref
    protected RectTransform m_RectTransform;

    protected Camera m_Camera;

    private float m_IconWidth;

    // --------------------------------------------------------------
    
    // Use this for initialization
    public virtual void Initialise (Health inHealthComp, UIManager inUIManager)
    {
        m_Target = inHealthComp;
        m_UIManager = inUIManager;

        if (m_Target == null)
        {
            return;
        }

        m_Target.UIListener = gameObject;

        m_RectTransform = GetComponent<RectTransform>();
        m_IconWidth = IconPrefab.GetComponent<RectTransform>().sizeDelta.x;
        m_Camera = FindObjectOfType<Camera>();
        m_Sprites = new List<Image>();

        UpdateSprites();
        UpdateHealthDisplay();

        m_Target.OnHealthChanged += ((change) => UpdateHealthDisplay());
    }


    public virtual void UpdateHealthDisplay()
    {
        for (int i = 0; i < m_Sprites.Count; i++)
        {
            Sprite newSprite;

            int qryHealth = (i * 2) + 1;

            if (m_Target.CurrentHealth < qryHealth)
            {
                newSprite = HeartIconEmpty;
            }
            else if (m_Target.CurrentHealth - qryHealth == 0)
            {
                newSprite = HeartIconHalf;
            }
            else
            {
                newSprite = HeartIconFull;
            }

            m_Sprites[i].sprite = newSprite;
        }
    }

    // calculates the number of sprites required and spawns/deletes them
    private void UpdateSprites()
    {
        int newNumSprites = Mathf.CeilToInt(m_Target.MaxHealth / 2.0f);

        if(newNumSprites > m_Sprites.Count)
        {
            int numToAdd = newNumSprites - m_Sprites.Count;

            for (int i = 0; i < numToAdd; i++)
            {
                int newIndex = m_Sprites.Count;

                Image newSprite = Instantiate(IconPrefab, transform.position, transform.rotation);
                newSprite.transform.SetParent(transform);

                RectTransform newRectTransform = newSprite.GetComponent<RectTransform>();

                Vector2 tmpTransform = newRectTransform.anchoredPosition;

                tmpTransform.x = (newIndex * m_IconWidth * ScaleFactor) + (m_IconWidth / 2.0f);
                newRectTransform.anchoredPosition = tmpTransform;
                newRectTransform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);

                m_Sprites.Add(newSprite);
            }
        }
        else if(newNumSprites < m_Sprites.Count)
        {
            int numToRemove = m_Sprites.Count - newNumSprites;
            int endIdx = m_Sprites.Count - numToRemove;

            for (int i = m_Sprites.Count - 1; i >= endIdx; i--)
            {
                Destroy(m_Sprites[i].gameObject);
                m_Sprites.RemoveRange(i, 1);
            }
        }

        m_RectTransform.sizeDelta = new Vector2(ScaleFactor * m_IconWidth * m_Sprites.Count, m_RectTransform.sizeDelta.y);

        SetSpriteAlpha(1.0f);
    }

    protected void SetSpriteAlpha(float alpha)
    {
        for (int i = 0; i < m_Sprites.Count; ++i)
        {
            Color curColor = m_Sprites[i].color;
            curColor.a = alpha;

            m_Sprites[i].color = curColor;
        }
    }
}

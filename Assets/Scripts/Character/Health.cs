using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // The total health of this unit, 2 health = 1 full heart
    public int MaxHealth = 8;

    int m_CurHealth = 100;

    public int CurrentHealth
    {
        get { return m_CurHealth; }
    }

    public delegate void HealthChanged(int change);
    public event HealthChanged OnHealthChanged;

    public delegate void Died();
    public event Died OnDied;

    [HideInInspector]
    public GameObject UIListener;

    public void Awake()
    {
      m_CurHealth = MaxHealth;
    }

    public void ModifyHealth(int change)
    {
        if (change == 0) return;

        m_CurHealth += change;
        m_CurHealth = Mathf.Clamp(m_CurHealth, 0, MaxHealth);

        if(OnHealthChanged != null)
          OnHealthChanged(change);

        if(UIListener == null)
        {
          FindObjectOfType<UIManager>().SpawnHealthBar(this);
        }

        if(m_CurHealth <= 0)
        {
          if(OnDied != null)
            OnDied();

          Destroy(gameObject);
        }
    }

    public bool IsAlive()
    {
        return m_CurHealth > 0;
    }
}

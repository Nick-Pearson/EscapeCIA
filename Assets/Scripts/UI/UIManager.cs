using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // --------------------------------------------------------------

    [SerializeField]
    Text m_BulletText;

    [SerializeField]
    Text m_GrenadeText;

    [SerializeField]
    SSHealthBar HealthBarPrefab;

    private List<SSHealthBar> HealthBarPool = new List<SSHealthBar>();

    // --------------------------------------------------------------

    public void SetAmmoText(int bulletCount, int grenadeCount)
    {
        if(m_BulletText)
        {
            m_BulletText.text = "Bullets: " + bulletCount;
        }

        if(m_GrenadeText)
        {
            m_GrenadeText.text = "Grenades: " + grenadeCount;
        }
    }


    // --------------------------------------------------------------

    public void SpawnHealthBar(Health target)
    {
      SSHealthBar newHealthBar = null;

      if(HealthBarPool.Count != 0)
      {
        newHealthBar = HealthBarPool[HealthBarPool.Count - 1];
        newHealthBar.gameObject.SetActive(true);

        HealthBarPool.RemoveAt(HealthBarPool.Count - 1);
      }
      else
      {
        newHealthBar = Instantiate(HealthBarPrefab, Vector3.zero, Quaternion.identity);
        newHealthBar.transform.SetParent(transform);
      }

      newHealthBar.Initialise(target, this);
    }

    public void ReturnHealthBarToPool(SSHealthBar healthBar)
    {
      HealthBarPool.Add(healthBar);
      healthBar.gameObject.SetActive(false);
    }
}

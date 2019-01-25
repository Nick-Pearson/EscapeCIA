using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // --------------------------------------------------------------
    
    [SerializeField]
    Text m_WeaponText;

    [SerializeField]
    Text m_InteractableText;
    [SerializeField]
    GameObject m_InteractableGroup;

    [SerializeField]
    SSHealthBar HealthBarPrefab;

    [SerializeField]
    HealthBar PlayerHealthBar;

    GunLogic m_CurrentWeapon;

    private List<SSHealthBar> HealthBarPool = new List<SSHealthBar>();

    private void Awake()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Health PlayerHealth = Player.GetComponent<Health>();

        if(PlayerHealth)
        {
            PlayerHealthBar.Initialise(PlayerHealth, this);
        }
        else
        {
            Destroy(PlayerHealthBar);
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
    
    // --------------------------------------------------------------

    public void SetCurrentWeapon(GunLogic newWeapon)
    {
        m_CurrentWeapon = newWeapon;
        UpdateWeaponText();

        if (newWeapon != null)
        {
            newWeapon.OnAmmoChanged += UpdateWeaponText;
        }
    }

    void UpdateWeaponText()
    {
        if (m_CurrentWeapon == null)
        {
            m_WeaponText.text = "Unarmed";
        }
        else
        {
            m_WeaponText.text = string.Format("{0}  {1} / {2}", m_CurrentWeapon.DisplayName, m_CurrentWeapon.CurrentAmmo, m_CurrentWeapon.AmmoPerClip);
        }
    }

    // --------------------------------------------------------------

    public void SetBestInteractable(Interactable interactable)
    {
        m_InteractableGroup.SetActive(interactable != null);

        if (interactable)
        {
            m_InteractableText.text = "[E]" + interactable.promptText;
        }
    }
}

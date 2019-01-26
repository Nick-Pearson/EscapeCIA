﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    Text m_TutorialText;

    [SerializeField]
    GameObject m_TutorialGroup;

    [SerializeField]
    GameObject m_DieScreen;

    public AudioClip TutorialSound;

    // how long a tutorial message remains on screen
    public float TutorialDuration = 10.0f;
    float m_TutorialEndTime;

    [SerializeField]
    SSHealthBar HealthBarPrefab;

    [SerializeField]
    HealthBar PlayerHealthBar;

    GunLogic m_CurrentWeapon;
    AudioSource m_AudioSource;

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

        PlayerHealth.OnDied += () => StartCoroutine(PlayerDied());

        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(m_TutorialEndTime < Time.time)
        {
            m_TutorialGroup.SetActive(false);
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

    // --------------------------------------------------------------

    public void SetTutorialMessage(string message)
    {
        m_TutorialText.text = message;
        m_TutorialEndTime = Time.time + TutorialDuration;
        m_TutorialGroup.SetActive(true);

        if(m_AudioSource && TutorialSound)
        {
            m_AudioSource.PlayOneShot(TutorialSound);
        }
    }

    // --------------------------------------------------------------

    public void GotToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    IEnumerator PlayerDied()
    {
        yield return new WaitForSeconds(3.0f);
        m_DieScreen.SetActive(true);
    }
}

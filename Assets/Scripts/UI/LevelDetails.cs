using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDetails : MonoBehaviour
{
    public Text LevelTitle;

    public GameObject LockedIcon;
    public GameObject LockItems;

    public Transform MedalsContainer;
    public Image MedalIconContent;

    bool m_Locked;
    LevelInfo m_LevelInfo;

    public void SetLevelInfo(GameDataManager data, LevelInfo info)
    {
        m_LevelInfo = info;

        LevelTitle.text = m_LevelInfo.LevelDisplayName;
        
        for(int i = 0; i < info.Medals.Length; ++i)
        {
            MedalBase Medal = data.GetMedalWithID(info.Medals[i]);

            Image MedalIcon = Instantiate(MedalIconContent, MedalsContainer);
            MedalIcon.sprite = Medal.MedalIcon;

            RectTransform rectTransform = MedalIcon.GetComponent<RectTransform>();
            Vector2 pos = rectTransform.anchoredPosition;
            pos.x += rectTransform.sizeDelta.x * i;
            rectTransform.anchoredPosition = pos;
        }
    }

    public void SetLocked(bool Locked)
    {
        m_Locked = Locked;
        LockedIcon.SetActive(m_Locked);
        LockItems.SetActive(!m_Locked);
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(m_LevelInfo.LevelSceneName);
    }
}

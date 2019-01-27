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

    bool m_Locked;
    LevelInfo m_LevelInfo;

    public void SetLevelInfo(LevelInfo info)
    {
        m_LevelInfo = info;

        LevelTitle.text = m_LevelInfo.LevelDisplayName;
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

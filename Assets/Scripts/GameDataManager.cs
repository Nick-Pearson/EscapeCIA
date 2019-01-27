using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelInfo
{
    // Scene name is the unique ID for the level
    public string LevelSceneName;

    public string LevelDisplayName;

    [HideInInspector]
    public bool Complete;

    [HideInInspector]
    public string[] Medals;
}

// Manages loading and storing of game data
public class GameDataManager : MonoBehaviour
{
    public LevelInfo[] levelInfo;

    public GunLogic[] AllWeapons;

    class GameData
    {
        public string[] PlayerWeapons;
        public LevelInfo[] LevelInfo;
    }

    GameData m_Data;

    bool m_Loaded = false;
    static string PREF_KEY;

    GameData GetDefaultData()
    {
        GameData data = new GameData();
        data.PlayerWeapons = new string[] { "PIS" };
        data.LevelInfo = new LevelInfo[0] { };

        return data;
    }

    void LoadIfRequired()
    {
        if (m_Loaded) return;
        
        m_Data = GetDefaultData();

        if (PlayerPrefs.HasKey(PREF_KEY))
        {
            string DataStr = PlayerPrefs.GetString(PREF_KEY);
            JsonUtility.FromJsonOverwrite(DataStr, m_Data);
        }

        // merge the loaded level data with the data we have
        for(int i = 0; i < m_Data.LevelInfo.Length; ++i)
        {
            LevelInfo loadedInfo = m_Data.LevelInfo[i];

            if(loadedInfo.Complete || loadedInfo.Medals.Length != 0)
            {
                // find the matching entry in our level data
                // and overwrite it. We cannot garantee the array indicies will be the same
                // as new levels may have been added
                for(int j = 0; j < levelInfo.Length; ++j)
                {
                    if(levelInfo[j].LevelSceneName == loadedInfo.LevelSceneName)
                    {
                        levelInfo[j].Complete = loadedInfo.Complete;
                        levelInfo[j].Medals = loadedInfo.Medals;
                        break;
                    }
                }
            }
        }


        m_Loaded = true;
    }

    public void SaveData()
    {
        m_Data.LevelInfo = levelInfo;

        string DataStr = JsonUtility.ToJson(m_Data);
        Debug.Log(DataStr);
        PlayerPrefs.SetString(PREF_KEY, DataStr);
    }

    public LevelInfo[] GetLevelInfo()
    {
        LoadIfRequired();

        return levelInfo;
    }

    public void SavePlayerWeapons(GunLogic[] Weapons)
    {
        string[] weaponIDs = new string[Weapons.Length];

        for(int i = 0; i < Weapons.Length; ++i)
        {
            weaponIDs[i] = Weapons[i].WeaponID;
        }

        m_Data.PlayerWeapons = weaponIDs;
        SaveData();
    }

    public GunLogic[] GetPlayerWeapons()
    {
        LoadIfRequired();

        GunLogic[] outWeapons = new GunLogic[m_Data.PlayerWeapons.Length];
        for(int i = 0; i < outWeapons.Length; ++i)
        {
            outWeapons[i] = GetWeaponWithID(m_Data.PlayerWeapons[i]);
        }

        return outWeapons;
    }

    public GunLogic GetWeaponWithID(string ID)
    {
        foreach(GunLogic gun in AllWeapons)
        {
            if (gun.WeaponID == ID) return gun;
        }

        return null;
    }

    public void MarkLevelCompleted(string SceneName)
    {
        for (int i = 0; i < levelInfo.Length; ++i)
        {
            if (levelInfo[i].LevelSceneName == SceneName)
            {
                levelInfo[i].Complete = true;
                break;
            }
        }

        SaveData();
    }

    public string GetNextLevelName(string CurrentLevel)
    {
        for (int i = 0; i < levelInfo.Length; ++i)
        {
            if (levelInfo[i].LevelSceneName != CurrentLevel)
                continue;
            
            // if we are the last level return nothing
            if(i+1 >= levelInfo.Length)
            {
                return "";
            }

            return levelInfo[i + 1].LevelSceneName;
        }

        return "";
    }
}

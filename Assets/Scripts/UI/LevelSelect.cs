using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public LevelDetails Template;

    void Start()
    {
        GameDataManager Manager = FindObjectOfType<GameDataManager>();
        LevelInfo[] infos = Manager.GetLevelInfo();

        for(int i = 0; i < infos.Length; ++i)
        {
            LevelDetails instance = Instantiate(Template, transform);
            instance.SetLevelInfo(infos[i]);
            instance.SetLocked(i > 0 && !infos[i - 1].Complete);

            RectTransform newRectTransform = instance.GetComponent<RectTransform>();
            Vector2 tmpTransform = newRectTransform.anchoredPosition;

            tmpTransform.y = (i * -110.0f) - 60.0f;
            newRectTransform.anchoredPosition = tmpTransform;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MedalBase : ScriptableObject
{
    public string MedalID;
    public string MedalName;
    public string MedalDescription;
    public Sprite MedalIcon;

    // returns true if the player has achieved this medal
    public abstract bool AchievedMedal(GameDataManager Data);
}

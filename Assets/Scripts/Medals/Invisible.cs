using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CIA/Medals/Invisible", order = 1)]
public class Invisible : MedalBase
{
    public override bool AchievedMedal(GameDataManager Data)
    {
        return Data.TimesFound == 0;
    }
}

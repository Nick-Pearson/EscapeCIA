using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CIA/Medals/Massacre", order = 1)]
public class Massacre : MedalBase
{
    public override bool AchievedMedal(GameDataManager Data)
    {
        return Data.EnemiesKilled >= Data.TotalEnemies;
    }
}

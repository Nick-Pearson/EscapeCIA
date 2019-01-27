using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CIA/Medals/Pacifist", order = 1)]
public class Pacifist : MedalBase
{
    public override bool AchievedMedal(GameDataManager Data)
    {
        return Data.EnemiesKilled == 0;
    }
}

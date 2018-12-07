using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CIA/LevelTextureSet", order = 1)]
public class LevelTextureSet : ScriptableObject {
  public Material floorMaterial;
  public Material wallMaterial;
}

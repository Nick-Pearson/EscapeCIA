using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor
{
  public override void OnInspectorGUI()
  {
      DrawDefaultInspector();

      LevelDesigner designer = (LevelDesigner)target;
      if(GUILayout.Button("Generate Level"))
      {
          designer.GenerateLevel();
      };
  }

  public void OnSceneGUI()
  {
    //value = Handles.PositionHandle(value, Quaternion.identity);
  }
}

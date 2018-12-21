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

          Mesh mesh = designer.gameObject.GetComponent<MeshFilter>().sharedMesh;
          if(mesh)
          {
            Unwrapping.GenerateSecondaryUVSet(mesh);
          }
      };
  }

  public void OnSceneGUI()
  {
    //value = Handles.PositionHandle(value, Quaternion.identity);
  }
}

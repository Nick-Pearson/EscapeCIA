using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIController))]
[CanEditMultipleObjects]
public class AIControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AIController controller = (AIController)target;
    }

    public void OnSceneGUI()
    {
        //value = Handles.PositionHandle(value, Quaternion.identity);
        AIController controller = (AIController)target;
        
        for(int i = 0; i < controller.Patrol.Length; ++i)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 position = controller.Patrol[i].Position;

            Handles.Label(position + new Vector3(0.0f, 0.1f, 0.0f), "Waypoint #" + i);

            position = Handles.PositionHandle(controller.Patrol[i].Position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(controller, "Modified waypoint");
                controller.Patrol[i].Position = position;
            }
        }
    }
}

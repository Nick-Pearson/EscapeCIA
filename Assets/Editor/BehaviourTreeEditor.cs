using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(BehaviourTree))]
public class BehaviourTreeEditor : Editor
{
    int selectorIndex = 0;

    public override void OnInspectorGUI()
    {
        BehaviourTree tree = (BehaviourTree)target;
        SerializedObject serializedTree = new SerializedObject(tree);

        DrawDefaultInspector();

        serializedTree.Update();
        
        //if (tree.root.GetType() != typeof(Task))
        {
            //DrawObject(tree.root);
        }
        //else
        {
            Task newRoot = DrawSelector<Task>();
            if (newRoot != null)
            {
                Undo.RecordObject(tree, "Modified root");
                //tree.root = newRoot;
            }
        }

        serializedTree.ApplyModifiedProperties();
    }

    public void DrawObject(object o)
    {
        if (o == null)
        {
            EditorGUILayout.LabelField("NULL");
            return;
        }

        FieldInfo[] infos = o.GetType().GetFields();
        Debug.Log(o.GetType());
        foreach(FieldInfo info in infos)
        {
            //if (!info.IsPublic) continue;
            
            Debug.Log(info.Name + ", " );
        }
        
    }

    void DrawTreeNode(SerializedProperty prop)
    {

    }

    T DrawSelector<T>() where T : class
    {
        System.Type[] allTypes = typeof(T).Assembly.GetTypes();

        List<string> names = new List<string>();
        List<System.Type> types = new List<System.Type>();

        for (int i = 0; i < allTypes.Length; ++i)
        {
            if (allTypes[i].IsSubclassOf(typeof(T)) && !allTypes[i].IsAbstract)
            {
                names.Add(allTypes[i].Name);
                types.Add(allTypes[i]);
            }
        }

        T returnVal = null;

        EditorGUILayout.BeginHorizontal("Label");
        selectorIndex = EditorGUILayout.Popup(selectorIndex, names.ToArray());

        if (GUILayout.Button("Create"))
        {
            //returnVal = (T)System.Activator.CreateInstance(types[selectorIndex]);
        }

        EditorGUILayout.EndHorizontal();

        return returnVal;
    }
}

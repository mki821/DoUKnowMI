using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateBlock))]
public class BlockTestInInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateBlock createBlock = (CreateBlock)target;
        
        if (GUILayout.Button("Create Blocks"))
        {
            createBlock.Create();
        }
        else if (GUILayout.Button("Clear Blocks")) {
            createBlock.Clear();
        }
        else if (GUILayout.Button("Move Slime")) {
            createBlock.Move();
        }
    }
}

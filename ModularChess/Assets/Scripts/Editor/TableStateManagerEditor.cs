using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TableStateManager))]
public class TableStateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TableStateManager tableStateManager = (TableStateManager)target;

        if (GUILayout.Button("Print Table States"))
        {
            tableStateManager.PrintTableStates();
        }
    }
}

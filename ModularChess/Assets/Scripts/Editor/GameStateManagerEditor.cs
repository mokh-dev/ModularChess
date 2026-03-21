using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameStateManager))]
public class GameStateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameStateManager gameStateManager = (GameStateManager)target;

        if (GUILayout.Button("Print Board States"))
        {
            gameStateManager.PrintBoardStates();
        }

        
        if (GUILayout.Button("Print Table States"))
        {
            gameStateManager.PrintTableStates();
        }
    }
}

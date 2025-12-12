using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(BoardStateManager))]
public class BoardStateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoardStateManager boardStateManager = (BoardStateManager)target;

        if (GUILayout.Button("Add Test Piece"))
        {
            boardStateManager.AddTestPiece();
        }

        if (GUILayout.Button("Show Board Dictionary"))
        {
            boardStateManager.PrintDictionary();
        }
    }
}

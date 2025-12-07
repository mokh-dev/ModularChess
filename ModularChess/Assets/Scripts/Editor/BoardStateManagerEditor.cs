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
        BoardStateManager boardGenerator = (BoardStateManager)target;

        if (GUILayout.Button("Add Test Piece"))
        {
            boardGenerator.AddTestPiece();
        }
    }
}

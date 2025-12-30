using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(BoardPiecesManager))]
public class BoardPiecesManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoardPiecesManager boardPiecesManager = (BoardPiecesManager)target;

        if (GUILayout.Button("Add Test Piece"))
        {
            boardPiecesManager.AddTestPiece();
        }

        if (GUILayout.Button("Show Board Dictionary"))
        {
            boardPiecesManager.PrintDictionary();
        }
    }
}

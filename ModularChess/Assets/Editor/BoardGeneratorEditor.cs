using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardGenerator))]
public class BoardGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BoardGenerator boardGenerator = (BoardGenerator)target;

        if (GUILayout.Button("Regenerate Board"))
        {
            boardGenerator.EraseBoard();
            boardGenerator.DrawBoard();
        }
    }
}

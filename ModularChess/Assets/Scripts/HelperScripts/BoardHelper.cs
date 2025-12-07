using UnityEngine;

public static class BoardHelper
{
    public static Vector2 ConvertIntPosToVector2Pos(int indexPos)
    {
        int y = Mathf.FloorToInt(indexPos/8);

        int x = indexPos - (y*8);

        return new Vector2(x, y);
    }

    public static int ConvertVector2PosToIntPos(Vector2 vectorPos)
    {
        int intPos = 0;

        intPos = (int)vectorPos.y*8;

        intPos += (int)vectorPos.x;

        return intPos;
    }
}

using System.Collections.Generic;

using UnityEngine;


public class PawnMovementPattern : MonoBehaviour, IMovement, IAttack
{
    public List<Vector2> FindMoves(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 oneStep = new Vector2(currentPiece.transform.position.x, currentPiece.transform.position.y+1);

        

        possibleMoves.Add(oneStep);

        // BoardStateManager.Instance


        return possibleMoves;
    }

    public void SpawnMoveMarkers(List<Vector2> spawnLocations)
    {
        foreach (var location in spawnLocations)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.possibleMoveMarkerPre, location, Quaternion.identity);
            BoardStateManager.Instance.Markers.Add(newMarker);
        }
    }

    public List<Vector2> FindAttacks(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece)
    {
        List<Vector2> possibleAttacks = new List<Vector2>();

        Vector2 topRight = new Vector2(currentPiece.transform.position.x+1, currentPiece.transform.position.y+1);
        Vector2 topLeft = new Vector2(currentPiece.transform.position.x-1, currentPiece.transform.position.y+1);

        //check if on pos exists on board

        possibleAttacks.Add(topRight);
        possibleAttacks.Add(topLeft);

        // BoardStateManager.Instance


        return possibleAttacks;
    }

    public void SpawnAttackMarkers(List<Vector2> spawnLocations)
    {
        foreach (var location in spawnLocations)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.possibleAttackMarkerPre, location, Quaternion.identity);
            BoardStateManager.Instance.Markers.Add(newMarker);
        }
    }
}



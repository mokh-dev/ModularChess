using UnityEngine;
using System.Collections.Generic;

public interface IMovement
{
    List<Vector2> FindMoves(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece);

    void SpawnMoveMarkers(List<Vector2> spawnLocations);
}

public interface IAttack
{
    List<Vector2> FindAttacks(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece);

    void SpawnAttackMarkers(List<Vector2> spawnLocations);
}
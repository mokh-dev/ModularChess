using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Objects/PieceData")]
public class PieceData : ScriptableObject //TODO make all piece data into a struct
{
    [field: SerializeField] public string Title {get; private set;}
    [field: SerializeField] public PieceMovementType MovementType {get; private set;}
    [field: SerializeField] public PieceAttackType AttackType {get; private set;}
    [field: SerializeField] public Teams Team {get; private set;}
    [field: SerializeField] public Sprite Art {get; private set;}
    [field: SerializeField] public int Health {get; private set;}
    [field: SerializeField] public int Damage {get; private set;}
    [field: SerializeField] public int MovementRange {get; private set;}
    [field: SerializeField] public int AttackingRange {get; private set;}
}

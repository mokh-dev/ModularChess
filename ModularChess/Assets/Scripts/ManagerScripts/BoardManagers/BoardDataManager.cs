using UnityEngine;

[ExecuteInEditMode]
public class BoardDataManager : MonoBehaviour
{

    private static BoardDataManager instance;
    public static BoardDataManager Instance { get { return instance; } }

    [field: SerializeField] public Vector2 BoardShape {get; private set;}



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }


    public PieceMoveLogic GetPieceMoveLogic(PieceLogicType pieceType)
    {
        switch (pieceType)
        {
            case PieceLogicType.Pawn:
                return new PawnController();
            
            case PieceLogicType.Knight:
                return new KnightController();    

            case PieceLogicType.Bishop:
                return new BishopController();     

            case PieceLogicType.Rook:
                return new RookController(); 

            case PieceLogicType.Queen:
                return new QueenController();  

            case PieceLogicType.King:
                return new KingController();        
            
            default:
                return null;
        }
    }
}

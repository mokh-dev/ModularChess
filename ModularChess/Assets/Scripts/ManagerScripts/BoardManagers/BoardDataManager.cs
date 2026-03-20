using UnityEngine;

[ExecuteInEditMode]
public class BoardDataManager : MonoBehaviour
{


    private static BoardDataManager instance;

    public static BoardDataManager Instance { get { return instance; } }

    
    [field: SerializeField] public Color WhitePieceTeamColor {get; private set;}
    [field: SerializeField] public Color BlackPieceTeamColor {get; private set;}

    [field: SerializeField] public Sprite PawnSprite {get; private set;}
    [field: SerializeField] public Sprite KnightSprite {get; private set;}
    [field: SerializeField] public Sprite BishopSprite {get; private set;}
    [field: SerializeField] public Sprite RookSprite {get; private set;}
    [field: SerializeField] public Sprite QueenSprite {get; private set;}
    [field: SerializeField] public Sprite KingSprite {get; private set;}

    [field: SerializeField] public Vector2 BoardShape {get; private set;}

    [field: SerializeField] public GameObject BasePiecePre {get; private set;}
    [field: SerializeField] public GameObject PossibleMovementMarkerPre {get; private set;}
    [field: SerializeField] public GameObject PossibleAttackMarkerPre {get; private set;}

    



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


    public PieceMoveLogic GetLogicFromPieceType(PieceTypes pieceType)
    {
        switch (pieceType)
        {
            case PieceTypes.Pawn:
                return new PawnController();
            
            case PieceTypes.Knight:
                return new KnightController();    

            case PieceTypes.Bishop:
                return new BishopController();     

            case PieceTypes.Rook:
                return new RookController(); 

            case PieceTypes.Queen:
                return new QueenController();  

            case PieceTypes.King:
                return new KingController();        
            
            default:
                return null;
        }
    }

    public Sprite GetSpriteFromPieceType(PieceTypes pieceType)
    {
        switch (pieceType)
        {
            case PieceTypes.Pawn:
                return PawnSprite;
            
            case PieceTypes.Knight:
                return KnightSprite;    

            case PieceTypes.Bishop:
                return BishopSprite;     

            case PieceTypes.Rook:
                return RookSprite; 

            case PieceTypes.Queen:
                return QueenSprite;  

            case PieceTypes.King:
                return KingSprite;        
            
            default:
                return null;
        }
    }


}

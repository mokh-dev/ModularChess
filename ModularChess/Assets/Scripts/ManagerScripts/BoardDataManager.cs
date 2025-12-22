using UnityEngine;

[ExecuteInEditMode]
public class BoardDataManager : MonoBehaviour
{


    private static BoardDataManager instance;

    public static BoardDataManager Instance { get { return instance; } }

    
    [field: SerializeField] public Color WhitePieceTeamColor {get; private set;}
    [field: SerializeField] public Color BlackPieceTeamColor {get; private set;}

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
}

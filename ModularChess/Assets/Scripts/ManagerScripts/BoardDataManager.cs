using UnityEngine;

[ExecuteInEditMode]
public class BoardDataManager : MonoBehaviour
{


    private static BoardDataManager _instance;

    public static BoardDataManager Instance { get { return _instance; } }

    
    [field: SerializeField] public Color LightPieceTeamColor {get; private set;}
    [field: SerializeField] public Color DarkPieceTeamColor {get; private set;}

    [field: SerializeField] public Vector2 boardShape {get; private set;}

    [field: SerializeField] public GameObject basePiecePre {get; private set;}
    [field: SerializeField] public GameObject possibleMoveMarkerPre {get; private set;}
    [field: SerializeField] public GameObject possibleAttackMarkerPre {get; private set;}



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : Singleton<PieceSpawner>
{
    [SerializeField] private GameObject[] piecePrefabs;

    private BoardData boardData = new BoardData();   
    private const string boardDataFilename = "BoardData";

    void  Awake() {
        LoadBoardData();
    }

    private void OnEnable()
    {
        BoardManager.OnFinishSetup += SpawnPieces;        
    }

    private void OnDisable() {
        BoardManager.OnFinishSetup -= SpawnPieces;
        
    }

    private void LoadBoardData(){
        string json = SaveLoadJSON.Instance.LoadFromJsonFile(boardDataFilename);
        JsonUtility.FromJsonOverwrite(json, boardData);        
    }
        
    // Start is called before the first frame update
    void SpawnPieces()
    {
        Debug.Log(boardData.tilePieces.Count);
        for (int i = 0; i < BoardManager.Instance.GetTileList().Count; i++)
        {
            if(boardData.tilePieces[i].type == 0)
                continue;

            Tile tile =  BoardManager.Instance.GetTileList()[i];
            GameObject tileGO = tile.gameObject;
            GameObject pieceGO = Instantiate(piecePrefabs[boardData.tilePieces[i].type - 1], tile.transform.position, tile.transform.rotation, transform);
            Piece pieceComponent = pieceGO.GetComponent<Piece>();
            pieceComponent.Setup(tile, boardData.tilePieces[i].type, boardData.tilePieces[i].team);

            pieceGO.name = string.Format("{0} {1}", pieceComponent.GetPieceTeam().ToString(), pieceComponent.GetPieceType().ToString()) ;
            
            BoardManager.Instance.GetTileList()[i].SetCurrentPiece(pieceComponent);

        }   
    }

    public List<Piece> GetTeamPieces(int team, int type = 0){
        List<Piece> pieces = new List<Piece>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Piece piece = transform.GetChild(i).GetComponent<Piece>();
            if( (int) piece.GetPieceTeam() == team && piece.isActiveAndEnabled){                
                if(type == 0)
                    pieces.Add(piece);
                else{
                    if((int)piece.GetPieceType() == type)
                        pieces.Add(piece);
                }

            }

        }

        return pieces;
    }

    public Piece GetTeamKing(int team){
        
        Piece piece = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            piece = transform.GetChild(i).GetComponent<Piece>();
            if( (int) piece.GetPieceTeam() == team && piece.GetPieceType() == Piece.Type.King){                
                return piece;
            }
        }

        return piece;
    }

    public GameObject GetPiecePrefab(int type){
        return piecePrefabs[type];                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

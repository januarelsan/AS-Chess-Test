using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;

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
        for (int i = 0; i < BoardManager.Instance.GetTiles().Count; i++)
        {
            if(boardData.tilePieces[i].type == 0)
                continue;

            GameObject tile =  BoardManager.Instance.GetTiles()[i];
            GameObject pieceGO = Instantiate(piecePrefab, tile.transform.position, tile.transform.rotation, transform);

        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

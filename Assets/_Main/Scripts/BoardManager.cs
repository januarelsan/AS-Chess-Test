using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardManager : Singleton<BoardManager>
{
    public delegate void FinishSetup();
    public static event FinishSetup OnFinishSetup;

    public BoardData BoardData
    {
        get { return boardData; }
        
    }

    [SerializeField] private GameObject tilePrefab;

    private List<Tile> tileList = new List<Tile>();
    private Dictionary<Vector2,Tile> tileDic = new Dictionary<Vector2,Tile>();

    private BoardData boardData = new BoardData();    

    private const string boardDataFilename = "BoardData";

    

    

    public void Setup(){
        LoadBoardData();
        GenerateTiles(boardData.colCount, boardData.rowCount);
    }

    private void LoadBoardData(){
        string json = SaveLoadJSON.Instance.LoadFromJsonFile(boardDataFilename);
        JsonUtility.FromJsonOverwrite(json, boardData);        
    }

    private void GenerateTiles(int colCount, int rowCount){
        GameObject tile = new GameObject();
        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                tile = GenerateTile(x,y);
                tileList.Add(tile.GetComponent<Tile>());
            }
        }

        tileDic = tileList.ToDictionary(tile => tile.GetCoordinate(), tile => tile);

        OnFinishSetup();
    }

    private GameObject GenerateTile(int x, int y){
        GameObject tile = Instantiate(tilePrefab, new Vector3(x,0,y), tilePrefab.transform.rotation, transform);        
        tile.GetComponent<Tile>().Setup(x,y);
        return tile;
    }

    public List<Tile> GetTileList(){
        return tileList;
    }

    public Dictionary<Vector2, Tile> GetTileDic(){
        return tileDic;
    }  

    // Update is called once per frame
    void Update()
    {
        
    }
}

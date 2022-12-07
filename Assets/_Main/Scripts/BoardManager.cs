using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public delegate void FinishSetup();
    public static event FinishSetup OnFinishSetup;

    [SerializeField] private int rowCount;
    [SerializeField] private int colCount;
    [SerializeField] private GameObject tilePrefab;

    private List<GameObject> tiles = new List<GameObject>();

    private BoardData boardData = new BoardData();    

    private const string boardDataFilename = "BoardData";


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Setup(){
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
                tiles.Add(tile);
            }
        }

        OnFinishSetup();
    }

    private GameObject GenerateTile(int x, int y){
        GameObject tile = Instantiate(tilePrefab, new Vector3(x,0,y), tilePrefab.transform.rotation, transform);        
        tile.GetComponent<Tile>().Setup(x,y);
        return tile;
    }

    public List<GameObject> GetTiles(){
        return tiles;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}

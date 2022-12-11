using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Button pieceButtonTemplate;
    [SerializeField] private InputField rowIF;
    [SerializeField] private InputField colIF;
    [SerializeField] private Text logText;

    private BoardData boardData = new BoardData();    

    private const int defaultRowColCount = 8;
    private const int maxRowColCount = 8;
    private const int minRowColCount = 4;
    private const string boardDataFilename = "BoardData";

    private int maxPieceCount;

    private int rowCount = defaultRowColCount;
    private int colCount = defaultRowColCount;

    void Start(){
        maxPieceCount = maxRowColCount * maxRowColCount;
        LoadBoardData();
        PopulatePieceButtons();
    }

    void PopulatePieceButtons(){
        int defautlPieceCount = boardData.colCount * boardData.rowCount;

        bool evenNumberToGrey = true;
        int colIndex = 0;
        gridLayoutGroup.constraintCount = boardData.colCount;        

        for (int i = 0; i < maxPieceCount; i++)
        {
            GameObject pieceButtonGO = Instantiate(pieceButtonTemplate.gameObject, pieceButtonTemplate.transform.position, 
                pieceButtonTemplate.transform.rotation, gridLayoutGroup.transform);

            if(boardData.tilePieces.Count == maxPieceCount){
                TilePiece tilePiece = boardData.tilePieces[i];
                pieceButtonGO.GetComponent<PieceButton>().Setup(tilePiece.type, tilePiece.team);
            }
            
            if(i >= defautlPieceCount)
                pieceButtonGO.SetActive(false);

            int y = i / boardData.colCount;
            int x = i - (y * boardData.colCount);
            
            SetNameAndColor(pieceButtonGO, i, evenNumberToGrey);

            
            colIndex++;
            if(colIndex > boardData.colCount - 1){
                colIndex = 0;
                evenNumberToGrey = !evenNumberToGrey;
            }
            
        }

        Destroy(pieceButtonTemplate.gameObject);
    }
    
    public void Generate(){
        
        logText.text = "";

        rowCount = int.Parse(rowIF.text);
        colCount = int.Parse(colIF.text);
                        
        if(rowCount > maxRowColCount || colCount > maxRowColCount ){
            logText.text = "Maximum Row & Column Count is " + maxRowColCount;
            rowCount = maxRowColCount;
            colCount = maxRowColCount;
        }

        if(rowCount < minRowColCount || colCount < minRowColCount ){
            logText.text = "Minimum Row & Column Count is " + minRowColCount;
            rowCount = minRowColCount;
            colCount = minRowColCount;
        }

        int totalPieceCount = rowCount * colCount;

        gridLayoutGroup.constraintCount = colCount;        
        
        bool evenNumberToGrey = true;
        int colIndex = 0;
        for (int i = 0; i < maxPieceCount; i++)
        {
            GameObject pieceButtonGO = gridLayoutGroup.transform.GetChild(i).gameObject;
            
            if(i >= totalPieceCount)
                pieceButtonGO.SetActive(false);
            else
                pieceButtonGO.SetActive(true);

            SetNameAndColor(pieceButtonGO, i, evenNumberToGrey);
                        
            colIndex++;
            if(colIndex > colCount - 1){
                colIndex = 0;
                evenNumberToGrey = !evenNumberToGrey;
            }

        }

    }

    public void SetNameAndColor(GameObject go, int i, bool evenNumberToGrey){
                                    
        
        bool isTileEven = i % 2 == 0;        
        
        if(evenNumberToGrey && isTileEven){
            go.GetComponent<Image>().color = Color.grey;
        } else if(!evenNumberToGrey && !isTileEven){
            go.GetComponent<Image>().color = Color.grey;
        } else {
            go.GetComponent<Image>().color = Color.white;
        }
    }

    public void Save(){
        
        boardData.tilePieces.Clear();

        int whiteKingCount = 0;
        int blackKingCount = 0;
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            PieceButton pieceButton = gridLayoutGroup.transform.GetChild(i).GetComponent<PieceButton>();
            TilePiece tilePiece = new TilePiece(pieceButton.GetTypeInt(), pieceButton.GetTeamInt());
            boardData.tilePieces.Add(tilePiece);            

            if(tilePiece.type == 6){
                if(tilePiece.team == 0)
                    whiteKingCount++;
                else
                    blackKingCount++;
            }
        }

        if(whiteKingCount > 1 || blackKingCount > 1){
            logText.text = "King can't be more than 1";
            return;
        }

        if(whiteKingCount < 1 || blackKingCount < 1){
            logText.text = "King can't be zero";
            return;
        }

        boardData.rowCount = rowCount;
        boardData.colCount = colCount;

        SaveLoadJSON.Instance.SaveIntoJsonFile(boardData, boardDataFilename);

        logText.text = "Board Saved";

    }

    private void LoadBoardData(){
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + boardDataFilename + ".json"))
        {            
            Debug.Log("Exist");
            string json = SaveLoadJSON.Instance.LoadFromJsonFile(boardDataFilename);
            JsonUtility.FromJsonOverwrite(json, boardData);        
        } else {
            SetupDefaultBoard();
        }

        
    }

    void SetupDefaultBoard(){
        
        boardData.rowCount = 8;
        boardData.colCount = 8;

        TilePiece tilePiece = new TilePiece(0, 0);

        boardData.tilePieces = new List<TilePiece>(100);


        for (int i = 0; i < 8 * 8; i++)
        {
            
            tilePiece = new TilePiece(0, 0);
            boardData.tilePieces.Add(tilePiece);                      
        }

        //White Pieces
        boardData.tilePieces[0] = new TilePiece(4, 0);
        boardData.tilePieces[1] = new TilePiece(2, 0);
        boardData.tilePieces[2] = new TilePiece(3, 0);
        boardData.tilePieces[3] = new TilePiece(5, 0);
        boardData.tilePieces[4] = new TilePiece(6, 0);
        boardData.tilePieces[5] = new TilePiece(3, 0);
        boardData.tilePieces[6] = new TilePiece(2, 0);
        boardData.tilePieces[7] = new TilePiece(4, 0);

        // White Pawns
        boardData.tilePieces[8] = new TilePiece(1, 0);
        boardData.tilePieces[9] = new TilePiece(1, 0);
        boardData.tilePieces[10] = new TilePiece(1, 0);
        boardData.tilePieces[11] = new TilePiece(1, 0);
        boardData.tilePieces[12] = new TilePiece(1, 0);
        boardData.tilePieces[13] = new TilePiece(1, 0);
        boardData.tilePieces[14] = new TilePiece(1, 0);
        boardData.tilePieces[15] = new TilePiece(1, 0);
        
        // Black Pawn
        boardData.tilePieces[48] = new TilePiece(1, 1);
        boardData.tilePieces[49] = new TilePiece(1, 1);
        boardData.tilePieces[50] = new TilePiece(1, 1);
        boardData.tilePieces[51] = new TilePiece(1, 1);
        boardData.tilePieces[52] = new TilePiece(1, 1);
        boardData.tilePieces[53] = new TilePiece(1, 1);
        boardData.tilePieces[54] = new TilePiece(1, 1);
        boardData.tilePieces[55] = new TilePiece(1, 1);
        //Black Pieces
        boardData.tilePieces[56] = new TilePiece(4, 1);
        boardData.tilePieces[57] = new TilePiece(2, 1);
        boardData.tilePieces[58] = new TilePiece(3, 1);
        boardData.tilePieces[59] = new TilePiece(5, 1);
        boardData.tilePieces[60] = new TilePiece(6, 1);
        boardData.tilePieces[61] = new TilePiece(3, 1);
        boardData.tilePieces[62] = new TilePiece(2, 1);
        boardData.tilePieces[63] = new TilePiece(4, 1);

        SaveLoadJSON.Instance.SaveIntoJsonFile(boardData, boardDataFilename);

    }
}

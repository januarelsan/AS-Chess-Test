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
    private const int maxRowColCount = 10;
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
        string json = SaveLoadJSON.Instance.LoadFromJsonFile(boardDataFilename);
        JsonUtility.FromJsonOverwrite(json, boardData);        
    }
}

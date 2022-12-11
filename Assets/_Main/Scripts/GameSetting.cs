using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    
    private GameSettingData gameSettingData = new GameSettingData();   
    private const string gameSettingDataFilename = "GameSettingData";

    private BoardData boardData = new BoardData();    
    private const string boardDataFilename = "BoardData";

    void Start(){        
        CheckLoadBoardData();
    }

    public void SelectPlayerOneTeam(int team){
        gameSettingData.playerOneTeam = team;
    }

    public void SelectGameMode(int mode){
        gameSettingData.mode = mode;
    }

    public void Save(){
        SaveLoadJSON.Instance.SaveIntoJsonFile(gameSettingData, gameSettingDataFilename);
    }

    void CheckLoadBoardData(){
        if (!System.IO.File.Exists(Application.persistentDataPath + "/" + boardDataFilename + ".json"))
        {            
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

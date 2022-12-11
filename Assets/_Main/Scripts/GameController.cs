using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public delegate void ChangeTeamTurn(int team);
    public static event ChangeTeamTurn OnChangeTurn;
    private List<Piece> deadPieces = new List<Piece>();
    private int teamTurn = 0;
    private int playerTeam = 0;
    private int gameMode = 0;

    private GameSettingData gameSettingData = new GameSettingData();   
    private const string gameSettingDataFilename = "GameSettingData";
    
    private void OnEnable()
    {
        Piece.OnOccupies += ChangeTurn;                    
    }

    private void OnDisable() {
        Piece.OnOccupies -= ChangeTurn;                
        
    }

    void Start(){
        
        SetupGame();

        CheckDrawGame();  
        PieceSpawner.Instance.GetTeamKing(0).GetComponent<King>().IsCheckmate();
    }

    void SetupGame(){
        
        LoadGameSettingData();

        playerTeam = gameSettingData.playerOneTeam;
        gameMode = gameSettingData.mode;

        BoardManager.Instance.Setup();
        
    }

    private void LoadGameSettingData(){
        string json = SaveLoadJSON.Instance.LoadFromJsonFile(gameSettingDataFilename);
        JsonUtility.FromJsonOverwrite(json, gameSettingData);        
    }

    void CheckKingCheckmate(){                
        
        if(PieceSpawner.Instance.GetTeamKing(0).GetComponent<King>().IsCheckmate() && teamTurn == 0){
            GameoverUI.Instance.Active("White King is Checkmate");
            return;
        }

        if(PieceSpawner.Instance.GetTeamKing(1).GetComponent<King>().IsCheckmate() && teamTurn == 1){
            GameoverUI.Instance.Active("Black King is Checkmate");
            return;
        }
                
    }

    public void AddDeadPiece(Piece deadPiece){
        deadPieces.Add(deadPiece);
        DeadPieceUI.Instance.AddDeadPiece(deadPiece);
    }

    public void ChangeTurn(){
        
        if(teamTurn == 0)
            teamTurn = 1;
        else
            teamTurn = 0;

        RemoveEnpassantablePawns(teamTurn);
        CheckKingCheckmate();
        CheckDrawGame();

        if(OnChangeTurn != null)
            OnChangeTurn(teamTurn);
    }

    public int TeamTurn(){
        return teamTurn;
    }

    public int PlayerTeam(){
        return playerTeam;
    }

    public int GameMode(){
        return gameMode;
    }

    void RemoveEnpassantablePawns(int team){
        List<Piece> pawns =  PieceSpawner.Instance.GetTeamPieces(team, 1);
        foreach (Piece pawn in pawns)
        {
            pawn.GetComponent<Pawn>().RemoveEnpassantable();
        }
    }
    
    void CheckDrawGame(){
        
        //king vs king
        //king & minor vs king
        //king & minor vs king & minor
        //king & 2 knights vs king

        List<Piece> whitePieces = PieceSpawner.Instance.GetTeamPieces(0);
        List<Piece> blackPieces = PieceSpawner.Instance.GetTeamPieces(1);

        //Outside possible draw
        if(whitePieces.Count > 3 || blackPieces.Count > 3 )
            return;
        
        //king vs king
        if(whitePieces.Count == 1 && blackPieces.Count == 1){            
            GameDraw();
            return;
        }        

        //white king & minor vs black king
        if(whitePieces.Count == 2 && blackPieces.Count == 1){            
            KingMinorDrawRule(whitePieces);
        }
        
        //black king & minor vs white king
        if(blackPieces.Count == 2 && whitePieces.Count == 1){            
            KingMinorDrawRule(blackPieces);
        }
        
        // white king & minor vs black king & minor
        if(whitePieces.Count == 2 && blackPieces.Count == 2){            
            BothKingMinorDrawRule(whitePieces, blackPieces);
        }

        // white king & 2 knight vs black king
        if(whitePieces.Count == 3 && blackPieces.Count == 1){            
            KingTwoKnightDrawRule(whitePieces);
        }

        // black king & 2 knight vs white king
        if(blackPieces.Count == 3 && whitePieces.Count == 1){            
            KingTwoKnightDrawRule(blackPieces);
        }
        
    }

    void KingTwoKnightDrawRule(List<Piece> teamPiece){
        
        bool notOnlyKingAndKnight = teamPiece.Find(
        delegate(Piece piece)
        {
            return (int) piece.GetPieceType() != 2 && (int) piece.GetPieceType() != 6 ;                
        });
        
        if(notOnlyKingAndKnight){
            return;
        }           
            
            GameDraw();
        
    }

    void BothKingMinorDrawRule(List<Piece> whitePieces, List<Piece> blackPieces){
        
        bool whiteWithMinor = whitePieces.Find(
        delegate(Piece piece)
        {
            return (int) piece.GetPieceType() == 2 || (int) piece.GetPieceType() == 3 ;                
        });

        bool blackWithMinor = blackPieces.Find(
        delegate(Piece piece)
        {
            return (int) piece.GetPieceType() == 2 || (int) piece.GetPieceType() == 3 ;                
        });
        
        if(whiteWithMinor && blackWithMinor){
            GameDraw();
            return;
        }           
        
    }

    void KingMinorDrawRule(List<Piece> teamPiece){
        
        bool withMinor = teamPiece.Find(
        delegate(Piece piece)
        {
            return (int) piece.GetPieceType() == 2 || (int) piece.GetPieceType() == 3 ;                
        });
        
        if(withMinor){
            GameDraw();
            return;
        }           
        
    }

    void GameDraw(){
        GameoverUI.Instance.Active("Draw: Insufficient Material");
    }

    public void GameStalemate(){
        GameoverUI.Instance.Active("Draw: Stalemate");
    }
}

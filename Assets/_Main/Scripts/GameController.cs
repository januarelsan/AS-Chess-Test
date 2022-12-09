using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private List<Piece> deadPieces = new List<Piece>();
    private int teamTurn = 0;

    
    
    private void OnEnable()
    {
        Piece.OnOccupies += CheckKingCheckmate;        
        Piece.OnOccupies += TakeTurn;        
    }

    private void OnDisable() {
        Piece.OnOccupies -= CheckKingCheckmate;                
        Piece.OnOccupies -= TakeTurn;        
    }

    void CheckKingCheckmate(){
        
        if(PieceSpawner.Instance.GetTeamKing(0).GetComponent<King>().IsCheckmate()){
            GameoverUI.Instance.Active("White King is Checkmate");
        }

        if(PieceSpawner.Instance.GetTeamKing(1).GetComponent<King>().IsCheckmate()){
            GameoverUI.Instance.Active("Black King is Checkmate");
        }
    }

    public void AddDeadPiece(Piece deadPiece){
        deadPieces.Add(deadPiece);
        DeadPieceUI.Instance.AddDeadPiece(deadPiece);
    }

    public void TakeTurn(){
        if(teamTurn == 0)
            teamTurn = 1;
        else
            teamTurn = 0;

        RemoveEnpassantablePawns(teamTurn);
    }

    public int TeamTurn(){
        return teamTurn;
    }

    void RemoveEnpassantablePawns(int team){
        List<Piece> pawns =  PieceSpawner.Instance.GetTeamPieces(team, 1);
        foreach (Piece pawn in pawns)
        {
            pawn.GetComponent<Pawn>().RemoveEnpassantable();
        }
    }
}

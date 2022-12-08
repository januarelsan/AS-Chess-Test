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
        Debug.Log("White King is Checkmate " + PieceSpawner.Instance.GetTeamKing(0).GetComponent<King>().IsCheckmate());
        Debug.Log("Black is Checkmate " + PieceSpawner.Instance.GetTeamKing(1).GetComponent<King>().IsCheckmate());
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
    }

    public int TeamTurn(){
        return teamTurn;
    }
}

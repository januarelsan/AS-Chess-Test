using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private List<Piece> deadPieces = new List<Piece>();
    
    private void OnEnable()
    {
        Piece.OnOccupies += CheckKingCheckmate;        
    }

    private void OnDisable() {
        Piece.OnOccupies -= CheckKingCheckmate;                
    }

    void CheckKingCheckmate(){
        Debug.Log("White King is Checkmate " + PieceSpawner.Instance.GetTeamKing(0).GetComponent<King>().IsCheckmate());
        Debug.Log("Black is Checkmate " + PieceSpawner.Instance.GetTeamKing(1).GetComponent<King>().IsCheckmate());
    }

    public void AddDeadPiece(Piece deadPiece){
        deadPieces.Add(deadPiece);
        DeadPieceUI.Instance.AddDeadPiece(deadPiece);
    }
}

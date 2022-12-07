using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private List<Piece> deadPieces = new List<Piece>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDeadPiece(Piece deadPiece){
        deadPieces.Add(deadPiece);
        DeadPieceUI.Instance.AddDeadPiece(deadPiece);
    }
}

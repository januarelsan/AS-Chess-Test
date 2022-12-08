using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPieceUI : Singleton<DeadPieceUI>
{
    [SerializeField] private Transform whitePieceHolder;
    [SerializeField] private Transform blackPieceHolder;
    [SerializeField] private GameObject pieceIconImageGO;
    [SerializeField] private Sprite[] whitePieceIconSprites;
    [SerializeField] private Sprite[] blackPieceIconSprites;

    public void AddDeadPiece(Piece deadPiece){
                 
        GameObject newPieceIconImageGO = Instantiate(pieceIconImageGO.gameObject);
        newPieceIconImageGO.SetActive(true);

        //white piece
        if( (int)deadPiece.GetPieceTeam() == 0){      
            newPieceIconImageGO.transform.parent = whitePieceHolder;      
            newPieceIconImageGO.GetComponent<Image>().sprite = whitePieceIconSprites[ (int) deadPiece.GetPieceType() - 1];
            return;
        }

        //black piece        
        newPieceIconImageGO.transform.parent = blackPieceHolder;
        newPieceIconImageGO.GetComponent<Image>().sprite = blackPieceIconSprites[ (int) deadPiece.GetPieceType() - 1];
        return;

        
    }   
}

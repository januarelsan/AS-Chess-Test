using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteManager : Singleton<PromoteManager>
{
    [SerializeField] private GameObject promotePanel;
    [SerializeField] private GameObject[] teamButtonHolders;
    private Pawn toPromotePawn;
    private Tile toPromoteLastTile;
    private Tile toPromoteTile;

    private bool isChoosingPromotion;


    public void PromotePawn(Pawn pawn, Tile lastTile, Tile toPromoteTile, bool isBeQueen = false){
        toPromotePawn = pawn;
        toPromoteLastTile = lastTile;
        this.toPromoteTile = toPromoteTile;

        if(isBeQueen){
            ChoosePromotionPiece(5);
            return;
        }

        OpenPromotionPanel((int) pawn.GetPieceTeam());
    }

    void OpenPromotionPanel(int team){

        isChoosingPromotion = true;

        promotePanel.SetActive(true);
        teamButtonHolders[team].SetActive(true);
    }

    void ClosePromotionPanel(int team){
        promotePanel.SetActive(false);
        teamButtonHolders[team].SetActive(false);
    }

    
    public void ChoosePromotionPiece(int type){
        
        GameObject pieceGO = Instantiate(PieceSpawner.Instance.GetPiecePrefab(type - 1), PieceSpawner.Instance.transform);
        Piece pieceComponent = pieceGO.GetComponent<Piece>();
        pieceComponent.Setup(toPromoteLastTile, type, (int) toPromotePawn.GetPieceTeam());
        toPromotePawn.GetOccupiedTile().SetCurrentPiece(null);
        pieceComponent.OccupiesTile(toPromoteTile);
        
        toPromotePawn.gameObject.SetActive(false);

        ClosePromotionPanel((int) toPromotePawn.GetPieceTeam());        

        isChoosingPromotion = false;

    }

    public bool GetIsChoosingPromotion(){
        return isChoosingPromotion;
    }
}

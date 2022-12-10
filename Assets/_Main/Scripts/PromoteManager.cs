using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteManager : Singleton<PromoteManager>
{
    [SerializeField] private GameObject promotePanel;
    [SerializeField] private GameObject[] teamButtonHolders;
    private Pawn toPromotePawn;
    private Tile toPromoteLastTile;


    public void PromotePawn(Pawn pawn, Tile lastTile){
        toPromotePawn = pawn;
        toPromoteLastTile = lastTile;

        OpenPromotionPanel((int) pawn.GetPieceTeam());
    }

    void OpenPromotionPanel(int team){
        promotePanel.SetActive(true);
        teamButtonHolders[team].SetActive(true);
    }

    void ClosePromotionPanel(int team){
        promotePanel.SetActive(false);
        teamButtonHolders[team].SetActive(false);
    }

    
    public void ChoosePromotionPiece(int type){
        GameObject pieceGO = Instantiate(PieceSpawner.Instance.GetPiecePrefab(type - 1), toPromotePawn.transform.position, toPromotePawn.transform.rotation, PieceSpawner.Instance.transform);
        Piece pieceComponent = pieceGO.GetComponent<Piece>();
        pieceComponent.Setup(toPromoteLastTile, type, (int) toPromotePawn.GetPieceTeam());
        toPromotePawn.GetOccupiedTile().SetCurrentPiece(null);
        pieceComponent.OccupiesTile(toPromotePawn.GetOccupiedTile());
        
        toPromotePawn.gameObject.SetActive(false);

        ClosePromotionPanel((int) toPromotePawn.GetPieceTeam());

        GameController.Instance.TakeTurn();
    }
}
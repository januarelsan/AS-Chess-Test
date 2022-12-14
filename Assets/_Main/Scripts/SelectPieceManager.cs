using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPieceManager : MonoBehaviour
{
    [SerializeField] private bool isCheckTurn;
    private bool isSelectingPiece;
    private Piece selectedPiece;

    private Tile hoveredTile;

    private void OnEnable()
    {
        Piece.OnSelectPiece += SelectPiece;        
        Piece.OnDeselectPiece += DeselectPiece;        

        Tile.OnHoverTile += HoverTile;        
        Tile.OnHoverExitTile += HoverExitTile;     
    }

    private void OnDisable() {
        Piece.OnSelectPiece -= SelectPiece;        
        Piece.OnDeselectPiece -= DeselectPiece;    

        Tile.OnHoverTile -= HoverTile;        
        Tile.OnHoverExitTile -= HoverExitTile;         
        
    }

    public void SelectPiece(Piece piece){

        if(PromoteManager.Instance.GetIsChoosingPromotion())
            return;
        
        if(GameController.Instance.GameMode() != 0){
            if(GameController.Instance.TeamTurn() != GameController.Instance.PlayerTeam())
                return; 
        } 
        
        if(GameController.Instance.GameMode() == 2){
            return; 
        }

        if(isSelectingPiece){
            Debug.Log("Still Selecting a Piece");
            return;
        } 

        if(isCheckTurn){
            if((int) piece.GetPieceTeam() != GameController.Instance.TeamTurn()){
                Debug.Log("Not Your Turn");
                return;
            } 
        }

        Debug.Log("GC Select: " + piece.name);
        selectedPiece = piece;
        isSelectingPiece = true;

        
    }

    public void DeselectPiece(Piece piece){
        if(!isSelectingPiece){
            Debug.Log("No Selected Piece");
            return;
        } 
        
        piece.TryOccupiesTile(hoveredTile);
        
        // Debug.Log("GC Deselect: " + piece.name);
        selectedPiece = null;
        isSelectingPiece = false;
    }

    public void HoverTile(Tile tile){
        hoveredTile = tile;
    }

    public void HoverExitTile(Tile tile){
        hoveredTile = null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedPiece && hoveredTile){
            Vector3 targetPos = hoveredTile.transform.position;
            targetPos.y = 1;
            selectedPiece.transform.position = Vector3.Lerp(selectedPiece.transform.position, targetPos, Time.deltaTime * 10);
        }
    }
}

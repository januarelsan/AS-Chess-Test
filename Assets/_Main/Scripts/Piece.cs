using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public delegate void SelectPiece(Piece piece);
    public static event SelectPiece OnSelectPiece;
    public static event SelectPiece OnDeselectPiece;    
    [SerializeField] private Material[] teamMaterials;    
    [SerializeField] private MeshRenderer meshRenderer;    

    protected Tile occupiedTile;

    public enum Type{
        Undifinied = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }

    public enum Team{
        White = 0,
        Black = 1
    }
    
    private Type type;
    protected Team team;
    
    public void Setup(Tile tile, int type, int team){
        occupiedTile = tile;
        this.type = (Type) type;
        this.team = (Team) team;        

      
        meshRenderer.material = teamMaterials[team];

        if(team == 1)
            transform.Rotate(new Vector3(0,180,0));
    }

    public Type GetPieceType(){
        return type;
    }

    public Team GetPieceTeam(){
        return team;
    }

    public void Select(){        
        OnSelectPiece(this);
    }

    public void Deselect(){
        OnDeselectPiece(this);
    }
    
    public virtual void TryOccupiesTile(Tile targetTile){
        
        if(targetTile == null){
            Debug.Log("No Tile to Occupy");    
            transform.position = occupiedTile.transform.position;
            return;
        }

        bool isLegalTile = GetLegalTileCoordinates().Contains(targetTile.GetCoordinate());        

        if(!isLegalTile){
            Debug.Log("Target Tile is not Legal Tiles");    
            transform.position = occupiedTile.transform.position;
            return;
        }
        

        OccupiesTile(targetTile);
    }

    protected void OccupiesTile(Tile targetTile){
        //Capture Enemy Piece
        if(targetTile.CurrentPiece() != null){
            
            targetTile.CurrentPiece().Dead();
            
        }

        Debug.Log(name + " Occupies Tile " + targetTile.name);
        //Leave Last Tile
        occupiedTile.SetCurrentPiece(null);

        //Set New Occupied Tile
        occupiedTile = targetTile;        
        occupiedTile.SetCurrentPiece(this);
        transform.position = occupiedTile.transform.position;
    }

    private void Dead(){
        occupiedTile = null;
        gameObject.SetActive(false);
        GameController.Instance.AddDeadPiece(this);
    }
    
    protected virtual List<Vector2> GetLegalTileCoordinates(){
        
        List<Vector2> coordinates = new List<Vector2>();        
        return coordinates;
    }

    public Tile GetOccupiedTile(){
        return occupiedTile;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    
    private List<Vector2> coordinates = new List<Vector2>();        
    public override List<Vector2> GetLegalTileCoordinates(){
        
        isCheckProtectedTile = false;

        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        SingleMove(occupiedTileCoord);
       
        return coordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){
        
        isCheckProtectedTile = true;
        
        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        SingleMove(occupiedTileCoord);
       
        return coordinates;
    }

    private void SingleMove(Vector2 occupiedTileCoord){        
        
        Vector2 targetCoord = new Vector2(0,0);
        
        // Up Right
        targetCoord = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y + 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Up Left
        targetCoord = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y + 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Down Right
        targetCoord = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y - 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Down Left
        targetCoord = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y - 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Up
        targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y + 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Down 
        targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y - 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Right
        targetCoord = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Left
        targetCoord = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }
                
            
                
                       
        
    }
        
    private bool BasicLegalTileRule(Tile tile)
    {
        
        if(tile.CurrentPiece() != null){
            if(tile.CurrentPiece().GetPieceTeam() == GetPieceTeam()){
                if(isCheckProtectedTile)
                    coordinates.Add(tile.GetCoordinate());
                return false;
            } 
            coordinates.Add(tile.GetCoordinate());
            return false;
        }

        coordinates.Add(tile.GetCoordinate());
        return true;            
        
    }
    
    public bool IsUnSafeMove(Vector2 targetTile){
        int opsTeam = ((int) team == 1) ? 0 : 1;
        return PieceSpawner.Instance.GetTeamPieces(opsTeam).Find(
            delegate(Piece piece)
            {
                return piece.GetProtectedTileCoordinates().Contains(targetTile);                
            });
        
    }
        
    
    public override void TryOccupiesTile(Tile targetTile){
        
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

        if(IsUnSafeMove(targetTile.GetCoordinate())){
            Debug.Log("Target Tile is not Safe");    
            transform.position = occupiedTile.transform.position;            
            return;
        }
        

        OccupiesTile(targetTile);
    }
    
}

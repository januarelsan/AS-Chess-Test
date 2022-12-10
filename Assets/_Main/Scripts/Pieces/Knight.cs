using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    
    
    public override List<Vector2> GetLegalTileCoordinates(){
        
        isCheckProtectedTile = false;

        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        //L move        
        LMove(occupiedTileCoord);

        return tileCoordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){
        
        isCheckProtectedTile = true;

        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        //L move        
        LMove(occupiedTileCoord);

        return tileCoordinates;
    }

    private void LMove(Vector2 occupiedTileCoord){
        
        Vector2 targetCoord = new Vector2();
        // Up Right
        targetCoord = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y + 2);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Up Left
        targetCoord = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y + 2);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Down Right
        targetCoord = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y - 2);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Down Left
        targetCoord = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y - 2);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Up Right
        targetCoord = new Vector2(occupiedTileCoord.x + 2, occupiedTileCoord.y + 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Up Left
        targetCoord = new Vector2(occupiedTileCoord.x - 2, occupiedTileCoord.y + 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Down Right
        targetCoord = new Vector2(occupiedTileCoord.x + 2, occupiedTileCoord.y - 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }

        // Middle Dow Left
        targetCoord = new Vector2(occupiedTileCoord.x - 2, occupiedTileCoord.y - 1);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            BasicLegalTileRule(targetTile);
        }
            
                
                       
        
    }
    
    
    
    
    
}

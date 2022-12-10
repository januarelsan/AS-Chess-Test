using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
        
    public override List<Vector2> GetLegalTileCoordinates(){

        isCheckProtectedTile = false;
        
        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        Vector2 targetCoord = new Vector2(0,0);
        
        //Register tiles        
        ForwardLongMove(occupiedTileCoord, targetCoord);
        BackwardLongMove(occupiedTileCoord, targetCoord);
        RightLongMove(occupiedTileCoord, targetCoord);
        LeftLongMove(occupiedTileCoord, targetCoord);

        return tileCoordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){

        isCheckProtectedTile = true;
        
        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        Vector2 targetCoord = new Vector2(0,0);
        
        //Register tiles        
        ForwardLongMove(occupiedTileCoord, targetCoord);
        BackwardLongMove(occupiedTileCoord, targetCoord);
        RightLongMove(occupiedTileCoord, targetCoord);
        LeftLongMove(occupiedTileCoord, targetCoord);

        return tileCoordinates;
    }

    
    
    
}

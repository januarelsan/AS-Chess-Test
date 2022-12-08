using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    
    private List<Vector2> coordinates = new List<Vector2>();        
    public override List<Vector2> GetLegalTileCoordinates(){
        
        isCheckProtectedTile = false;

        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        Vector2 targetCoord = new Vector2(0,0);
        
        //Register tiles        
        RightForwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftForwardDiagLongMove(occupiedTileCoord, targetCoord);        
        RightBackwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftBackwardDiagLongMove(occupiedTileCoord, targetCoord);        

        return coordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){

        isCheckProtectedTile = true;
        
        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        Vector2 targetCoord = new Vector2(0,0);

        //Register tiles        
        RightForwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftForwardDiagLongMove(occupiedTileCoord, targetCoord);        
        RightBackwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftBackwardDiagLongMove(occupiedTileCoord, targetCoord);  
                                        
        return coordinates;
    }

    

    private void RightForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private void LeftForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    
    private void RightBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }

    private void LeftBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private bool BasicLegalTileRule(Tile tile)
    {
        
        if(tile.CurrentPiece() != null){
            if(tile.CurrentPiece().GetPieceTeam() == GetPieceTeam()){
                if(isCheckProtectedTile)
                    coordinates.Add(tile.GetCoordinate());
                
                return false;

            } else {

                coordinates.Add(tile.GetCoordinate());
                return isCheckProtectedTile && tile.CurrentPiece().GetPieceType() == Type.King;

            }
        }

        coordinates.Add(tile.GetCoordinate());
        return true;            
        
    }
    
    
    
}
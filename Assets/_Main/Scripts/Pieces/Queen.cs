using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    
    private List<Vector2> coordinates = new List<Vector2>();        
    protected override List<Vector2> GetLegalTileCoordinates(){
        
        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        Vector2 targetCoord = new Vector2(0,0);
        
        //Register tiles        
        RightForwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftForwardDiagLongMove(occupiedTileCoord, targetCoord);        
        RightBackwardDiagLongMove(occupiedTileCoord, targetCoord);
        LeftBackwardDiagLongMove(occupiedTileCoord, targetCoord);      

        ForwardLongMove(occupiedTileCoord, targetCoord);
        BackwardLongMove(occupiedTileCoord, targetCoord);
        RightLongMove(occupiedTileCoord, targetCoord);
        LeftLongMove(occupiedTileCoord, targetCoord);  

        return coordinates;
    }

    private void ForwardLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.y; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    
    private void BackwardLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.y; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }

    private void RightLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private void LeftLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }

    private void RightForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private void LeftForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private void RightBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    private void LeftBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        Debug.Log(BoardManager.Instance.GetTileDic().Count);
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            Debug.Log(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord));
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord);

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

                return false;
            } 
            coordinates.Add(tile.GetCoordinate());
            return false;
        }

        coordinates.Add(tile.GetCoordinate());
        return true;            
        
    }
    
    
    
}

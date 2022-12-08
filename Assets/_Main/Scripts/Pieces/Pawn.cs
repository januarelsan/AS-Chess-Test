using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool isFirstMove = true;
    private List<Vector2> coordinates = new List<Vector2>();      

    public override List<Vector2> GetLegalTileCoordinates(){
        
        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        ForwardSingleMove(occupiedTileCoord, direction);
        ForwardDoubleMove(occupiedTileCoord, direction);
        CaptureMove(occupiedTileCoord, direction);
        
        return coordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){
        
        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
                                
        //Capture Right
        Vector2 targetCoord  = new Vector2(occupiedTileCoord.x + direction, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){                                
            coordinates.Add(targetCoord);                        
        }
        
        //Capture Left
        targetCoord  = new Vector2(occupiedTileCoord.x - direction, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            coordinates.Add(targetCoord);            
        }                   

        return coordinates;
    }

    

    private void ForwardSingleMove(Vector2 occupiedTileCoord, int direction){
        Vector2 targetCoord = new Vector2(0,0);
        int step = 1 * direction;        
        
        if(occupiedTileCoord.y < BoardManager.Instance.BoardData.rowCount){
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                return;            
            
            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!ForwardSingleMoveRule(targetTile))
                return;
        }
        
    }

    private void ForwardDoubleMove(Vector2 occupiedTileCoord, int direction){
        if(!isFirstMove)
            return;

        Vector2 targetCoord = new Vector2(0,0);

        int step = 1 * direction;        
        for (int i = (int) occupiedTileCoord.y; i < occupiedTileCoord.y + 2; i++)
        {            
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y + step);            
            Tile targetTile = null;            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            Debug.Log(targetCoord + " " + step);

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!ForwardSingleMoveRule(targetTile))
                break;
                       
            step += direction;
        }
        
    }

    private void CaptureMove(Vector2 occupiedTileCoord, int direction){                
        
        Tile targetTile = null;

        //Capture Right
        Vector2 targetCoord  = new Vector2(occupiedTileCoord.x + direction, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){                                
            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];            
            CaptureRule(targetTile);
        }

        
        //Capture Left
        targetCoord  = new Vector2(occupiedTileCoord.x - direction, occupiedTileCoord.y + direction);                                    
        if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
            return;                            
        targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
        CaptureRule(targetTile);                
        
    }

    private bool CaptureRule(Tile tile)
    {
        
        if(tile.CurrentPiece() == null){            
            return false;
        }

        if(tile.CurrentPiece().GetPieceTeam() == team)
            return false;

        coordinates.Add(tile.GetCoordinate());
        return true;            
        
    }

    private bool ForwardSingleMoveRule(Tile tile)
    {
        
        if(tile.CurrentPiece() != null){            
            return false;
        }

        coordinates.Add(tile.GetCoordinate());
        return true;            
        
    }

    public override void TryOccupiesTile(Tile targetTile){
        if(targetTile == null){
            Debug.Log("No Tile to Occupy");    
            transform.position = occupiedTile.transform.position;
            return;
        }

        bool isOnPatternTile = GetLegalTileCoordinates().Contains(targetTile.GetCoordinate());        

        if(!isOnPatternTile){
            Debug.Log("Target Tile is not On Pattern");    
            transform.position = occupiedTile.transform.position;
            return;
        }

        occupiedTile.SetCurrentPiece(null);
        King king  = PieceSpawner.Instance.GetTeamKing((int)team).GetComponent<King>();        
        if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate()) && king.IsUnSafeMove(targetTile.GetCoordinate())){
            Debug.Log("You leave The King Unprotected");    
            transform.position = occupiedTile.transform.position;            
            occupiedTile.SetCurrentPiece(this);
            return;
        }        
        occupiedTile.SetCurrentPiece(this);

        isFirstMove = false;

        base.OccupiesTile(targetTile);
                
    }
}

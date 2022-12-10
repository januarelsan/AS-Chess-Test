using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool isFirstMove = true;
    private bool isEnpassantable;
    
    public List<EnpassantTarget> enpassantTargets = new List<EnpassantTarget>();      

    public override List<Vector2> GetLegalTileCoordinates(){
        
        tileCoordinates = new List<Vector2>();  
        enpassantTargets = new List<EnpassantTarget>();     
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        ForwardSingleMove(occupiedTileCoord, direction);
        ForwardDoubleMove(occupiedTileCoord, direction);
        CaptureMove(occupiedTileCoord, direction);
        EnpassantMove(occupiedTileCoord, direction);
        
        return tileCoordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){
        
        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
                                
        //Capture Right
        Vector2 targetCoord  = new Vector2(occupiedTileCoord.x + direction, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){                                
            tileCoordinates.Add(targetCoord);                        
        }
        
        //Capture Left
        targetCoord  = new Vector2(occupiedTileCoord.x - direction, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            tileCoordinates.Add(targetCoord);            
        }                   

        return tileCoordinates;
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
                        
            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!ForwardSingleMoveRule(targetTile,true))
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

    private void EnpassantMove(Vector2 occupiedTileCoord, int direction){                
        
        Tile checkTile = null;

        //Enpassant Right
        Vector2 checkCoord  = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y);                                    
        Vector2 targetCoord  = new Vector2(occupiedTileCoord.x + 1, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(checkCoord)){                                
            checkTile = BoardManager.Instance.GetTileDic()[checkCoord];                        
            EnpassantRule(checkTile, targetCoord);
        }

        
        //Enpassant Left
        checkCoord  = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y);                                    
        targetCoord  = new Vector2(occupiedTileCoord.x - 1, occupiedTileCoord.y + direction);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(checkCoord)){                                
            checkTile = BoardManager.Instance.GetTileDic()[checkCoord];                               
            EnpassantRule(checkTile, targetCoord);
        }
        
    }

    private bool CaptureRule(Tile tile)
    {
        
        if(tile.CurrentPiece() == null){            
            return false;
        }

        if(tile.CurrentPiece().GetPieceTeam() == team)
            return false;

        tileCoordinates.Add(tile.GetCoordinate());
        return true;            
        
    }

    private bool EnpassantRule(Tile checkTile, Vector2 targetCoord)
    {
        
        if(checkTile.CurrentPiece() == null){            
            return false;
        }

        if(checkTile.CurrentPiece().GetPieceTeam() == team)
            return false;

        if(checkTile.CurrentPiece().GetPieceType() != Type.Pawn)
            return false;

        if(!checkTile.CurrentPiece().GetComponent<Pawn>().isEnpassantable)            
            return false;

        tileCoordinates.Add(targetCoord);
        enpassantTargets.Add(new EnpassantTarget(targetCoord, checkTile.CurrentPiece()));
        
        return true;            
        
    }
    private bool ForwardSingleMoveRule(Tile tile, bool testDoubleMove = false)
    {
        if(testDoubleMove)
            return false;

        if(tile.CurrentPiece() != null){            
            return false;
        }

        tileCoordinates.Add(tile.GetCoordinate());
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

        if(LeavingKingUprotected(targetTile))
            return;

        isFirstMove = false;

        isEnpassantable = IsEnpasantableMove(targetTile);

        DoEnpassantMove(targetTile.GetCoordinate());

        Tile lastTile = occupiedTile;
        
        base.OccupiesTile(targetTile);
                
        DoPromotionMove(targetTile, lastTile);
    }

    bool LeavingKingUprotected(Tile targetTile){
        occupiedTile.SetCurrentPiece(null);
        King king  = PieceSpawner.Instance.GetTeamKing((int)team).GetComponent<King>();        
        if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){
            Debug.Log("The King Unprotected");  
            targetTile.SetCurrentPiece(this, true);
            
            if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){
                Debug.Log("You leave The King Unprotected");    
                transform.position = occupiedTile.transform.position;            
                occupiedTile.SetCurrentPiece(this);
                targetTile.SetCurrentPiece(null);
                targetTile.RemoveTempPiece();
                return true;
            }
            Debug.Log("You Protect The King");  
            targetTile.RemoveTempPiece();
        }        
        occupiedTile.SetCurrentPiece(this);

        return false;
    }


    void DoPromotionMove(Tile targetTile, Tile lastTile){
        
        if(team == Team.White){
            
            if((int) targetTile.GetCoordinate().y != BoardManager.Instance.BoardData.rowCount - 1)
                return;
                        
            PromoteManager.Instance.PromotePawn(this, lastTile);        
            
        } else {
                        
            if((int) targetTile.GetCoordinate().y != 0)
                return;
                        
            PromoteManager.Instance.PromotePawn(this, lastTile);                
        }


    }

    void DoEnpassantMove(Vector2 targetCoord){
        EnpassantTarget enpassantTarget = enpassantTargets.Find(
            delegate(EnpassantTarget enpassantTarget)
            {
                return enpassantTarget.TargetCoord() == targetCoord;                
            });

        if(enpassantTarget == null)
            return;

        Debug.Log("Do Enpassant Move");

        enpassantTarget.TargetPiece().GetOccupiedTile().SetCurrentPiece(null);
        enpassantTarget.TargetPiece().Dead();
    }

    public void RemoveEnpassantable(){
        isEnpassantable = false;
    }

    bool IsEnpasantableMove(Tile targetTile){
        int direction = (team == 0) ? -1 : 1;

        if( (targetTile.GetCoordinate().y + direction * 2) == occupiedTile.GetCoordinate().y){
            //Check is there pawn on the left
            Vector2 targetCoord  = new Vector2(targetTile.GetCoordinate().x - 1, targetTile.GetCoordinate().y);                                    
            if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){                                
                Tile leftTargetTile = BoardManager.Instance.GetTileDic()[targetCoord];            
                if(leftTargetTile.IsOccupied())
                    return leftTargetTile.CurrentPiece().GetPieceType() == Type.Pawn;

            }

            //Check is there pawn on the right
            targetCoord  = new Vector2(targetTile.GetCoordinate().x + 1, targetTile.GetCoordinate().y);                                    
            if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){                                
                Tile rightTargetTile = BoardManager.Instance.GetTileDic()[targetCoord];            
                if(rightTargetTile.IsOccupied())
                    return rightTargetTile.CurrentPiece().GetPieceType() == Type.Pawn;

            }

            return false;
        }
        
        return false;
    }

    
}

public class EnpassantTarget {

    Vector2 targetCoord;
    Piece targetPiece;
    public EnpassantTarget(Vector2 targetCoord, Piece targetPiece){
        this.targetCoord = targetCoord;
        this.targetPiece = targetPiece;
    }

    public Vector2 TargetCoord(){
        return targetCoord;
    }

    public Piece TargetPiece(){
        return targetPiece;
    }

    
    
}

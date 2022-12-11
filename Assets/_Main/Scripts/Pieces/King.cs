using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    
    private List<CastlingRook> availCastlingRooks = new List<CastlingRook>();

    private bool canCastling;
    public override List<Vector2> GetLegalTileCoordinates(){
        
        isCheckProtectedTile = false;

        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        SingleMove(occupiedTileCoord);
        CastlingMove(occupiedTileCoord);
       
        return tileCoordinates;
    }

    public override List<Vector2> GetProtectedTileCoordinates(){
        
        isCheckProtectedTile = true;
        
        tileCoordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        SingleMove(occupiedTileCoord);
       
        return tileCoordinates;
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

    private void CastlingMove(Vector2 occupiedTileCoord){        
        
        Vector2 targetCoord = new Vector2(0,0);
                
        // Castling Right
        targetCoord = new Vector2(occupiedTileCoord.x + 2, occupiedTileCoord.y);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            CastlingRule(targetTile,1);
        }

        // Castling Left
        targetCoord = new Vector2(occupiedTileCoord.x - 2, occupiedTileCoord.y);                                    
        if(BoardManager.Instance.GetTileDic().ContainsKey(targetCoord)){
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];        
            CastlingRule(targetTile,-1);
        }
                
    }

    private bool CastlingRule(Tile tile, int wayDirection)
    {
        if(hasMoved)
            return false;
            
        if(IsUnSafeMove(GetOccupiedTile().GetCoordinate())){
            return false;
        }
        
        if(tile.CurrentPiece() != null){            
            return false;
        }

        int step = wayDirection;        
        
        for (int i = 0; i < BoardManager.Instance.BoardData.colCount; i++)
        {
            
            Vector2 targetCoord = new Vector2(occupiedTile.GetCoordinate().x + step, occupiedTile.GetCoordinate().y);            
                        
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
                    
            Tile targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            step += wayDirection;

            int status = CastlingWayTileRule(targetTile, wayDirection);
            
            if(status == 0) //Ilegal
                break;

            if(status == 1) // Legal
                continue;

            if(status == 2){ // Rook Found                
                tileCoordinates.Add(tile.GetCoordinate());
                break;
            }
                       
        }
        
        return true;            
        
    }

    private int CastlingWayTileRule(Tile tile, int wayDirection)
    {
        
        if(tile.CurrentPiece() == null){            
            if(IsUnSafeMove(tile.GetCoordinate()))
                return 0;

            return 1;
        }        

        if(tile.CurrentPiece().GetPieceType() != Type.Rook)
            return 0;

        if(tile.CurrentPiece().GetPieceTeam() != team)
            return 0;

        if(tile.CurrentPiece().HasMoved())
            return 0;

        CastlingRook castlingRook = new CastlingRook(tile.CurrentPiece().GetComponent<Rook>(), wayDirection);
        availCastlingRooks.Add(castlingRook);

        return 2;            
        
    }
    
    
        
    public override float EvaluateTryOccupiesTile(Tile targetTile){
        
        float score = Random.Range(0f,1f);

        //Evaluate if target tile is not null
        if(targetTile == null){                        
            return score = -1;
        }

        bool isLegalTile = GetLegalTileCoordinates().Contains(targetTile.GetCoordinate());        

        //Evaluate if this is legal tile
        if(!isLegalTile){                        
            return score = -1;
        }
        
        //Evaluate if target tile is unsafe
        if(IsUnSafeMove(targetTile.GetCoordinate())){                        
            return score = -1;
        }                

        //Evaluate if the king under threat
        if(IsUnSafeMove(occupiedTile.GetCoordinate()))
            score += 700;

        //Evaluate if can castling
        if( CanCastling(targetTile) ){            
            score += 1100;
        }

        //Evaluate if king not under threat
        if(!IsUnSafeMove(occupiedTile.GetCoordinate())){             
            if(PieceSpawner.Instance.GetTeamPieces((int)team).Count < 5)
                score += Random.Range(10, 1000);
            else
                score = 0.1f;
        }

        //Evaluate if occupies target tile will capture enemy piece
        if(targetTile.IsOccupied()){            
            if(!IsUnSafeMove(targetTile.GetCoordinate())){
                score += targetTile.CurrentPiece().GetValue() * 10;            
            } 
        }
        
        return score;        
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
            Debug.Log("Target Tile is not Safe: " + targetTile.GetCoordinate());    
            transform.position = occupiedTile.transform.position;            
            return;
        }
        
        hasMoved = true;

        DoCastlingMove(targetTile.GetCoordinate());

        base.OccupiesTile(targetTile);
    }

    bool CanCastling(Tile targetTile){
        if( (occupiedTile.GetCoordinate().x - 2) == targetTile.GetCoordinate().x || (occupiedTile.GetCoordinate().x + 2) == targetTile.GetCoordinate().x){            
            return true;
        }
        return false;
    }

    void DoCastlingMove(Vector2 targetCoord){

        if( (occupiedTile.GetCoordinate().x - 2) == targetCoord.x || (occupiedTile.GetCoordinate().x + 2) == targetCoord.x){            
            
            int direction = 1;
            
            if(occupiedTile.GetCoordinate().x > targetCoord.x){ //Castling left                
                direction = -1;
            }

            CastlingRook castlingRook = availCastlingRooks.Find(
            delegate(CastlingRook castlingRook)
            {
                return castlingRook.Direction() == direction;                
            });

            castlingRook.Rook().GetOccupiedTile().SetCurrentPiece(null); // Leave Tile
            Vector2 newTileCoord = new Vector2(targetCoord.x + direction * -1, targetCoord.y);
            Tile newTile = BoardManager.Instance.GetTileDic()[newTileCoord];
            castlingRook.Rook().SetOccupiedTile(newTile);
            newTile.SetCurrentPiece(castlingRook.Rook());
            castlingRook.Rook().transform.position = new Vector3(newTile.transform.position.x, 
                castlingRook.Rook().transform.position.y,
                newTile.transform.position.z);
                        
        }
            

    }

    public bool IsCheckmate(){
        int safeTileCount = 0;
        foreach (Vector2 legalTileCoordinate in GetLegalTileCoordinates())
        {
            
            if(IsUnSafeMove(legalTileCoordinate)){                
                transform.position = occupiedTile.transform.position;            
                continue;                
            }
            safeTileCount++;
        }

        bool canMove = safeTileCount != 0 && GetLegalTileCoordinates().Count != 0;
        
        bool canHelped = false;
        foreach (Piece piece in PieceSpawner.Instance.GetTeamPieces((int)team))
        {
            if(piece.GetPieceType() == Type.King)
                continue;
                        
            if(piece.IsCanProtectKing()){
                canHelped = true;
                break;
            }
        }
        

        //Check if Stalemate instead
        bool isOnCheck = IsUnSafeMove(occupiedTile.GetCoordinate());
        bool isCurrentTurn = GameController.Instance.TeamTurn() == (int)team;
        
        if(!canMove && !canHelped && !isOnCheck && isCurrentTurn){
            GameController.Instance.GameStalemate();
            return false;
        }
        //Check if Stalemate instead |
                
        return !canMove && !canHelped;
    }
    
}

public class CastlingRook {

    Rook rook;
    int direction;
    public CastlingRook(Rook rook, int direction){
        this.rook = rook;
        this.direction = direction;
    }

    public Rook Rook(){
        return rook;
    }

    public int Direction(){
        return direction;
    }

    
    
}

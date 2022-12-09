using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    
    private List<Vector2> coordinates = new List<Vector2>();    

    private List<CastlingRook> availCastlingRocks = new List<CastlingRook>();
    public override List<Vector2> GetLegalTileCoordinates(){
        
        isCheckProtectedTile = false;

        coordinates = new List<Vector2>();  
        
        int direction = (team == 0) ? 1 : -1;

        Vector2 occupiedTileCoord = GetOccupiedTile().GetCoordinate();
        
        
        //Register tiles        
        SingleMove(occupiedTileCoord);
        CastlingMove(occupiedTileCoord);
       
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

            Debug.Log(targetCoord.x + " - " + status);

            if(status == 0) //Ilegal
                break;

            if(status == 1) // Legal
                continue;

            if(status == 2){ // Rock Found
                Debug.Log("Rock Found");
                coordinates.Add(tile.GetCoordinate());
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

        CastlingRook castlingRook = new CastlingRook(tile.CurrentPiece().GetComponent<Rock>(), wayDirection);
        availCastlingRocks.Add(castlingRook);

        return 2;            
        
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
        
        hasMoved = true;

        DoCastlingMove(targetTile.GetCoordinate());

        base.OccupiesTile(targetTile);
    }

    void DoCastlingMove(Vector2 targetCoord){

        if( (occupiedTile.GetCoordinate().x - 2) == targetCoord.x || (occupiedTile.GetCoordinate().x + 2) == targetCoord.x){

            Debug.Log("Do Castling");

            int targetRock = 0; //right rock
            int direction = 1;
            
            if(occupiedTile.GetCoordinate().x > targetCoord.x){ //Castling left
                targetRock = 1;
                direction = -1;
            }

            CastlingRook castlingRook = availCastlingRocks.Find(
            delegate(CastlingRook castlingRook)
            {
                return castlingRook.Direction() == direction;                
            });

            castlingRook.Rock().GetOccupiedTile().SetCurrentPiece(null); // Leave Tile
            Vector2 newTileCoord = new Vector2(targetCoord.x + direction * -1, targetCoord.y);
            Tile newTile = BoardManager.Instance.GetTileDic()[newTileCoord];
            castlingRook.Rock().SetOccupiedTile(newTile);
            newTile.SetCurrentPiece(castlingRook.Rock());
            castlingRook.Rock().transform.position = new Vector3(newTile.transform.position.x, 
                castlingRook.Rock().transform.position.y,
                newTile.transform.position.z);
            
            Debug.Log("Finish Castling");
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
                        
            if(piece.IsCanProtectKing()){
                canHelped = true;
                break;
            }
        }
        
        return !canMove && !canHelped;
    }
    
}

public class CastlingRook {

    Rock rock;
    int direction;
    public CastlingRook(Rock rock, int direction){
        this.rock = rock;
        this.direction = direction;
    }

    public Rock Rock(){
        return rock;
    }

    public int Direction(){
        return direction;
    }

    
    
}

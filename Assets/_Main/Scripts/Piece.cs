using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public delegate void SelectPiece(Piece piece);
    public static event SelectPiece OnSelectPiece;
    public static event SelectPiece OnDeselectPiece;    

    public delegate void Occupy();
    public static event Occupy OnOccupies;
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
    
    protected bool isCheckProtectedTile;
    
    protected bool hasMoved;

    protected List<Vector2> tileCoordinates = new List<Vector2>();  

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

    public int GetValue(){
        int value = 0;
        switch ((int) type)
        {
            case 0: 
                value = 0;
                break;
            case 1: 
                value = 10;
                break;
            case 2: 
                value = 30;
                break;
            case 3: 
                value = 30;
                break;
            case 4: 
                value = 50;
                break;
            case 5: 
                value = 90;
                break;
            case 6: 
                value = 900;
                break;            
        }
        return value;
    }

    public void Select(){        
        OnSelectPiece(this);
    }

    public void Deselect(){
        OnDeselectPiece(this);
    }

    #region AI Evaluation
    public virtual float EvaluateTryOccupiesTile(Tile targetTile){
        
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

        //Evaluate if target tile will leave the king unprotected
        if(LeavingKingUprotected(targetTile))
            return score = -1;   
        
        //Evaluate if the king is threatened and target tile will protect the king
        if(IsUnSafeMove(PieceSpawner.Instance.GetTeamKing((int)team).occupiedTile.GetCoordinate())){
            if(CanProtectKing(targetTile)){
                if(IsUnSafeMove(targetTile.GetCoordinate()))
                    score += 600;   
                else
                    score += 800 - GetValue();   
            }
        }

        //Evaluate if threatened
        if(IsUnSafeMove(occupiedTile.GetCoordinate()))
            score += GetValue();
        
        //Evaluate if target tile is unsafe
        if(IsUnSafeMove(targetTile.GetCoordinate()))
            score -= GetValue() * 2;

        //Evaluate if occupies target tile will capture enemy piece
        if(targetTile.IsOccupied()){            
            if(!IsUnSafeMove(targetTile.GetCoordinate())){
                score += targetTile.CurrentPiece().GetValue() * 10;            
            } else {
                if(GetValue() > targetTile.CurrentPiece().GetValue())
                    score += (targetTile.CurrentPiece().GetValue()/2);
                else
                    score += targetTile.CurrentPiece().GetValue() * 5;            
            }

        }
        
        //Evaluate if target tile can threat enemy pieces        
        return score += EvaluateCanThreat(targetTile);
        
    }
    
    float EvaluateCanThreat(Tile targetTile){
        
        float addScore = 0;
        
        List<Piece> enemyPieces = PieceSpawner.Instance.GetTeamPieces((team == 0) ? 1 : 0);
        Tile lastTile = occupiedTile;
        Piece lastTargetTilePawn = targetTile.CurrentPiece();        
        bool targetTileIsOccupied = targetTile.IsOccupied();
                
        occupiedTile.SetCurrentPiece(null); // leave piece
        SetOccupiedTile(null); // leave tile

        if(targetTile.IsOccupied())
            targetTile.CurrentPiece().SetOccupiedTile(null); //target tile piece leave tile
        
        targetTile.SetCurrentPiece(this); // move to target tile
        SetOccupiedTile(targetTile); // move to target tile
        
        foreach (Piece ePiece in enemyPieces)
        {           
            
            if(targetTileIsOccupied){
                continue;
            }
            if(ePiece.IsUnSafeMove(ePiece.occupiedTile.GetCoordinate())){                
                
                if(IsUnSafeMove(occupiedTile.GetCoordinate())){                    
                    if(ePiece.GetValue() < GetValue())
                        addScore -= 1000;
                    else
                        addScore += ePiece.GetValue() / 3;
                } else {
                    if(ePiece.GetPieceType() == Type.King){
                        addScore += ePiece.GetValue() / 2;
                    } else {
                        addScore += ePiece.GetValue();
                    }
                }                                
            }
            
            
        }

        SetOccupiedTile(lastTile); //back to original tile
        
        lastTile.SetCurrentPiece(this);

        if(targetTileIsOccupied){
            lastTargetTilePawn.SetOccupiedTile(targetTile);
            targetTile.SetCurrentPiece(lastTargetTilePawn);
        } else {
            targetTile.SetCurrentPiece(null);
        }
        return addScore;
    }

    #endregion

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

        if(LeavingKingUprotected(targetTile))
            return;
        
        hasMoved = true;

        OccupiesTile(targetTile);
    }

    public void OccupiesTile(Tile targetTile){
        
        //Capture Enemy Piece
        if(targetTile.CurrentPiece() != null){
            
            targetTile.CurrentPiece().Dead();
            
        }

        //Leave Last Tile
        occupiedTile.SetCurrentPiece(null);

        //Set New Occupied Tile
        occupiedTile = targetTile;        
        occupiedTile.SetCurrentPiece(this);
        transform.position = occupiedTile.transform.position;

        OnOccupies();
    }

    
    public void Dead(){
        occupiedTile = null;
        gameObject.SetActive(false);
        GameController.Instance.AddDeadPiece(this);
    }
    
    public Tile GetOccupiedTile(){
        return occupiedTile;
    }

    public void SetOccupiedTile(Tile tile){
        occupiedTile = tile;
    }

    public bool HasMoved(){
        return hasMoved;
    }

    public virtual List<Vector2> GetLegalTileCoordinates(){
        
        List<Vector2> coordinates = new List<Vector2>();                
        return coordinates;
    }
    
    public virtual List<Vector2> GetProtectedTileCoordinates(){
        
        List<Vector2> coordinates = new List<Vector2>();                
        return coordinates;
    }


    #region Common Moves

    protected void ForwardLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.y; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y + step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    protected void BackwardLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.y; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x, occupiedTileCoord.y - step);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    protected void RightLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i < BoardManager.Instance.BoardData.rowCount; i++)
        {
            targetCoord = new Vector2(occupiedTileCoord.x + step, occupiedTileCoord.y);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    protected void LeftLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
        int step = 1;        
        
        for (int i = (int) occupiedTileCoord.x; i > 0; i--)
        {
            targetCoord = new Vector2(occupiedTileCoord.x - step, occupiedTileCoord.y);            
            Tile targetTile = null;
            
            if(!BoardManager.Instance.GetTileDic().ContainsKey(targetCoord))
                break;            
            
            

            targetTile = BoardManager.Instance.GetTileDic()[targetCoord];
            
            if(!BasicLegalTileRule(targetTile))
                break;
                       
            step++;
        }
    }
    protected void RightForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
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
    protected void LeftForwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
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
    protected void RightBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
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
    protected void LeftBackwardDiagLongMove(Vector2 occupiedTileCoord, Vector2 targetCoord){
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
    protected bool BasicLegalTileRule(Tile tile)
    {
        
        if(tile.CurrentPiece() != null){
            if(tile.CurrentPiece().GetPieceTeam() == GetPieceTeam()){
                if(isCheckProtectedTile)
                    tileCoordinates.Add(tile.GetCoordinate());
                
                return false;

            } else {

                tileCoordinates.Add(tile.GetCoordinate());
                return isCheckProtectedTile && tile.CurrentPiece().GetPieceType() == Type.King;

            }
        }

        tileCoordinates.Add(tile.GetCoordinate());
        return true;            
        
    }
    
    #endregion

    #region Special Moves & Rules

    bool LeavingKingUprotected(Tile targetTile){
        occupiedTile.SetCurrentPiece(null);
        King king  = PieceSpawner.Instance.GetTeamKing((int)team).GetComponent<King>();        
        if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){
            // Debug.Log("The King Unprotected");  
            targetTile.SetCurrentPiece(this, true);
            
            if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){
                // Debug.Log("You leave The King Unprotected");    
                transform.position = occupiedTile.transform.position;            
                occupiedTile.SetCurrentPiece(this);
                targetTile.SetCurrentPiece(null);
                targetTile.RemoveTempPiece();
                return true;
            }
            // Debug.Log("You Protect The King");  
            targetTile.RemoveTempPiece();
        }        
        occupiedTile.SetCurrentPiece(this);

        return false;
    }

    bool CanProtectKing(Tile targetTile){
        occupiedTile.SetCurrentPiece(null);
        King king  = PieceSpawner.Instance.GetTeamKing((int)team).GetComponent<King>();        
        if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){            
            targetTile.SetCurrentPiece(this, true);
            
            if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){                
                transform.position = occupiedTile.transform.position;            
                occupiedTile.SetCurrentPiece(this);
                targetTile.SetCurrentPiece(null);
                targetTile.RemoveTempPiece();
                return false;
            }
            // Debug.Log("You Protect The King");  
            targetTile.RemoveTempPiece();
            occupiedTile.SetCurrentPiece(this);
            return true;
        }        
        occupiedTile.SetCurrentPiece(this);

        return false;
    }

    public bool IsCanProtectKing(){        

        int safeTileCount = 0;
        foreach (Vector2 legalTileCoordinate in GetLegalTileCoordinates())
        {
            
            occupiedTile.SetCurrentPiece(null);
            King king  = PieceSpawner.Instance.GetTeamKing((int)team).GetComponent<King>();        
            if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){                
                
                Tile targetTile = BoardManager.Instance.GetTileDic()[legalTileCoordinate];
                
                targetTile.SetCurrentPiece(this, true);
                
                if(king.IsUnSafeMove(king.GetOccupiedTile().GetCoordinate())){
                    
                    transform.position = occupiedTile.transform.position;            
                    occupiedTile.SetCurrentPiece(this);
                    targetTile.SetCurrentPiece(null);
                    targetTile.RemoveTempPiece();
                    continue;
                }
                
                targetTile.RemoveTempPiece();
            }        
            occupiedTile.SetCurrentPiece(this);
            safeTileCount++;
        }
        
        return safeTileCount > 0;
    }

    public bool IsUnSafeMove(Vector2 targetTile){
        int opsTeam = ((int) team == 1) ? 0 : 1;
        return PieceSpawner.Instance.GetTeamPieces(opsTeam).Find(
            delegate(Piece piece)
            {
                return piece.GetProtectedTileCoordinates().Contains(targetTile);                
            });
        
    }

    #endregion

    

}

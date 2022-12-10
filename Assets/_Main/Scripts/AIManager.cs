using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    private int aiTeam;

    public List<Piece> lastEvaluatedPieces = new List<Piece>();
    public List<Tile> lastEvaluatedTiles = new List<Tile>();
    public List<float> lastEvaluatedScores = new List<float>();
    private void OnEnable()
    {
        GameController.OnChangeTurn += CheckTurn;                    
        PieceSpawner.OnFinishSpawnPieces += Setup;        
    }

    private void OnDisable() {
        GameController.OnChangeTurn -= CheckTurn;         
        PieceSpawner.OnFinishSpawnPieces -= Setup;                           
    }

    // Start is called before the first frame update
    void Setup()
    {
        aiTeam = (GameController.Instance.PlayerTeam() == 0) ? 1 : 0;
        // aiTeam = 2;
        CheckTurn(GameController.Instance.TeamTurn());
        
    }

    

    void CheckTurn(int team){
        // Debug.Log("AI Turn " + (aiTeam == GameController.Instance.TeamTurn()));
        
        if(aiTeam != GameController.Instance.TeamTurn())
            return;

        MovePiece(team);
    }

    void MovePiece(int team){
        // Debug.Log("AI Move Piece " + team);
        
        StartCoroutine(MovePieceIE(team));
    }

    IEnumerator MovePieceIE(int team){
        yield return new WaitForEndOfFrame();
        EvaluatedTile highestEvaluatedTile = new EvaluatedTile(null,null, -1);
        
        lastEvaluatedPieces = new List<Piece>();
        lastEvaluatedTiles = new List<Tile>();
        lastEvaluatedScores = new List<float>();

        for (int i = 0; i < PieceSpawner.Instance.GetTeamPieces(team).Count; i++)
        {            
            Piece piece = PieceSpawner.Instance.GetTeamPieces(team)[i];
            foreach (Vector2 legalTileCoord in piece.GetLegalTileCoordinates())
            {            
                Tile targetTile = BoardManager.Instance.GetTileDic()[legalTileCoord];
                
                float evaluatedScore = piece.EvaluateTryOccupiesTile(targetTile);
                // Debug.Log(piece.name + " " + targetTile.name + " Score: " + evaluatedScore);            

                EvaluatedTile evaluatedTile = new EvaluatedTile(piece, targetTile, evaluatedScore);
                
                if(evaluatedScore == -1)
                    continue;

                if(evaluatedScore >= highestEvaluatedTile.GetScore())
                    highestEvaluatedTile = evaluatedTile;            
            }
        }

        // lastEvaluatedPieces.Add(highestEvaluatedTile.GetPieceToMove());
        // lastEvaluatedTiles.Add(highestEvaluatedTile.GetTile());
        // lastEvaluatedScores.Add(highestEvaluatedTile.GetScore());
        
        if(highestEvaluatedTile.GetPieceToMove() == null){
            Debug.Log("No more evaluated tile");
            yield break;
        }
        Debug.Log(highestEvaluatedTile.GetPieceToMove().name + " " + highestEvaluatedTile.GetTile().name + " Score: " + highestEvaluatedTile.GetScore());            
        highestEvaluatedTile.GetPieceToMove().TryOccupiesTile(highestEvaluatedTile.GetTile());
        
        
        
    }

    public int GetAITeam(){
        return aiTeam;
    }
}

public class EvaluatedTile{
    private Piece pieceToMove;
    private Tile tile;
    private float score;

    public EvaluatedTile(Piece pieceToMove, Tile tile, float score){
        this.pieceToMove = pieceToMove;
        this.tile = tile;
        this.score = score;
    }

    public Piece GetPieceToMove(){
        return pieceToMove;
    }

    public Tile GetTile(){
        return tile;
    }

    public float GetScore(){
        return score;
    }

}
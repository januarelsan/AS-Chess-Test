using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void HoverTile(Tile tile);
    public static event HoverTile OnHoverTile;
    public static event HoverTile OnHoverExitTile;

    [SerializeField] private Material whiteTileMat;
    [SerializeField] private Material blackTileMat;
    [SerializeField] private Material hoverTileMat;
    [SerializeField] private MeshRenderer meshRenderer;
    private Vector3 coordinate;
    private Piece currentPiece;
    private Piece backupTempPiece;
    public void Setup(int x, int y){
        coordinate = new Vector3(x,0,y);
        name = GetName();
        meshRenderer.material = GetMaterial();
    }

    void OnMouseOver()
    {                
        meshRenderer.material = hoverTileMat;
        OnHoverTile(this);
    }
    
    void OnMouseExit()
    {        
        meshRenderer.material = GetMaterial();
        OnHoverExitTile(this);
    }

    void OnMouseDown() {
        if(IsOccupied()){            
            CurrentPiece().Select();
        }
    }

    void OnMouseUp() {
        if(IsOccupied()){            
            CurrentPiece().Deselect();
        }
    }

    private Material GetMaterial(){
        bool isTileEven = coordinate.x % 2 == 0 && coordinate.z % 2 == 0;
        bool isTileOdd = coordinate.x % 2 == 1 && coordinate.z % 2 == 1;
        
        if(isTileEven || isTileOdd){
            return blackTileMat;
        }

        return whiteTileMat;
    }

    private string GetName()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var colName = "";

        if (coordinate.x >= letters.Length)
            colName += letters[(int) coordinate.x / letters.Length - 1];

        colName += letters[(int) coordinate.x % letters.Length];

        string rowName = (coordinate.z + 1).ToString();

        return colName + rowName + string.Format(" ({0},{1})", coordinate.x, coordinate.z);
    }

    public void SetCurrentPiece(Piece currentPiece, bool isTemporary = false){
        if(isTemporary){
            backupTempPiece = this.currentPiece;
            if(CurrentPiece())
                CurrentPiece().gameObject.SetActive(false);
        }

        this.currentPiece = currentPiece;
    }

    public void RemoveTempPiece(){        
        this.currentPiece = backupTempPiece;
        
        if(CurrentPiece())
            CurrentPiece().gameObject.SetActive(true);
    }

    public Piece CurrentPiece(){
        return currentPiece;
    }
    public bool IsOccupied(){
        return currentPiece != null;
    }

    public Vector2 GetCoordinate(){
        return new Vector2(coordinate.x, coordinate.z);
    }
}

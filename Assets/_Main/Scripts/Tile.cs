using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material whiteTileMat;
    [SerializeField] private Material blackTileMat;
    [SerializeField] private Material hoverTileMat;
    [SerializeField] private MeshRenderer meshRenderer;
    private Vector3 coordinate;
    public void Setup(int x, int y){
        coordinate = new Vector3(x,0,y);
        name = GetName();
        meshRenderer.material = GetMaterial();
    }

    void OnMouseOver()
    {        
        // Debug.Log(string.Format("Mouse is over GameObject {0}.",name));
        meshRenderer.material = hoverTileMat;
    }

    void OnMouseExit()
    {        
        meshRenderer.material = GetMaterial();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool isSelectingPiece;
    private Piece selectedPiece;

    private void OnEnable()
    {
        Piece.OnSelectPiece += SelectPiece;        
        Piece.OnDeselectPiece += DeselectPiece;        
    }

    private void OnDisable() {
        Piece.OnSelectPiece -= SelectPiece;        
        Piece.OnDeselectPiece -= DeselectPiece;        
        
    }

    public void SelectPiece(Piece piece){
        if(isSelectingPiece){
            Debug.Log("Still Selecting a Piece");
            return;
        } 

        Debug.Log("GC Select: " + piece.name);
        selectedPiece = piece;
        isSelectingPiece = true;
    }

    public void DeselectPiece(Piece piece){
        if(!isSelectingPiece){
            Debug.Log("No Selected Piece");
            return;
        } 
        
        Debug.Log("GC Deselect: " + piece.name);
        selectedPiece = null;
        isSelectingPiece = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

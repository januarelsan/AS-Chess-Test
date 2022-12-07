using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private Mesh[] typeMeshes;
    [SerializeField] private Material[] teamMaterials;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

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
    private Team team;

    public void Setup(int type, int team){
        this.type = (Type) type;
        this.team = (Team) team;        

        meshFilter.mesh = typeMeshes[type - 1];
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
    
}

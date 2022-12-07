using System;

[Serializable]
public class TilePiece {           
    
    public int type;
    public int team;

    public TilePiece(int type, int team){
        this.type = type;
        this.team = team;
    }
    
}

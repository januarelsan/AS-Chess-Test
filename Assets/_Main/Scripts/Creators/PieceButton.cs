using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite[] iconSprites;

    private enum Type{
        Undifinied = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }

    private enum Team{
        White = 0,
        Black = 1
    }
    
    private Type type = 0;
    private Team team = 0;

    private float changeTeamTime;
    private bool isChangeTeamTimeRun;

    public void Setup(int type, int team){
        this.type = (Type) type;
        this.team = (Team) team;

        if((int) this.type == 0){            
            iconImage.enabled = false;
            return;
        } 
        
        iconImage.enabled = true;

        if(team == 0){                
            iconImage.sprite = iconSprites[(int) type];
        } else {
            iconImage.sprite = iconSprites[(int) type + 7];                
        }

    }

    public void ChangeType(){
        if((int) type > 5){
            type = (Type) 0;            
            iconImage.enabled = false;
        } else{
            type++;
            iconImage.enabled = true;

            if(team == 0){                
                iconImage.sprite = iconSprites[(int) type];
            } else {
                iconImage.sprite = iconSprites[(int) type + 7];                
            }
            
        }

    }

    void Update(){
        if(isChangeTeamTimeRun){
            changeTeamTime += Time.deltaTime;
            if(changeTeamTime > 1){
                ChangeTeam();
                changeTeamTime = 0;
            }
        }

    }

    void ChangeTeam(){
        if(team == 0){
            team = (Team) 1;
            iconImage.sprite = iconSprites[(int) type + 7];
        } else {
            team = (Team) 0;
            iconImage.sprite = iconSprites[(int) type];
        }
    }

    public void SetIsChangeTeamTimeRun(bool value){
        changeTeamTime = 0;
        isChangeTeamTimeRun = value;
    }

    public int GetTeamInt(){
        return (int) team;
    }

    public int GetTypeInt(){
        return (int) type;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    private GameSettingData gameSettingData = new GameSettingData();   
    private const string gameSettingDataFilename = "GameSettingData";

    public void SelectPlayerOneTeam(int team){
        gameSettingData.playerOneTeam = team;
    }

    public void SelectGameMode(int mode){
        gameSettingData.mode = mode;
    }

    public void Save(){
        SaveLoadJSON.Instance.SaveIntoJsonFile(gameSettingData, gameSettingDataFilename);
    }
}

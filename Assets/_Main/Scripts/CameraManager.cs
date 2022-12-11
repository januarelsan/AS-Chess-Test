using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private GameObject[] teamCameras;

    private void OnEnable()
    {
        GameController.OnChangeTurn += ChangeTurn;                    
        
    }

    private void OnDisable() {
        GameController.OnChangeTurn -= ChangeTurn;         
        
    }

    public void ChangeTurn(int team)
    {
        StartCoroutine(ChangeTurnIE(team));
    }

    IEnumerator ChangeTurnIE(int team){
        yield return new WaitForSeconds(0.5f);
        
        teamCameras[0].SetActive(team == 0);
        teamCameras[1].SetActive(team == 1);
    }

}

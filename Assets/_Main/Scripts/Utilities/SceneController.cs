using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GoTo(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

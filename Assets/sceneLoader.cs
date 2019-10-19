using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public void loadSceneNormal()
    {
        SceneManager.LoadScene("MainGame");
        StaticData.gameModeNormal = true;
    }

    public void loadSceneBoost()
    {
        SceneManager.LoadScene("MainGame");
        StaticData.gameModeNormal = false;
    }
}

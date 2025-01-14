using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneActions : MonoBehaviour
{

    string gameScene = "";
    string sceneName = "";

    public void StartGameScene()
    {

        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;

        gameScene = "comGame";

        if (sceneName == "comGame")
        {
            gameScene = "comGameWin";
        }

        if ( sceneName == "comGameWin")
        {
            gameScene = "comUnscrambleInfo";
        }

        if (sceneName == "comUnscrambleInfo")
        {
            gameScene = "comUnscrambleIntro";
        }

        if (sceneName == "comUnscrambleIntro")
        {
            gameScene = "comUnscramble";
        }

        if (sceneName == "comUnscramble")
        {
            gameScene = "Hub";
        }

        //Debug.Log(gameScene);
        SceneManager.LoadScene(gameScene);


    }



}

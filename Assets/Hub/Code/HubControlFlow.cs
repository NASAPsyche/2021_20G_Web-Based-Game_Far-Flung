using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DialogMaker;

public class HubControlFlow : MonoBehaviour
{
    int sceneNum;
    DialogGenerator diagIntro;
    DialogGenerator diagFlight;
    DialogGenerator diagScan;
    DialogGenerator diagComms;
    DialogGenerator diagLab;
    DialogGenerator diagOutro;
    AudioSource bgAudio;
    FadeController fader;
    GameObject mainMenu;
    bool introStarted;
    // Start is called before the first frame update
    void Start()
    {
        diagIntro = GameObject.Find("DialogIntro").GetComponent<DialogGenerator>();

        diagFlight = GameObject.Find("DialogFlight").GetComponent<DialogGenerator>();
        diagScan = GameObject.Find("DialogScan").GetComponent<DialogGenerator>();
        diagComms = GameObject.Find("DialogComms").GetComponent<DialogGenerator>();
        diagLab = GameObject.Find("DialogLab").GetComponent<DialogGenerator>();
        diagOutro = GameObject.Find("DialogOutro").GetComponent<DialogGenerator>();
        fader = GameObject.Find("FadeController").GetComponent<FadeController>();
        //mainMenu = GameObject.Find("Canvas");
        //fader.Fade();
        introStarted = false;
        //if (HubTracker.LevelToLoad == 0)
        //{
        //    HubTracker.LevelToLoad++;
        //}

        bgAudio = GameObject.Find("Background Music").GetComponent<AudioSource>();

        bgAudio.volume = 0.5f;


        switch (HubTracker.LevelToLoad)
        {
            case 2:
                HideMainMenu();
                diagFlight.BeginPlayingDialog();
                break;
            case 3:
                HideMainMenu();
                diagScan.BeginPlayingDialog();
                break;
            case 4:
                HideMainMenu();
                diagScan.BeginPlayingDialog();
                break;
            case 5:
                HideMainMenu();
                diagScan.BeginPlayingDialog();
                break;
            case 6:
                HideMainMenu();
                diagOutro.BeginPlayingDialog();
                break;
            default:
                break;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (HubTracker.IntroStarted)
        {
            bgAudio.volume = 0.1f;
        }

        if (HubTracker.LevelToLoad >= 0)
        {
            //if (diagIntro.AllDialogComplete())
            if (diagIntro == null && HubTracker.LevelToLoad == 1)
            {
                //Scene scene = SceneManager.GetActiveScene();
                //sceneNum = scene.buildIndex;
                //SceneManager.LoadScene(HubTracker.LevelToLoad++ + 1);
                HubTracker.LevelToLoad++;
                SceneManager.LoadScene("Assembly 3d");
            }
            else if (diagFlight == null && HubTracker.LevelToLoad == 2)
            {
                HubTracker.LevelToLoad++;
                SceneManager.LoadScene("1_FlightpathIntro");
                
            }
            else if (diagScan == null && HubTracker.LevelToLoad == 3)
            {
                HubTracker.LevelToLoad++;
                SceneManager.LoadScene("StartScene.Scanning");
            }
            else if (diagComms == null && HubTracker.LevelToLoad == 4)
            {
                HubTracker.LevelToLoad++;
                SceneManager.LoadScene("comGameIntro");
            }
            else if (diagLab == null && HubTracker.LevelToLoad == 5)
            {
                HubTracker.LevelToLoad++;
                SceneManager.LoadScene("scene5");
            }
            else if (diagOutro == null && HubTracker.LevelToLoad == 6)
            {
                //HubTracker.LevelToLoad++;
                //SceneManager.LoadScene("scene5");
            }
        }
    }

    public void Fade()
    {
        fader.Fade();
    }

    public void StartIntro()
    {
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        //diagIntro = GameObject.Find("DialogIntro").GetComponent<DialogGenerator>();
        Fade();
        yield return new WaitForSeconds(2);
        diagIntro.BeginPlayingDialog();
        HubTracker.LevelToLoad = 1;
        HubTracker.IntroStarted = true;
        HideMainMenu();
        //mainMenu.SetActive(false);
        yield return new WaitForSeconds(2);
        fader.ResetAndFade();
    }

    IEnumerator delay(int amount)
    {
        yield return new WaitForSeconds(amount);
    }

    public void HideMainMenu()
    {
        GameObject.Find("UIBackground").SetActive(false);
        GameObject.Find("PlayButton").SetActive(false);
        GameObject.Find("FarFlungLogoImg").SetActive(false);
    }
}

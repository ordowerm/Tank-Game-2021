using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Manages camera scaling for UI, as well as some other stuff, potentially.

 All of this can be accomplished through an animation controller/animations,
 but manually editing the values in the animation clips, key-by-key, value-by-value
 is pretty cumbersome.
 
 */
public class LevelUIManager : MonoBehaviour
{
    public bool debug;
    const string TAG = "LevelUIManager: ";

    //Display preferences
    public float topPaneHeight; //proportion of screen covered by the top UI panel
    public float mainCameraHeight; //proportion of screen covered by the main game, when the message panel is not visible 
    public float messageCamHeight; //for displaying text on bottom of screen
    public float targetAspect; //target aspect ratio --> UI is designed to match 1:1, so it might be a good idea to stick to this

    //References to individual cameras
    public Camera windowCamera; //To force a fixed aspect ratio, we have a camera covering the whole screen with a solid black color skybox. This is used for letterboxing.
    public Camera topCamera; //The top camera should display the HUD with player name, health, weapon, etc.
    public Camera mainCamera; //This camera should display the onscreen action
    public Camera bottomCamera; //This camera should display the Canvas containing miscellaneous onscreen messages like textboxes

    //Prefabs
    public GameObject playerPaneTemplate; //reference to a player panel for displaying in the HUD at the top of the screen
    public GameObject timerPaneTemplate;

    //Runtime variables:
    float screenAspect; //current aspect ratio of the window camera




    public float UIWindowScaleTime; //time, in seconds, required to complete scaling
    public PlayerUIPaneMgmt[] playerPanes;
    public bool fixCameraAspectRatio; //adds borders to the main camera viewport to ensure fixed field of view
    public float targetCameraWidth;

    
    float animTimer = 0;
    bool messageVisible=false; //if this flag is true, then the animation should advance until animTimer reaches scaleTime

    public Text timerText;
    public void SetTimerText(float f)
    {
        timerText.text = string.Format("{0:0.0}", f);
    }
    public void SetTimerText(string s)
    {
        timerText.text = s;
    }
    public void ShowMessage(bool isTrue)
    {
        messageVisible = isTrue;
        animTimer = 0;
        Debug.Log("Message visible: " + messageVisible);
    }

    public void UpdateUIPaneWeaponDisplay(int playerId, BulletData bd)
    {
        if (playerId >= 0 && playerId < playerPanes.Length)
        {
            playerPanes[playerId].SetWeaponGraphic(bd);
        }
    }
              
    
    void AdjustMainCameraViewport()
    {
        float normalizedWidth = mainCamera.rect.height / (2.0f * mainCamera.orthographicSize) * targetCameraWidth;

    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            ShowMessage(!messageVisible);
        }


        if (
            animTimer < UIWindowScaleTime
            )
        {
            //advance animation timer 
            animTimer += Time.deltaTime;
            if (animTimer > UIWindowScaleTime) { animTimer = UIWindowScaleTime; }

            //linearly interpolate camera sizes
            float prop;
            
            if (messageVisible)
            {
                prop = animTimer / UIWindowScaleTime;
            }
            else
            {
                prop = 1 - (animTimer / UIWindowScaleTime);

            }

            mainCamera.rect = new Rect(0, prop * messageCamHeight, 1, (mainCameraHeight - prop * messageCamHeight));
            bottomCamera.rect = new Rect(0, 0, 1, prop * messageCamHeight);            
        }


        //AdjustMainCameraViewport();
    }


    private void Awake()
    {
        //avoid user-error from setting a bad value in the editor for the variable targetAspect
        if (targetAspect <= 0)
        {
            targetAspect = 1;
        }

        //start coroutines
        StartCoroutine(ResizeScreen());

    }

    // Start is called before the first frame update
    void Start()
    {
        if (UIWindowScaleTime == 0)
        {
            UIWindowScaleTime = 1;
        }
    }


    

    //Coroutine for resizing screen at fixed intervals
    IEnumerator ResizeScreen()
    {
        for (; ; )
        {
            //only rescale if there's a change in resolution
            if (screenAspect != windowCamera.aspect)
            {
                if (debug) { Debug.Log(TAG + "Resizing screen"); }

                if (windowCamera.aspect >= 1)
                {
                    mainCamera.rect = new Rect((1 - targetAspect / windowCamera.aspect) / 2.0f, 0, targetAspect / windowCamera.aspect, 1);
                }
                else
                {
                    mainCamera.rect = new Rect(0, (1 - targetAspect * windowCamera.aspect) / 2.0f, 1, targetAspect * windowCamera.aspect);
                }
                screenAspect = windowCamera.aspect;
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}

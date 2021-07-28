using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 This script should be placed in the Game UI prefab present in each level of "real" gameplay (as opposed to the title screen, etc.)

    It performs the following tasks:
    - runs a coroutine that arranges the cameras on screen to match a target aspect ratio. It also applies letterboxing
    - controls an animation in which the HUD slides offscreen, and a pane for displaying in-game dialogue appears
    - contains methods, to be called from the scene's LevelManager script, that in turn update the UI displayed in the HUD
 
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
    public GameObject sceneMessagePrefab;

    //Runtime variables:
    public LevelManager mgmt;
    public SceneOverlayMessageUIStateMachine sceneMessage;
    float screenAspect; //current aspect ratio of the window camera
    bool bottomPaneMessageVisible = false; //if this flag is true, then the animation should advance until animTimer reaches scaleTime
    public PlayerUIPaneMgmt[] playerPanes; //references to each player pane in the HUD. Eventually, we may want to programmatically spawn them on Awake.


    //Animation-related parameters
    public bool useHUDAnimation; //toggles the animation in which the HUD and dialogue boxes slide on/off screen
    public float UIWindowScaleTime; //time, in seconds, required to complete scaling
    float animTimer = 0; //time elapsed during animation

    //MGMT of Timer UI Pane:
    public TMPro.TextMeshProUGUI timerText; //Reference to the Text UI Element containing the "Seconds Remaining" stuff.   
    //Sets the timer to display in the correct format and updates the text
    public void SetTimerText(float f)
    {
        timerText.text = string.Format("{0:0.0}", f);
    }

    //Sets the timer to display a message
    public void SetTimerText(string s)
    {
        timerText.text = s;
    }


    //Toggles whether the bottom UI pane should be shown. If the bottom pane is shown, the top pane should be invisble.
    public void ShowBottomPaneMessage(bool isTrue)
    {
        bottomPaneMessageVisible = isTrue;
        animTimer = 0;
        if (debug) { Debug.Log("Message visible: " + bottomPaneMessageVisible); }
    }

    //Updates the weapon displayed on the Player's UI pane, given the corresponding bullet data and playerId
    public void UpdateUIPaneWeaponDisplay(int playerId, BulletData bd)
    {
        if (playerId >= 0 && playerId < playerPanes.Length)
        {
            playerPanes[playerId].SetWeaponGraphic(bd);
        }
    }

    //Enqueues messages into the SceneOverlayMessageUIStateMachine
    public void EnqueueOverlayMessage(SceneOverlayMessage s)
    {
        sceneMessage.EnqueueMessageString(s);
    }
    public void EnqueueOverlayMessages(SceneOverlayMessage[] s)
    {
        foreach (SceneOverlayMessage st in s)
        {
            sceneMessage.EnqueueMessageString(st);
        }
    }
    public void RunOverlayMessage()
    {
        
        sceneMessage.StartMessageCycle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && debug) {
            ShowBottomPaneMessage(!bottomPaneMessageVisible);
            DistributeCameras();
        }
    }

    //Called when object is spawned
    private void Awake()
    {
        //sceneMessage.Initialize(sceneMessage.inactiveState);
       // Debug.Log("Reference exists: "+sceneMessage); 

        //avoid user-error from setting a bad value in the editor for the variable targetAspect
        if (targetAspect <= 0)
        {
            targetAspect = 1;
        }

        messageCamHeight = topPaneHeight; //holdover from previous version;
        DistributeCameras(); //set up initial camera bounds

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

    //Spawns player panes in Canvas in HUD
    public void SpawnPlayerPanes(PlayerVars[] p)
    {
        if (p.Length != 4)
        {
            Debug.LogError("Error: incorrect dimensions for player vars when spawning UI");
            return;
        }

        for (int i = 0; i<4;i++)
        {
            playerPanes[i].mgmt = this;
            playerPanes[i].UpdateFromPlayerVar(p[i]);
        }
    }

    //Called by PlayerUIPaneMgmt to update fields
    public void UpdateUIParams(int id)
    {
        try
        {
          playerPanes[id].UpdateFromPlayerVar(mgmt.playerVars[id]);
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            Debug.LogError(e);
        }
    }

    //Coroutine for resizing screen at fixed intervals + helper functions
    IEnumerator ResizeScreen()
    {
        for (; ; )
        {
            //only rescale if there's a change in resolution
            if (screenAspect != windowCamera.aspect)
            {
                if (debug) { Debug.Log(TAG + "Resizing screen"); }
                DistributeCameras();

                
            
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    //Places the camera windows in their correct positions
    void DistributeCameras()
    {
        float camX = (1 - targetAspect / windowCamera.aspect) / 2.0f;
        float mainCamY = 0;
        if (bottomPaneMessageVisible)
        {
            mainCamY = messageCamHeight;
        }


        //Depending on whether the screen is wider than it is tall, or vice-versa, size the cameras differently
        if (windowCamera.aspect >= 1) //wider than tall
        {
            topCamera.rect = new Rect(camX, mainCameraHeight + mainCamY, targetAspect / windowCamera.aspect, topPaneHeight);
            mainCamera.rect = new Rect(camX, mainCamY, targetAspect / windowCamera.aspect, mainCameraHeight);
            bottomCamera.rect = new Rect(camX, mainCamY - messageCamHeight, targetAspect / windowCamera.aspect, messageCamHeight);
        }
        else //taller than wide
        {
            /*
             * When dealing with aspect ratios < 1, each Camera should have normalized viewport width = 1 and normalized viewport height = windowAspect/targetAspect
             * 
             */
            float normalizedHeight = windowCamera.aspect / targetAspect;
            float bottomY = (1.0f - normalizedHeight) / 2.0f; //highest point of the bottom black box of the letterboxing, in normalized space
            
            //
            topCamera.enabled = !bottomPaneMessageVisible;
            bottomCamera.enabled = bottomPaneMessageVisible;

            topCamera.rect = new Rect(0, bottomY + normalizedHeight * (mainCamY+mainCameraHeight), 1,normalizedHeight* topPaneHeight);
            mainCamera.rect = new Rect(0, bottomY+normalizedHeight*mainCamY, 1, normalizedHeight * mainCameraHeight);
            bottomCamera.rect = new Rect(0, bottomY + normalizedHeight * (mainCamY-messageCamHeight), 1, normalizedHeight * messageCamHeight);
            
            
        }
        screenAspect = windowCamera.aspect;


    }


    //Animation function
    void AnimateWindowPosition()
    {
        
        //Animation timer should increase if message is visible and decrease if it's not.
        //animTimer / UIWindowScaleTime should define the window offset in the DistributeCameras function
        if (bottomPaneMessageVisible) {
            if (useHUDAnimation)
            {
                animTimer = 1;
            }
            else
            {
                animTimer = Mathf.Min(animTimer + Time.deltaTime, UIWindowScaleTime);
            }
        }
        else
        {
            if (useHUDAnimation)
            {
                animTimer = 0;
            }
            else
            {
                animTimer = Mathf.Max(animTimer - Time.deltaTime, 0);
            }
        }
    }
    
}

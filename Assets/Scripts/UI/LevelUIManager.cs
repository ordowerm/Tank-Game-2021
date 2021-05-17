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
    public float topPaneHeight; //proportion of screen covered by the top UI panel
    public float mainCameraHeight; //proportion of screen covered by the main game, when the message panel is not visible 
    public float messageCamHeight; //for displaying text on bottom of screen
    public Camera topCamera;
    public Camera mainCamera;
    public Camera bottomCamera;
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


        AdjustMainCameraViewport();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (UIWindowScaleTime == 0)
        {
            UIWindowScaleTime = 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float scaleTime; //time, in seconds, required to complete scaling
    
    
    float animTimer = 0;
    bool messageVisible=false; //if this flag is true, then the animation should advance until animTimer reaches scaleTime

    public void ShowMessage(bool isTrue)
    {
        messageVisible = isTrue;
        animTimer = 0;
        Debug.Log("Message visible: " + messageVisible);
    }

              
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            ShowMessage(!messageVisible);
        }


        if (
            animTimer < scaleTime
            )
        {
            //advance animation timer 
            animTimer += Time.deltaTime;
            if (animTimer > scaleTime) { animTimer = scaleTime; }

            //linearly interpolate camera sizes
            float prop;
            
            if (messageVisible)
            {
                prop = animTimer / scaleTime;
            }
            else
            {
                prop = 1 - (animTimer / scaleTime);

            }

            mainCamera.rect = new Rect(0, prop * messageCamHeight, 1, (mainCameraHeight - prop * messageCamHeight));
            bottomCamera.rect = new Rect(0, 0, 1, prop * messageCamHeight);            
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (scaleTime == 0)
        {
            scaleTime = 1;
        }
    }
}

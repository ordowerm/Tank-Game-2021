using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 Manages the message textbox that appears in the main game scene's canvas.
 
 */

public class SceneOverlayMessageUIStateMachine : GameStateMachine, ITextDisplayListener
{
    //User-controller parameters
    public float width;
    public float height;
    public Queue<SceneOverlayMessage> messageQueue;

    //Timing parameters
    public float scrollMaxtime; //multiplier for Time.deltaTime when animating scrolling motions
    public float textboxDelayTime; //timer for waiting between textboxes

    //References to specific objects
    public TextDisplayer textDisplayer;
    public GameObject textObject;
    public GameObject[] images; //references to GameObjects containing Image components in order to alter their alpha values for the fadeout effect
    public RectTransform rt; //in editor, set this to the RectTransform of this script's gameObject


    //Runtime parameters
    SceneOverlayMessage message; //reference to the most-recently-popped thing
    public SceneOverlayInactiveState inactiveState;
    public SceneOverlayScrollFromLeftState scrollFromLeftState;
    public SceneOverlayStateRenderTextState renderTextState;
    public SceneOverlayScrollCenterToRightState scrollCenterToRightState;
    bool initialized = false;


    private void Awake()
    {


        if (!initialized)
        {


            //Creates states
            inactiveState = new SceneOverlayInactiveState(gameObject, this);
            scrollFromLeftState = new SceneOverlayScrollFromLeftState(gameObject, this);
            renderTextState = new SceneOverlayStateRenderTextState(gameObject, this);
            scrollCenterToRightState = new SceneOverlayScrollCenterToRightState(gameObject, this);
            currentState = inactiveState;
            initialized = true;
        }



        //
        rt = gameObject.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(-width - 50, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5 and reference resolution = 100x100
        rt.sizeDelta = new Vector2(width, height);
        messageQueue = new Queue<SceneOverlayMessage>();
    }



    //Enqueue string (Call from LevelUIManager)
    public void EnqueueMessageString(SceneOverlayMessage s)
    {
        messageQueue.Enqueue(s);

    }


    //Causes text to display in the textbox
    public void SendOverlayMessage()
    {
        //Debug.Log("Sending overlay message in SceneOverlaySM");
        if (messageQueue.Count > 0)
        {
            message = messageQueue.Dequeue();
            textDisplayer.SetMessage(message);
        } else
        {
            //Begin scrolling right if no messages are supposed to display
            if (
                currentState == renderTextState
                )
            {
                ChangeState(scrollCenterToRightState);
            }

            //Notify level manager that the messages have been delivered
            levelManager.NotifySceneMessageFinishedRendering();
        }
    }


    //Call from TextDisplayer after render is complete
     public void NotifyTextRenderComplete(TextDisplayer td)
    {
        //Debug.Log("SceneOverlaySM: Notified");
        if (message.messageDelay != Mathf.Infinity)
        {
            StartCoroutine(DelayMessageAdvance(message.messageDelay));
        }
    }

    //Coroutine that delays for a little bit before updating the next frame
    IEnumerator DelayMessageAdvance(float delay)
    {
        bool running = true;
        while (running)
        {
            running = false;
            yield return new WaitForSeconds(delay);
        }
        SendOverlayMessage();
    }

    //Restarts messaging process
    public void StartMessageCycle()
    {
        if (!initialized)
        {
           // Debug.Log("Uninitialized.");
            Awake();
        }
       ChangeState(scrollFromLeftState);
    }
}

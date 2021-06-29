using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 Manages the message textbox that appears in the main game scene's canvas.

 As of 6-28-2021, this is implemented as an FSM
 
 
 */

public class SceneOverlayMessageUIScript : GameStateMachine
{
   

    //User-controller parameters
    public float width;
    public float height;
    public Queue<string> messageQueue;

    //Timing parameters
    float timer;
    public float scrollMaxtime; //multiplier for Time.deltaTime when animating scrolling motions
    public float textboxDelayTime; //timer for waiting between textboxes

    //References to specific objects
    public GameObject[] textboxes; //These textboxes contain Text components and TextDisplayer scripts.
    public GameObject[] images; //references to GameObjects containing Image components in order to alter their alpha values for the fadeout effect
    RectTransform rt; //in editor, set this to the RectTransform of this script's gameObject


    //Runtime parameters
    SceneMessageState state; //overall state of script
    StateChangeVariant stateChangeStatus; 
    SceneMessageState nextState; 

    private void Awake()
    {

        rt = gameObject.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(-width - 50, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5
        rt.sizeDelta = new Vector2(width, height);
        messageQueue = new Queue<string>();
        state = SceneMessageState.INACTIVE;
        stateChangeStatus = StateChangeVariant.ON_ENTER;

    }


    //Changes state
    public void ChangeState(SceneMessageState st)
    {
        stateChangeStatus = StateChangeVariant.ON_EXIT;
        nextState = st;
    }


    /*
     Instead of using separate OnEnter and OnExit functions and individual classes representing each state type,
     we're just using a flag denoting whether the state is being entered or exited.
     Set onEnter to true to run the onEnter helper function. Set onEnter to false to run the onExit helper function.
     */
    protected enum StateChangeVariant
    {
        ON_ENTER,
        RUN,
        ON_EXIT
    }
    //Main function to call either in Update or a Coroutine, depending on how 
    void RunState()
    {
        if (debug)
        {
            Debug.Log("State: " +state.ToString() + "  Call: "+stateChangeStatus);
        }

        if (stateChangeStatus == StateChangeVariant.ON_ENTER)
        {
            timer = 0;
        }

        switch (state)
        {
            case SceneMessageState.INACTIVE:
                RunInactiveState();
                break;
            case SceneMessageState.SCROLL_L_CENTER:
                RunScrollLeftCenterState();
                break;
            case SceneMessageState.RENDERING_TEXT:
                RunRenderTextState();
                break;
            case SceneMessageState.AWAITING_CONFIRM:
                break;
            case SceneMessageState.FADING:
                break;
            case SceneMessageState.AWAITING_TIMER:
                RunAwaitTimerState();
                break;
            case SceneMessageState.SCROLL_CENTER_R:
                RunScrollCenterRightState();
                break;
            default:
                break;
        }

        //Once the pseudo-onEnter has been called in the current state's helper function, set to running state
        if (stateChangeStatus == StateChangeVariant.ON_ENTER)
        {
            stateChangeStatus = StateChangeVariant.RUN;
        }

        //Once the pseudo-onExit has been called in the current state's helper function, advance the current state
        if (stateChangeStatus == StateChangeVariant.ON_EXIT)
        {
            state = nextState;
            stateChangeStatus = StateChangeVariant.ON_ENTER;
        }
    }

    
    //Helper function corresponding to SceneMessageState.INACTIVE
    void RunInactiveState()
    {
        bool activate = false;
        switch(stateChangeStatus) {
            case StateChangeVariant.ON_ENTER:
                break;
            case StateChangeVariant.RUN:
                break;
            case StateChangeVariant.ON_EXIT:

                activate = true;
                break;
        }
        /*
        //Activate/Deactivate all referenced UI objects
        foreach (GameObject g in images)
        {
            g.SetActive(activate);
        }
        foreach (GameObject g in textboxes)
        {
            g.SetActive(activate);
        }*/
    }
    //Helper function corresponding to SceneMessageState.Scroll_L_C
    void RunScrollLeftCenterState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                rt.localPosition = new Vector3(-width/2.0f - 50, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5
                break;
            case StateChangeVariant.RUN:
                float tempWidth = Mathf.Lerp(-50 - width/2.0f,0, Mathf.Min(timer, scrollMaxtime)/ scrollMaxtime);
                timer += Time.deltaTime; 
                rt.localPosition = new Vector3(tempWidth, 0, 0);
                if (timer >= scrollMaxtime)
                {
                    ChangeState(SceneMessageState.RENDERING_TEXT);
                }

                return;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }
    //Helper function for the RenderText state
    void RunRenderTextState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                if (messageQueue.Count > 0)
                {
                    string messageString = messageQueue.Dequeue();
                    textboxes[1].GetComponent<TextDisplayer>().SetMessage(messageString);
                }
                break;
            case StateChangeVariant.RUN:
                break;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }
    //Helper function for AwaitingConfirm
    void RunAwaitConfirmState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                break;
            case StateChangeVariant.RUN:
                break;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }
    //Helper function for AwaitTimer
    void RunAwaitTimerState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                break;
            case StateChangeVariant.RUN:
                timer += Time.deltaTime;
                if (timer >= scrollMaxtime)
                {
                    if (messageQueue.Count > 0) { 
                        ChangeState(SceneMessageState.RENDERING_TEXT);
                    }
                    else
                    {
                        ChangeState(SceneMessageState.SCROLL_CENTER_R);
                    }
                }
                break;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }
    //Helper function for fading
    void RunFadingTextState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                break;
            case StateChangeVariant.RUN:
                break;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }
    //Helper function for scrolling right
    void RunScrollCenterRightState()
    {
        switch (stateChangeStatus)
        {
            case StateChangeVariant.ON_ENTER:
                rt.localPosition = new Vector3(0, 0); //assumes anchors and pivots = 0.5, 0.5, 0.5, 0.5

                break;
            case StateChangeVariant.RUN:
                float tempWidth = Mathf.Lerp( 0, 50 + width, Mathf.Min(timer, scrollMaxtime) / scrollMaxtime);
                timer += Time.deltaTime;
                rt.localPosition = new Vector3(tempWidth, 0, 0);
                if (timer >= scrollMaxtime)
                {
                    ChangeState(SceneMessageState.INACTIVE);
                }

                return;
            case StateChangeVariant.ON_EXIT:
                break;
        }
    }


    //Enqueue string (Call from LevelUIManager)
    public void EnqueueMessageString(string s)
    {
        messageQueue.Enqueue(s);

    }
    /*
    // Update is called once per frame
    void Update()
    {
        RunState();


        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                messageQueue.Enqueue("This is a test.");
                ChangeState(SceneMessageState.SCROLL_L_CENTER);
            }
        }

    }*/
}

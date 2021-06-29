using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Renders text gradually into the textbox specified in the public field.
 * 
 * 
 */
public class TextDisplayer : MonoBehaviour
{
    public float displayStepSpeed_slow; //delay before next character(s) displayed
    public float displayStepSpeed_fast;
    public int charactersPerDisplayStep_slow;
    public int charactersPerDisplayStep_fast;
    public float confirmDelay; //delay before confirm button appears at the end of the textbox.
    public TextSpeed speed;
    public SceneOverlayMessageUIScript sceneMessageUIScript;

    //References to UI objects
    public Text text;

    float timer = 0; //timer controlling the delay
    string message = ""; //entire string to eventually render
    string textBuffer = ""; //part of string to render
    bool awaitingConfirmation = false;

    public enum TextSpeed
    {
        SLOW,
        FAST,
        SKIP
    }


    public void SetTextSpeed(TextSpeed t)
    {
        speed = t;
    }
    public void SetMessage(string s)
    {
        message = s;
        ClearBuffer();
        timer = 0;
        awaitingConfirmation = false;
    }
    public void SetMessage(SceneOverlayMessage s){

        speed = s.speed;
        SetMessage(s.message);
    }


    public void FillBuffer()
    {
        if (speed == TextSpeed.SKIP)
        {
            textBuffer = message;
            return;
        }
        
        //set iterations
        int iterations = charactersPerDisplayStep_slow;
        if (speed == TextSpeed.FAST)
        {
            iterations = charactersPerDisplayStep_fast;
        }

        //add characters to the message
        for (int i = 0; i<iterations;i++)
        {
            if (textBuffer.Length < message.Length)
            {
                textBuffer += message[textBuffer.Length];
            }
            else
            {
                if (!awaitingConfirmation)
                {
                    awaitingConfirmation = true;
                }
            }
        }
        
    }
    public void ClearBuffer()
    {
        textBuffer = "";
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }


    //advances timer and renders if appropriate
    void AdvanceTimer()
    {
        float maxTime;
        switch (speed)
        {
            case TextSpeed.FAST:
                maxTime = displayStepSpeed_fast;
                break;
            case TextSpeed.SLOW:
                maxTime = displayStepSpeed_slow;
                break;
            case TextSpeed.SKIP:
            default:
                maxTime = 0;
                break;
        }
        if (!awaitingConfirmation)
        {
            timer = Mathf.Min(timer + Time.deltaTime, maxTime);
            if (timer >= maxTime)
            {
                FillBuffer();
                timer = 0;
                text.text = textBuffer;

            }
        }


        if (message.Equals(textBuffer))
        {
            if (!awaitingConfirmation)
            {
                sceneMessageUIScript.ChangeState(SceneMessageState.AWAITING_TIMER);
                awaitingConfirmation = true;


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Test messaging:
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetMessage("This is a message to you, Rudy.");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetMessage("Better think of your future.");
        }

        AdvanceTimer();
    }
}

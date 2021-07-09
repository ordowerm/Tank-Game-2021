using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Renders text gradually into the textbox specified in the public field.
 * 
 * Maintains a list of Observers to notify when a message is finished rendering
 * 
 */
public class TextDisplayer : MonoBehaviour
{
    //Display preferences
    public float displayStepSpeed_slow; //delay before next character(s) displayed
    public float displayStepSpeed_fast;
    public int charactersPerDisplayStep_slow; //number of characters added to buffer, per frame
    public int charactersPerDisplayStep_fast;
    public TextSpeed speed;

    //References to UI objects
    public Text text;

    //Runtime variables that get modified
    public GameObject[] listeners;
    float timer = 0; //timer controlling the delay
    float delay; //current delay between calls to AppendCharactersToBuffer
    string fullMessage = ""; //entire string to eventually render
    string textBuffer = ""; //part of string to render on a given frame
    bool coroutineRunning = false; //flag denoting that the text displayer should stop trying to add new characters
    bool initialized = false;

    //Enumeration allowing for different presets of text speed
    public enum TextSpeed
    {
        SLOW,
        FAST,
        SKIP
    }
    public void SetTextSpeed(TextSpeed t)
    {
        speed = t;
        switch (speed)
        {
            case TextSpeed.FAST:
                delay = displayStepSpeed_fast;
                break;
            case TextSpeed.SLOW:
                delay = displayStepSpeed_slow;
                break;
            default:
                delay = 0;
                break;
        }
    }



    //Begin rendering
    public void SetMessage(string s)
    {
        if (coroutineRunning)
        {
            Debug.Log("Coroutine stopped");
            StopCoroutine(TextDisplayCoroutine()); //halt race conditions
        }
        fullMessage = s;
        ClearBuffer(); //Clears buffer before repopulating it
        //timer = 0; //resets timer
        //awaitingConfirmation = false;
        StartCoroutine(TextDisplayCoroutine());
    }
    public void SetMessage(SceneOverlayMessage s){

        speed = s.speed;
        SetMessage(s.message);
    }


    //Appends characters to current buffer
    public void AppendCharactersToBuffer()
    {
        if (speed == TextSpeed.SKIP)
        {
            textBuffer = fullMessage;
            return;
        }
        
        //set number of characters to append
        int lettersRenderedPerCycle = charactersPerDisplayStep_slow;
        if (speed == TextSpeed.FAST)
        {
            lettersRenderedPerCycle = charactersPerDisplayStep_fast;
        }

        //append individual characters to the message
        for (int i = 0; i<lettersRenderedPerCycle;i++)
        {
            if (textBuffer.Length < fullMessage.Length)
            {
                textBuffer += fullMessage[textBuffer.Length]; //Appends the next character of the message to the text buffer
            }
          
        }
        
    }
    
    //Clears buffer
    public void ClearBuffer()
    {
        textBuffer = "";
    }


    //NOTE: replaced this with a coroutine
    //advances timer and renders if appropriate
    //Eventually I might want to move this to a coroutine to improve performance
    void AdvanceTimer()
    {
       
        if (!coroutineRunning)
        {
            //Append characters to string and update Text component's text field
            timer = Mathf.Min(timer + Time.deltaTime, delay);
            if (timer >= delay)
            {
                AppendCharactersToBuffer();
                timer = 0;
                text.text = textBuffer;

            }

            //Check if buffer is filled with the entire message. If it is, notify listeners and mark the rendering as complete
            if (fullMessage.Length == textBuffer.Length) //I considered checking for an exact match between strings but didn't want the extra overhead
            {
                coroutineRunning = true;
                foreach (GameObject l in listeners)
                {
                    l.GetComponent<ITextDisplayListener>().NotifyTextRenderComplete(this);
                }
            }
        }

        
    }


    //There currently aren't any protections against race conditions 
    IEnumerator TextDisplayCoroutine()
    {
        //Debug.Log("Running TextDisplayCoroutine");
        while (textBuffer.Length < fullMessage.Length )
        {
            coroutineRunning = true;
            AppendCharactersToBuffer();
            text.text = textBuffer;

            //Check if buffer is filled with the entire message. If it is, notify listeners and mark the rendering as complete
            if (fullMessage.Length == textBuffer.Length) //I considered checking for an exact match between strings but didn't want the extra overhead
            {
                //Debug.Log("TextDisplay buffer filled");
                foreach (GameObject l in listeners)
                {
                    l.GetComponent<ITextDisplayListener>().NotifyTextRenderComplete(this);
                }
                coroutineRunning = false; //I think this should only call when the coroutine finishes, but I could be wrong.
                yield return null;
            }

            yield return new WaitForSeconds(delay);
        }
    }




    // Update is called once per frame
    void Update()
    {
        //Test messaging:
        
        /*(Input.GetKeyDown(KeyCode.L))
        {
            SetMessage("This is a message to you, Rudy.");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetMessage("Better think of your future.");
        }*/

        //AdvanceTimer();
    }
}

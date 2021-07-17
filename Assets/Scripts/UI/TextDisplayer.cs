using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMPro.TextMeshProUGUI text;

    //Runtime variables that get modified
    public GameObject[] listeners;
    float delay; //current delay between calls to AppendCharactersToBuffer
    bool coroutineRunning = false; //flag denoting that the text displayer should stop trying to add new characters
    bool initialized = false;
    int maxChars = 0; //max characters displayed in TextMeshProUGUI

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
        maxChars = 0;
        text.maxVisibleCharacters = maxChars;
        text.text = s;
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
            maxChars = text.text.Length;
            return;
        }
        //set number of characters to append
        int lettersRenderedPerCycle = charactersPerDisplayStep_slow;
        if (speed == TextSpeed.FAST)
        {
            lettersRenderedPerCycle = charactersPerDisplayStep_fast;
        }
       
        maxChars = Mathf.Min(maxChars + lettersRenderedPerCycle, text.text.Length); //increase number of visible chars
       
    }
    
     IEnumerator TextDisplayCoroutine()
    {
        while (maxChars < text.text.Length)
        {
            coroutineRunning = true;
            AppendCharactersToBuffer();
            text.maxVisibleCharacters = maxChars;
            
            //Check if buffer is filled with the entire message. If it is, notify listeners and mark the rendering as complete
            if (text.text.Length == maxChars) //I considered checking for an exact match between strings but didn't want the extra overhead
            {
                //Debug.Log("TextDisplay buffer filled:" + textBuffer);
                foreach (GameObject l in listeners)
                {
                    l.GetComponent<ITextDisplayListener>().NotifyTextRenderComplete(this);
                }
                coroutineRunning = false; //I think this should only call when the coroutine finishes, but I could be wrong.
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }
    }

}

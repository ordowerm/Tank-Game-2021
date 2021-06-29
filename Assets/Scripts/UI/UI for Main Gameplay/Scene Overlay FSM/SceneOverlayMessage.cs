using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    This class contains fields that should be fed into a TextDisplayer class by a SceneOverlayState.
 */
[System.Serializable]
public class SceneOverlayMessage
{
    public string message;
    public TextDisplayer.TextSpeed speed;
    public bool autoAdvance; //if the text should automatically advance to the next queued message, then this should be set to true; otherwise, the message will display until it is manually told to stop

    public SceneOverlayMessage(string t, TextDisplayer.TextSpeed s, bool a)
    {
        message = t;
        speed = s;
        autoAdvance = a;
    }
}

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
    public float messageDelay;

    public SceneOverlayMessage(string t, TextDisplayer.TextSpeed s, float d)
    {
        message = t;
        speed = s;
        messageDelay = d;
    }
}

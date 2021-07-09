using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayState : GameState
{
    public SceneOverlayState(GameObject t, GameStateMachine s) : base(t, s)
    {
    }

    //Even though this can be called directly from the SceneOverlayMessageUIStateMachine instead of relying on an external state, we make this interface available.
    public void ForceMessageDisplay()
    {
        if (((SceneOverlayMessageUIStateMachine)sm).messageQueue.Count > 0)
        {
            ((SceneOverlayMessageUIStateMachine)sm).SendOverlayMessage();
        }
    }


    //These methods just exist so that I don't have to repeatedly write out casting the state machine to the SceneOverlayMessageUIStateMachine class. 
    protected RectTransform rt()
    {
        return ((SceneOverlayMessageUIStateMachine)sm).GetComponent<RectTransform>();
    }
    protected Vector2 size()
    {
        return new Vector2(((SceneOverlayMessageUIStateMachine)sm).width, ((SceneOverlayMessageUIStateMachine)sm).height);
    }
    protected void SendOverlayMessage()
    {
        ((SceneOverlayMessageUIStateMachine)sm).SendOverlayMessage();
    }
}

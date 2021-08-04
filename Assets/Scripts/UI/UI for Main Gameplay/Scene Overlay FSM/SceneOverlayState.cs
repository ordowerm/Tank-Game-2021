using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOverlayState : GameState
{
    public SceneOverlayState(GameObject t, GameStateMachine s) : base(ref t, ref s)
    {
    }

    //Even though this can be called directly from the SceneOverlayMessageUIStateMachine instead of relying on an external state, we make this interface available.
    public void ForceMessageDisplay()
    {
        if (((SceneOverlayMessageUIStateMachine)_sm).messageQueue.Count > 0)
        {
            ((SceneOverlayMessageUIStateMachine)_sm).SendOverlayMessage();
        }
    }


    //These methods just exist so that I don't have to repeatedly write out casting the state machine to the SceneOverlayMessageUIStateMachine class. 
    protected RectTransform rt()
    {
        return ((SceneOverlayMessageUIStateMachine)_sm).GetComponent<RectTransform>();
    }
    protected Vector2 size()
    {
        return new Vector2(((SceneOverlayMessageUIStateMachine)_sm).width, ((SceneOverlayMessageUIStateMachine)_sm).height);
    }
    protected void SendOverlayMessage()
    {
        ((SceneOverlayMessageUIStateMachine)_sm).SendOverlayMessage();
    }
}

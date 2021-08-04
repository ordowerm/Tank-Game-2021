using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : GameState
{
    protected IControllerInput cont;

    public InputState(GameObject t, GameStateMachine s, IControllerInput c):base(ref t, ref s)
    {
        cont = c;
    }

    public void SetController(IControllerInput c)
    {
        cont = c;
    }

    public virtual void HandleInput() { }

}

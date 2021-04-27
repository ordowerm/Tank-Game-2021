using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//State machine accounting for input methods
public class InputStateMachine : GameStateMachine
{
    public bool gamepadControl;
    public GamepadConfig gamepad;
    public KeyConfig keyconfig;

   protected override void Update()
    {
        //POSSIBLE REFACTOR: does this need to be updated every frame?        
        if (Input.GetJoystickNames().Length>0 && gamepadControl) //automatically set to joystick control if settings call for it
        {
            ((InputState)currentState).SetController(gamepad);
        }
        else
        {
            ((InputState)currentState).SetController(keyconfig);
        }


        ((InputState)currentState).HandleInput();
        base.Update();
    }
}

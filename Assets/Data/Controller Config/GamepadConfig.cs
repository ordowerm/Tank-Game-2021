using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Scriptable object for a controller preset.

 Exposes interface for controller input: IControllerInput
 */


[CreateAssetMenu(fileName = "Gamepad Configuration", menuName = "ScriptableObjects/GamePreferences/ControllerConfig/Controller")]
public class GamepadConfig : ScriptableObject, IControllerInput
{
    public bool debug;
    public bool isRuntimeScriptableObject; //if this is marked as true, then the values of these variables can be overwritten during runtime.
    public KeyCode roll;
    public KeyCode switchWeaponRight;
    public KeyCode switchWeaponLeft;
    public KeyCode switchSubRight;
    public KeyCode switchSubLeft;
    public KeyCode shotbutton;
    public KeyCode subweapon;
    public KeyCode lockon;
    public KeyCode cancelLockOn;
    public KeyCode switchTargetRight;
    public KeyCode switchTargetLeft;
    public string moveAxisX; //default to left analog
    public string moveAxisY;
    public string altAxisX; //for dpad control
    public string altAxisY;
    public string aimAxisX; //default to right analog
    public string aimAxisY;
    public string shottrigger; //axis for shooting
    public string rolltrigger;
    public string subtrigger;
    public string locktrigger;
    public string unlockTrigger;
    public float triggerDeadzone; //deadzone for trigger
    public float deadzoneMove;
    public float deadzoneAim;
    public float deadzoneSwitchTarget;
    public LockOnType lockType;
    public string switchTargetAxis; //axis for toggling enemies
    public KeyCode weapon0;
    public KeyCode weapon1;
    public KeyCode weapon2;
    public KeyCode weapon3;

    //UI stuff
    public KeyCode menuConfirmButton;
    public KeyCode menuCancelButton;

    //Return normalized aiming vector. Use arctan to convert to angle when calculating aiming direction.
    public Vector2 GetAim()
    {
        //if (debug) { Debug.Log("In GetAim: "); }

        float x = Input.GetAxis(aimAxisX);
        float y = Input.GetAxis(aimAxisY);

        //if (debug) { Debug.Log("Aim: " + new Vector2(x, y)); }
        if (Mathf.Abs(x) < deadzoneAim) { x = 0; }
        if (Mathf.Abs(y) < deadzoneAim) { y = 0; }
        return new Vector2(x, y);//.normalized;    
    }

    //Return normalized movement vector
    public Vector2 GetAxis()
    {
        if (debug) { Debug.Log("In GetAxis: "); }
        //Debug.Log("Stix: "+Input.GetJoystickNames().Length);

        //check for dpad input.
        float x = Input.GetAxis(altAxisX);
        float y = Input.GetAxis(altAxisY);


        //if there's no dpad input, check analog input
        if (x == 0) {
            x = Input.GetAxis(moveAxisX);
            if (Mathf.Abs(x) < deadzoneMove)
            {
                x = 0;
            }
        }
        if (y == 0)
        {
            y = Input.GetAxis(moveAxisY);
            if (Mathf.Abs(y) < deadzoneMove)
            {
                y = 0;
            }
        }

        /*
        if (debug)
        {
            //Debug.Log("Movement axes: " + new Vector2(x, y));
        }*/

        //Return vector with maximum magnitude = 1. Smaller vectors are allowed using analog stick.
        Vector2 returnVector = new Vector2(x,y);
        if (returnVector.magnitude > 1)
        {
            returnVector.Normalize();
        }
        
        return returnVector;
    }

    //Return whether locked on enemy should switch
    public int GetSwitchAxis()
    {
        int result = 0;
        float magnitude = Mathf.Abs(Input.GetAxis(switchTargetAxis));
        if (
            GetButtonDown(ButtonID.SWITCH_TARGET_RIGHT) ||
            (Input.GetAxis(switchTargetAxis) > 0 && (magnitude >= deadzoneSwitchTarget))
            )
        {
            result++;
        }
        if (
            GetButtonDown(ButtonID.SWITCH_TARGET_LEFT) ||
            (Input.GetAxis(switchTargetAxis) < 0 && (magnitude >= deadzoneSwitchTarget))
            )
        {
            result--;
        }
        //Debug.Log("Axis for switching targets: " + result);
        return result;
    }

    //Helper functions for buttons. 
    //Store Input.GetKey/GetKeyDown/GetKeyUp as anonymous functions for ButtonFunc when calling ButtonHelper.
    delegate bool ButtonFunc(KeyCode k);

    //helper function for converting trigger values into booleans
    bool TriggerHelper(string s, bool unpress)
    {
        bool triggered = false;
        //check if roll is also assigned to trigger
        if (!s.Equals(""))
        {
            float val = Input.GetAxis(s);
            //Debug.Log("Axis Value: " + val);
            if (Mathf.Abs(val) > triggerDeadzone)
            {
                triggered = true;
            }
        }
        if (unpress)
        {
            return !triggered;
        }
        else
        {
            return triggered;
        }
    }

    //Feed correct button into Input callback
    bool ButtonHelper(ButtonID button, ButtonFunc bf, bool buttonUp)
    {
        KeyCode key;
        bool result = false;
        switch (button)
        {
            case ButtonID.ROLL:
                key = roll;
                result = TriggerHelper(rolltrigger,buttonUp);
                break;
            case ButtonID.SHOT:
                key = shotbutton;
                result = TriggerHelper(shottrigger,buttonUp);
                break;
            case ButtonID.SUBWEAPON:
                key = subweapon;
                result = TriggerHelper(subtrigger,buttonUp);
                break;
            case ButtonID.SWITCH_MAIN_LEFT:
                key = switchWeaponLeft;
                break;
            case ButtonID.SWITCH_MAIN_RIGHT:
                key = switchWeaponRight;
                break;
            case ButtonID.SWITCH_SUB_LEFT:
                key = switchSubLeft;
                break;
            case ButtonID.SWITCH_SUB_RIGHT:
            default:
                key = switchSubRight;
                break;
            case ButtonID.LOCK_ON:
                key = lockon;
                result = TriggerHelper(locktrigger, buttonUp);
                break;
            case ButtonID.CANCEL_LOCK_ON:
                key = cancelLockOn;
                result = TriggerHelper(unlockTrigger, buttonUp);
                break;
            case ButtonID.WEAPON0:
                key = weapon0;
                break;
            case ButtonID.WEAPON1:
                key = weapon1;
                break;
            case ButtonID.WEAPON2:
                key = weapon2;
                break;
            case ButtonID.WEAPON3:
                key = weapon3;
                break;
            case ButtonID.MENU_CONFIRM:
                key = menuConfirmButton;
                break;
            case ButtonID.MENU_CANCEL:
                key = menuCancelButton;
                break;

        }
        if (!result) { result = bf(key); } //if the result is still false (i.e. trigger input hasn't been converted to button input), then check key

        return result;
    }


    public bool GetButton(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKey(x); };
        return ButtonHelper(button, callback,false);
    }

    public bool GetButtonDown(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKeyDown(x); };
        return ButtonHelper(button, callback,false);
    }

    public bool GetButtonUp(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKeyUp(x); };
        return ButtonHelper(button, callback,true);
    }

    public bool IsController()
    {
        return true;
    }

    public LockOnType GetLockOnType()
    {
        return lockType;
    }

    


}

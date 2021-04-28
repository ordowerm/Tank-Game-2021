using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Button names
public enum ButtonID
{
    SHOT, SUBWEAPON, ROLL, SWITCH_MAIN_RIGHT, SWITCH_MAIN_LEFT, SWITCH_SUB_RIGHT, SWITCH_SUB_LEFT, LOCK_ON
}


//define interface for each control configuration type. Pass into HandleInput where applicable
public interface IControllerInput
{

    //For shooting, etc.
    bool GetButtonDown(ButtonID button); //set subweapon flag to true to check for sub
    bool GetButton(ButtonID button);
    bool GetButtonUp(ButtonID button);
       
        
    //For movement
    Vector2 GetAim(); //return either mouse position or analog horizontal/vertical axes when aiming with analog stick
    Vector2 GetAxis(); //

    //Return true if controller
    bool IsController();
}

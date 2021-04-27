using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Scriptable object for a controller preset.
 
 
 */


[CreateAssetMenu(fileName = "Keyboard Configuration", menuName = "ScriptableObjects/GamePreferences/ControllerConfig/Keyboard")]
public class KeyConfig : ScriptableObject, IControllerInput
{
    public KeyCode upkey;
    public KeyCode downkey;
    public KeyCode leftkey;
    public KeyCode rightkey;
    public KeyCode rollkey;
    public KeyCode switchWeaponRightKey;
    public KeyCode switchWeaponLeftKey;
    public KeyCode switchSubRightKey;
    public KeyCode switchSubLeftKey;
    public KeyCode shootkey;
    public KeyCode subweaponkey;

    public Vector2 GetAim()
    {
        return new Vector2(Input.mousePosition.x, Input.mousePosition.y); //return mouse position
    }

    public Vector2 GetAxis()
    {
        float x = 0;
        float y = 0;
        if (Input.GetKey(leftkey))
        {
            x--;
        }
        if (Input.GetKey(rightkey))
        {
            x++;
        }
        if (Input.GetKey(upkey))
        {
            y++;
        }
        if (Input.GetKey(downkey))
        {
            y--;
        }

        return new Vector2(x, y).normalized;
    }

    
    //Helper functions for buttons. 
    //Store Input.GetKey/GetKeyDown/GetKeyUp as anonymous functions for ButtonFunc when calling ButtonHelper.
    delegate bool ButtonFunc(KeyCode k);
    
    //Feed correct button into Input callback
    bool ButtonHelper(ButtonID button, ButtonFunc bf){
        KeyCode key;
        switch (button)
        {
            case ButtonID.ROLL:
                key = rollkey;
                break;
            case ButtonID.SHOT:
                key = shootkey;
                break;
            case ButtonID.SUBWEAPON:
                key = subweaponkey;
                break;
            case ButtonID.SWITCH_MAIN_LEFT:
                key = switchWeaponLeftKey;
                break;
            case ButtonID.SWITCH_MAIN_RIGHT:
                key = switchWeaponRightKey;
                break;
            case ButtonID.SWITCH_SUB_LEFT:
                key = switchSubLeftKey;
                break;
            case ButtonID.SWITCH_SUB_RIGHT:
            default:
                key = switchSubRightKey;
                break;
        }
        return bf(key);
    }


    public bool GetButton(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKey(x); };
        return ButtonHelper(button,callback);   
    }

    public bool GetButtonDown(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKeyDown(x); };
        return ButtonHelper(button, callback);
    }

    public bool GetButtonUp(ButtonID button)
    {
        ButtonFunc callback = (KeyCode x) => { return Input.GetKeyUp(x); };
        return ButtonHelper(button, callback);
    }

    public bool IsController()
    {
        return false;
    }
}
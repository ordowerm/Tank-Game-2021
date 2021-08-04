using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : InputState
{
    protected bool isActive;
    protected float iFrameTimer;

    public PlayerState(GameObject t, GameStateMachine s, IControllerInput c) : base(t, s, c)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        iFrameTimer = ((PlayerSM)_sm).GetIFrameTimer();
    }

    public override void OnExit()
    {
        base.OnExit();
        ((PlayerSM)_sm).SetIFrameTimer(iFrameTimer);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        //TO-DO: button presses change the weapon
        WeaponSelectHelp();

    }
    
    //Helper function eliminating button press priority when trying to change weapons
    //In other words, pressing Weapon0 only works if you're NOT also pressing others
    protected void WeaponSelectHelp()
    {
        bool w0 = cont.GetButtonDown(ButtonID.WEAPON0);
        bool w1 = cont.GetButtonDown(ButtonID.WEAPON1);
        bool w2 = cont.GetButtonDown(ButtonID.WEAPON2);
        
        
        if (
                //if multiple weapon switch pressed, return
                (
                    (w0 && w1) ||
                    (w1 && w2) ||
                    (w0 && w2)
                ) ||
                !(w0 || w1 || w2) //if no weapon switch pressed, return
            )
        {
            return;
        }

        

        if (w0) { ((PlayerSM)_sm).SetWeapon(WeaponScript.WeaponName.WEAPON0); }
        if (w1) { ((PlayerSM)_sm).SetWeapon(WeaponScript.WeaponName.WEAPON1); }
        if (w2) { ((PlayerSM)_sm).SetWeapon(WeaponScript.WeaponName.WEAPON2); }



    }
}

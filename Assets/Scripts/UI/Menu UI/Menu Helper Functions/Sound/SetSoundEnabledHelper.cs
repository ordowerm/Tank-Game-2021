using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetSoundEnabled",menuName ="UI Menu Item/SetSoundEnabled")]
public class SetSoundEnabledHelper : MenuHelper
{
    public override void ExecuteMenuCommand(GameSettings g,MenuScript m)
    {
        base.ExecuteMenuCommand(g,m);
        g.soundEnabled = true;
        m.SetFlags(false, true, true, false);

    }

    public override void Print()
    {
        Debug.Log(this.ToString()+"Sound enabled");
    }

    public override void OnInitialize(GameSettings g, MenuScript target)
    {
        base.OnInitialize(g, target);
        if (g.soundEnabled)
        {
            target.SetFlags(false, true, true, false);
        }
        else
        {
            target.SetFlags(false, false, false, false);
        }
    }
}

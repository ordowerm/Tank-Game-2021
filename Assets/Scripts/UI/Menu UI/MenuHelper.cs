using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This ScriptableObject exposes the ExecuteMenuCommand interface to the calling class, typically a MenuScriptOld.
 Classes inheriting from MenuHelper will offer a specific implementation of the IMenuHelper's ExecuteMenuCommand method.
 We store this as a ScriptableObject for workflow reasons; doing so allows us to store each MenuHelper as a Serialized asset.
 By saving each MenuHelper as separate asset, we can drag and drop each command into the Inspector without using an extra MonoBehaviour.

 */
public class MenuHelper : ScriptableObject
{
    public bool debug;
    public virtual void ExecuteMenuCommand(GameSettings g, MenuScript target)
    {
        if (debug) { Print(); }
        if (g == null) { return; }
    }

    public virtual void OnInitialize(GameSettings g, MenuScript target)
    {

    }

    public virtual void Print()
    {

    }
}

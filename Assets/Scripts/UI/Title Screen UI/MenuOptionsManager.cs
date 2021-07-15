using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Maintains a list of references to MenuOptionScripts.
 * Notifies them/changes their states as needed.
 * 
 */
public class MenuOptionsManager : MonoBehaviour
{
    public MenuOptionScript[] optionScripts;
    List<MenuOptionScript> options;
    public GameSettings settings;
    int selectedIndex = -1;
    IControllerInput cont;

    //Color palette to use for the different UI states
    public ColorList[] palette;



    public void AddOption(MenuOptionScript opt)
    {
        options.Add(opt);
        opt.SetOptId(options.Count - 1);
    }
    public void RemoveOption(int index)
    {
        options.RemoveAt(index);
        ReassignIndices();
        selectedIndex = -1;
    }
    void ReassignIndices()
    {
        for (int i = 0; i<options.Count; i++)
        {
            options[i].SetOptId(i);
        }
    }
    public void NotifyPressed(int index)
    {
        foreach (MenuOptionScript o in options)
        {
            if (o.GetIndex() != index)
            {
                o.SetState(MenuOptionScript.MenuOptionState.NEUTRAL);
            }
            else
            {
                o.SetState(MenuOptionScript.MenuOptionState.CLICK);
            }
        }
    }

  


}

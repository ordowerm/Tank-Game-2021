using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
 * Maintains a list of references to MenuOptionScripts.
 * Notifies them/changes their states as needed.
 * 
 */
public class MenuOptionScriptOld : MonoBehaviour
{
    //Layout variables
    public float preferredHeight; //pass this into LayoutElement component
    public float preferredWidth; //pass this into LayoutElement component
    public enum MenuOrientation { HORIZONTAL, VERTICAL }
    public MenuOrientation orientation;
    public ScriptableObject controller; //Reference to a ScriptableObject implementing IControllerInput. Eventually, this will be replaced by the GameManager passing in the IControllerInput.
    public MenuColorList menuColorList;

    //References to UI Objects
    public Image[] myTitleFrames; //reference to the frame overlay
    public Image[] myTitleBgFills; //reference to the background fill
    public TextMeshProUGUI[] myTitleTexts; //reference to the text associated with this menu option
    public MenuOptionScriptOld[] childrenOptions; //references to submenus
    public Image subMenuFrame; //reference to the frame surrounding the submenu
    public GameSettings settings;

    //Runtime variables
    MenuOptionScriptOld parentMenu; //reference to parent
    public int id;
    //int previouslySelectedIndex = 0; //this is the index of the child option that is already selected
    int highlightedIndex = 0; //index of the child currently highlighted
    public bool active;
    public MenuStatus status;
    Vector2 prevIn; //previous input
    IControllerInput control;
 

    //Recursive function that either returns the local MenuColorList, the parent menu's MenuColorList, or null.
    public MenuColorList GetMenuColorList()
    {
        if (menuColorList)
        {
            return menuColorList;
        }
        else
        {
            if (parentMenu)
            {
                return parentMenu.GetMenuColorList();
            }
        }

        return null; 
    }
    //Recursive function for obtaining GameSettings
    public GameSettings GetSettings()
    {
        if (settings!=null)
        {
            return settings;
        }
        else
        {
            if (parentMenu)
            {
                return parentMenu.GetSettings();
            }
        }

        return null;
    }


 
    

    //Functions to override
    public virtual void OnCancel()
    {
        if (parentMenu)
        {
            parentMenu.active = true;
            active = false;
        }
    }
    public virtual void OnSelect()
    {
        menuColorList = GetMenuColorList();
        if (childrenOptions.Length > 0)
        {
            SetColors(MenuStatus.UNSELECTED);
            SetChildDisplayFrameActive(true);
            Debug.Log("Selecting");
            childrenOptions[highlightedIndex].SetActive(true);
            active = false;
        }
        else
        {
            SetColors(MenuStatus.SELECTED);
        }
    }



    //Compares movement axis input from controller to previous reading. If a change in the input has been detected, update the index of the currently selected option.
    protected void UpdateFromAnalog()
    {
        Vector2 dir = control.GetAxis();
        float axisVal = dir.x;
        float compVal = prevIn.x;
        if (orientation == MenuOrientation.VERTICAL)
        {
            axisVal = dir.y;
            compVal = prevIn.y;
        }

        if (axisVal != 0 && childrenOptions.Length > 0)
        {
            if (axisVal < 0 && compVal >= 0)
            {
                highlightedIndex = (highlightedIndex + 1) % childrenOptions.Length;
            }
            else if (axisVal > 0 && compVal <= 0)
            {
                if (highlightedIndex == 0) { highlightedIndex = childrenOptions.Length - 1; }
                else { highlightedIndex--; }
            }
        }
        prevIn = dir;
    }
    //Gets button input for confirm/cancel keys/buttons and calls OnSelect or OnCancel
    protected void UpdateFromButtons() {
        if (control.GetButtonDown(ButtonID.MENU_CONFIRM))
        {
            OnSelect();
        }
        else if (control.GetButtonDown(ButtonID.MENU_CANCEL))
        {
            OnCancel();
        }
    }


    //Recolors UI objects
    public void SetColors(Color fontColor, Color fillColor)
    {
        foreach (TextMeshProUGUI tmp in myTitleTexts)
        {
            tmp.color = fontColor;
        }

        foreach (Image i in myTitleBgFills)
        {
           i.color = fillColor;
        }
    }
    public void SetColors(MenuStatus m)
    {
        Color fontColor = menuColorList.GetTextColor(m);
        Color fillColor = menuColorList.GetBoxColor(m);

        foreach (TextMeshProUGUI tmp in myTitleTexts)
        {
            tmp.color = fontColor;
        }

        foreach (Image i in myTitleBgFills)
        {
            i.color = fillColor;
        }
    }
    
    //Activates/deactivates input reading
    public void SetActive(bool activeTrue)
    {
        active = activeTrue;
        prevIn = control.GetAxis(); //since input isn't updated when active is set to false, prevIn might retain its value from before this menu was set to inactive. This safeguards against that.
    }


    //Toggle whether the white border surrounding this menu option should be displayed
    public void SetDisplayFrameActive(bool val)
    {
        foreach (Image f in myTitleFrames)
        {
            f.enabled = val;
        }
    }
    public void SetChildDisplayFrameActive(bool val)
    {
        if (subMenuFrame)
        {
            subMenuFrame.enabled = val;

        }
    }


    //MonoBehaviour overrides
    private void Awake()
    {
        if (parentMenu)
        {
            menuColorList = GetMenuColorList(); //recursively assign color list
            settings = GetSettings();
        }
        
        
        try
        {
            control = (IControllerInput)controller;

        }
        catch (System.InvalidCastException e)
        {
            Debug.LogError("Scriptable object attached to MenuOptionsManager does not implement IControllerInput: " + e);
        }

        for (int i = 0; i < childrenOptions.Length; i++)
        {
            childrenOptions[i].parentMenu = this;
            childrenOptions[i].id = i;
        }
    }

    private void Update()
    {
        if (active)
        {
            UpdateFromAnalog();
            UpdateFromButtons();
        }
    }


}

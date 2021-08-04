using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScriptOld : MonoBehaviour
{
    public enum MenuOrientation { HORIZONTAL, VERTICAL }
    public MenuOrientation orientation;

    //UI Object references
    public GlobalGameMgmt gameMgmt;
    public MenuColorList menuColorList;
    public MenuScriptOld parentMenu;
    public Image cursorImage; //reference to the image used as a bullet point to indicate that an option is selected
    public Image backgroundPanel;
    public Image frameImage; //Image for the border frame
    public TextMeshProUGUI text; //the text mesh containing the label for this Menu
    public MenuScriptOld[] children;
    public MenuHelper command;

    //Runtime parameters
    public bool isCurrentMenu;
    List<IControllerInput> controllers;
    public GameSettings settings = null;
    public MenuStatus status; //status of this menu
    int index; //index of this object
    int selectedIndex; //index of previously-selected child. Used when the child menu calls OnCancel().
    int highlightedIndex; //when this menu is active, this represents the selected child's index
    int previousDirectionalInput = 0; //used for detecting changes in menu input

    //Helper functions
    public virtual void OnSelect()
    {
        if (command)
        {
            //command.ExecuteMenuCommand(GetSettings(),this);
        }
        SetColors(MenuStatus.SELECTED);

        if (children.Length>0)
        {
            isCurrentMenu = true;
            SetChildrenHighlighted();
        }
        else if (parentMenu)
        {
            parentMenu.isCurrentMenu = true;
            parentMenu.selectedIndex = parentMenu.highlightedIndex;
            parentMenu.SetChildrenHighlighted();
            isCurrentMenu = false;
        }
    }
    public virtual void OnHighlight() {
        SetColors(MenuStatus.HIGHLIGHTED);
    }
    public virtual void OnNotHighlight()
    {
        SetColors(MenuStatus.UNSELECTED);
    }
    public virtual void OnCancel()
    {
        if (parentMenu)
        {
            parentMenu.isCurrentMenu = true;
            parentMenu.highlightedIndex = parentMenu.selectedIndex;
            parentMenu.SetChildrenHighlighted();
            isCurrentMenu = false;

        }
    }
    public virtual void OnHighlightSelected()
    {
        SetColors(MenuStatus.HIGHLIGHTED_SELECTED);
    }

    //Recursive calls
    public GameSettings GetSettings()
    {
        if (parentMenu)
        {
            Debug.Log("Getting parent settings");
            return parentMenu.GetSettings();
        }
        else
        {
            return settings;
        }
    }
    public MenuColorList GetColorList()
    {
        if (parentMenu)
        {
            Debug.Log("Getting color list from parents!");
            return parentMenu.GetColorList();
        }
        else
        {
            return menuColorList;
        }
    }


    //Gets user input from any active controllers
    protected bool GetAllButtonsDown(ButtonID bId)
    {
        bool result = false;
        foreach (IControllerInput ic in controllers)
        {
            result = result | ic.GetButtonDown(bId);
        }
        return result;
    }
    protected int GetDirectionalInput()
    {
        //Debug.Log("Menu: Getting directional input.");
        bool incrementPressed = false;
        bool decrementPressed = false;

        //Combine all directional inputs
        foreach (IControllerInput i in controllers)
        {
            //Debug.Log("Menu: at least one controller detected");
            Vector2 dir = i.GetAxis();
            if (orientation == MenuOrientation.HORIZONTAL)
            {
                incrementPressed = incrementPressed | (dir.x > 0);
                decrementPressed = decrementPressed | (dir.x < 0);
            }
            else
            {
                incrementPressed = incrementPressed | (dir.y < 0);
                decrementPressed = decrementPressed | (dir.y > 0);
            }
        }


        //Return results
        if (incrementPressed)
        {
            if (decrementPressed)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else if (decrementPressed)
        {
            return -1;
        }

        return 0;
    }
    public void GetInput()
    {
        //Ensure there's controller input
        if (controllers.Count < 1)
        {
            if (gameMgmt)
            {
                //Debug.Log("Menu: getting settings from gameMgmt");
                settings = gameMgmt.settings;


            }
            else
            {
                settings = GetSettings();
            }
            foreach (PlayerVars p in settings.playerVars)
            {
                //Debug.Log("Menu: trying to add controllers");
                if (p.Cont() != null)
                {
                    //Debug.Log("Menu: Controller added");
                    controllers.Add(p.Cont());
                }
            }

        }


        //Run helper functions
        //Debug.Log("Menu: getting input");
        if (GetAllButtonsDown(ButtonID.MENU_CANCEL))
        {
            Debug.Log("Menu:Cancel pressed");
            highlightedIndex = selectedIndex;
            OnCancel();
        }
        else if (GetAllButtonsDown(ButtonID.MENU_CONFIRM))
        {
            Debug.Log("Menu: Confirm pressed");
            if (children.Length > 0)
            {
                isCurrentMenu = false; //leave this menu and go to highlighted menu
                children[highlightedIndex].OnSelect();
            }
        }
        else
        {
            
            int input = GetDirectionalInput();
            if (
                input != 0 &&
                previousDirectionalInput != input &&
                children.Length > 0
                )
            {
                //Update highlighted index
                highlightedIndex += input;
                if (highlightedIndex >= children.Length)
                {
                    highlightedIndex = 0;
                }
                else if (highlightedIndex < 0)
                {
                    highlightedIndex = children.Length - 1;
                }

                SetChildrenHighlighted();
            }
            previousDirectionalInput = input;
            //Debug.Log("Menu: directional input = " + previousDirectionalInput);
        }
    }

    //UI mgmt
    public void SetColors(MenuStatus m)
    {
        
        if (cursorImage != null)
        {
            cursorImage.color = menuColorList.GetArrowColor(m);
        }
        if (text != null)
        {
            text.color = menuColorList.GetTextColor(m);
        }
        if (backgroundPanel != null)
        {
            backgroundPanel.color = menuColorList.GetBoxColor(m);
        }
    }
   
    
    public void SetChildrenHighlighted()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (i == highlightedIndex)
            {
                if (i == selectedIndex && parentMenu)
                {
                    children[i].OnHighlightSelected();

                }
                else
                {
                    children[i].OnHighlight();
                }
            }
            else
            {
                children[i].OnNotHighlight();
            }
        }
    }


    //MonoBehaviour stuff
    protected void Awake()
    {
        foreach (MenuScriptOld m in children)
        {
            m.parentMenu = this;
           
        }
        GetColorList();
        if (gameMgmt)
        {
            settings = gameMgmt.settings;
        }
        else
        {
            settings = GetSettings();

        }
        controllers = new List<IControllerInput>();
        SetColors(status);
        SetChildrenHighlighted();
        /*foreach (PlayerVars p in settings.playerVars)
        {
            if (p.cont != null)
            {
                Debug.Log("Menu: Controller added");
                controllers.Add(p.cont);
            }
        }*/
    
    }
    protected void Update()
    { 
        if (isCurrentMenu)
        {
            //Debug.Log("Menu is active");
            GetInput();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
    //UI element references
    public Image bgFrame;
    public Image bgFill;
    public Image arrowImage;
    public TextMeshProUGUI text;

    //Menu functionality variables
    public MenuScript parentMenu;
    public MenuScript[] children;
    public bool isRootMenu;
    bool isCurrentMenu; //set true if this menu is currently accepting input
    bool isHighlighted; //marks whether arrow is visible
    bool isSelected; //mark as true if this menu is a terminal node in the menu graph, AND it corresponds to the selected index of its parent
    bool isDeactivated; //for fields like "Music Volume", which can be deactivated, mark this as true when it's deactivated
    
    int index; //menu index for this item
    int selectedIndex; //which child MenuScript is currently selected?
    int highlightedIndex; //which child MenuScript is currently highlighted?
    public MenuHelper menuCommand; //specific implementation of what this menu item should do when selected

    //Game control parameters
    public GameSettings gameSettings; //reference to the game settings
    public MenuColorList colorList;
    int lastDirectionPressed = 0; //stores most recent directional input

    //Propagates menu/settings references downward
    protected void SetParametersRecursive()
    {
        for (int i = 0; i<children.Length;i++)
        {
            children[i].index = i;
            children[i].parentMenu = this;
            children[i].gameSettings = gameSettings;
            children[i].colorList = colorList;
            children[i].SetParametersRecursive();
            if (children[i].menuCommand)
            {
                children[i].menuCommand.OnInitialize(gameSettings, this);
            }
        }
    }

    //Events
    public void SetFlags(bool current, bool highlight, bool selected, bool deactivated)
    {
        isCurrentMenu = current;
        isHighlighted = highlight;
        isSelected = selected;
        isDeactivated = deactivated;
        AssignColors();
    }


    //depending on the boolean flags for this menu, assigns the appropriate colors to the UI objects
    protected void AssignColors()
    {

        Color bgColor = colorList.blackShade;
        Color textColor = colorList.whiteShade;
        Color arrowColor;

        //Assigns colors based on flags
       if (isSelected)
            {
                if (isDeactivated)
                {
                    bgColor = colorList.lightGrayShade;
                    textColor = colorList.darkGrayShade;
                }
                else
                {
                    bgColor = colorList.whiteShade;
                    textColor = colorList.blackShade;
                }
            }
       else
       {
           if (isDeactivated)
           {
               textColor = colorList.darkGrayShade;
           }
          
       }

        //if the item isn't highlighted, then its arrow should be the same color as the background color
        if (isHighlighted)
        {
            arrowColor = textColor; 
        }
        else
        {
            arrowColor = bgColor;
        }



        //Assigns colors if the UI elements exist
        if (bgFill)
        {
            bgFill.color = bgColor;
        }
        if (text)
        {
            text.color = textColor;
        }
        if (arrowImage)
        {
            arrowImage.color = arrowColor;
        }
    }
    
    //Call from child
    protected void NotifyChildSelected(int childindex)
    {
        if (childindex < 0 || childindex >= children.Length)
        {
            return;
        }
        else
        {
            selectedIndex = childindex;
        }
    }
    
    //Call when this MenuObject is selected from its parent menu
    protected void Select()
    {
        if (parentMenu)
        {
            parentMenu.NotifyChildSelected(index);
        }
        if (menuCommand)
        {
            menuCommand.ExecuteMenuCommand(gameSettings, this);
        }
    
    }



    protected void Awake()
    {
        if (isRootMenu)
        {
            SetParametersRecursive();
        }
    }
}

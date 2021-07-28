using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOptionScript : MonoBehaviour
{
    //Layout parameters
    public float preferredHeight; //pass this into LayoutElement component
    public float preferredWidth; //pass this into LayoutElement component
    public enum MenuOrientation { HORIZONTAL, VERTICAL }
    public MenuOrientation orientation;
    public float flashPeriod;
    public float flashTimeStep;

    //UI Object references
    public MenuColorList menuColorList;
    public MenuOptionScript parentMenu;
    public Image frameImage; //reference to the frame covering the entire row
    public Image titleBgPanel; //reference to the background image that should have its color changed when this MenuOptionScript is controlled by its parent
    public TextMeshProUGUI titleText; //reference to this menu's text object
    public MenuOptionScript[] children;
    
    //Runtime parameters
    List<IControllerInput> controllers;
    public GameSettings settings;
    public MenuStatus status ; //status of this menu
    int index; //index of this object
    int selectedIndex; //index of previously-selected child. Used when the child menu calls OnCancel().
    int highlightedIndex; //when this menu is active, this represents the selected child's index
    public bool active;
    bool isADeactivatedOption; //set this to true if the menu should be entirely unavailable. For example, if sound is disabled, then SFX Volume should be a deactivated option 
    IEnumerator runningEnumerator;
    int previousDirectionalInput=0; //used for detecting changes in menu input

    //Public methods
    public void ChangeStatus(MenuStatus s)
    {   
        if (status == s) { return; } //don't fiddle with anything if there's no real change

        //Stop currently-running enumerator to prevent race conditions
        if (runningEnumerator != null)
        {
            StopCoroutine(runningEnumerator);
        }
        StartCoroutine(LerpColor(status,s)); //Interpolate colors
        
        
        
        status = s;
        
    }
    protected virtual void OnHighlight() {
        ChangeStatus(MenuStatus.HIGHLIGHTED);
    }
    protected virtual void OnNotHighlight() { 
        ChangeStatus(MenuStatus.UNSELECTED);

    }
    protected virtual void OnSelect() {
        ChangeStatus(MenuStatus.SELECTED);
        //If a child menu exists, exit this menu and enter the highlighted menu
        if (children.Length > 0)
        {
            children[highlightedIndex].active = true;
            active = false;
        }
        else
        {

        }

    }
    protected virtual void OnCancel() {
        if (parentMenu)
        {
            parentMenu.highlightedIndex = parentMenu.selectedIndex;
            parentMenu.active = true;
        }

    }
    protected virtual void OnMenuOptionDisable()
    {
        if (parentMenu)
        {
            if (index == parentMenu.selectedIndex)
            {
                ChangeStatus(MenuStatus.DISABLED_SELECTED);
            }
            else
            {
                ChangeStatus(MenuStatus.DISABLED_UNSELECTED);

            }
        }
    }
    

    //Call, for example, when sound gets disabled. This sets the child options to irrelevant and should gray them out.
    protected virtual void SetChildrenDeactivated(bool val)
    {
        foreach (MenuOptionScript m in children)
        {
            m.isADeactivatedOption = val;
        }
    }

    //Sets child status as highlighted or unhighlighted
    protected virtual void SetHighlighted()
    {
        foreach (MenuOptionScript m in children)
        {
            if (m.index == highlightedIndex)
            {
                if (!m.isADeactivatedOption)
                {
                    m.ChangeStatus(MenuStatus.HIGHLIGHTED);

                }
                else
                {
                    m.ChangeStatus(MenuStatus.DISABLED_SELECTED);
                }
            }
            else
            {
                if (!m.isADeactivatedOption)
                {
                    m.ChangeStatus(MenuStatus.UNSELECTED);

                }
                else
                {
                    m.ChangeStatus(MenuStatus.DISABLED_UNSELECTED);
                }


            }
        }
    }

    //Call this in ChangeStatus.
    //This method interpolates text and box color for a smoother UX
    protected IEnumerator LerpColor(MenuStatus startStatus, MenuStatus endStatus)
    {

        //Constants:
        const float delay = 0.03f; //timestep between enumerations
        const float lifespan = 0.3f; //length of fading

        //Get colors between which to interpolate
        Color fontStart = menuColorList.GetTextColor(startStatus);
        Color fontEnd = menuColorList.GetTextColor(endStatus);
        Color bgStart = menuColorList.GetBoxColor(startStatus);
        Color bgEnd = menuColorList.GetBoxColor(endStatus);


        runningEnumerator = LerpColor(startStatus, endStatus); //To prevent race conditions, we pass a reference to this coroutine to the calling script
        float lerpTimer =0;
        while (lerpTimer < lifespan)
        {
            if (titleBgPanel)
            {
                titleBgPanel.color = Color.Lerp(bgStart, bgEnd, lerpTimer / lifespan);
            }
            if (titleText)
            {
                titleText.color = Color.Lerp(fontStart, fontEnd, lerpTimer / lifespan);
            }

            lerpTimer += delay;
            yield return new WaitForSeconds(delay);
        }
    }
    //Causes textbox to flash when highlighted, but not selected
    protected IEnumerator LerpFrameHighlighted()
    {
        float flashTimer = 0;
        //float timeStep = 0.08f;
        //const float flashPeriod = 0.6f;
        if (menuColorList == null) { menuColorList = GetColorList(); }
        Color fontStart = menuColorList.GetTextColor(MenuStatus.SELECTED);
        Color fontEnd = menuColorList.GetTextColor(MenuStatus.UNSELECTED);
        Color boxStart = menuColorList.GetBoxColor(MenuStatus.SELECTED);
        Color boxEnd = menuColorList.GetBoxColor(MenuStatus.UNSELECTED);
        bool increasing = true;

        while (status == MenuStatus.HIGHLIGHTED)
        {
            if (titleBgPanel)
            {
                frameImage.color = Color.Lerp(boxStart, boxEnd, flashTimer/flashPeriod);
            }
            if (titleText)
            {
                titleText.color = Color.Lerp(fontStart, fontEnd, flashTimer/flashPeriod);
            }

            if (increasing)
            {
                flashTimer += flashTimeStep;
                if (flashTimer >= flashPeriod)
                {
                    flashTimer = flashPeriod;
                    increasing = false;
                }

            }
            else
            {
                flashTimer -= flashTimeStep;
                if (flashTimer < 0)
                {
                    flashTimer = 0;
                    increasing = true;
                }
            }

            yield return new WaitForSeconds(flashTimeStep);
        }
    }



    //Recursive calls
    public GameSettings GetSettings()
    {
        if (settings != null)
        {
            return settings;
        }
        else if (parentMenu)
        {
            return parentMenu.GetSettings();
        }
        else
        {
            return null;
        }
    }
    public MenuColorList GetColorList()
    {
        if (menuColorList != null)
        {
            return menuColorList;
        }
        else if (parentMenu)
        {
            return parentMenu.GetColorList();
        }
        else
        {
            return null;
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
        bool incrementPressed = false;
        bool decrementPressed = false;

        //Combine all directional inputs
        foreach (IControllerInput i in controllers)
        {
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
        if (GetAllButtonsDown(ButtonID.MENU_CANCEL))
        {
            OnCancel();
        }
        else if (GetAllButtonsDown(ButtonID.MENU_CONFIRM))
        {
            OnSelect();
        }
        else
        {
            int input = GetDirectionalInput();
            if (
                input!= 0 && 
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

                SetHighlighted(); //update children colors
            }
            previousDirectionalInput = input;
        }
    }



    //MonoBehaviour overrides
    protected void Awake()
    {
        menuColorList = GetColorList();
        settings = GetSettings();
        controllers = new List<IControllerInput>();
        foreach(PlayerVars p in settings.playerVars)
        {
            controllers.Add(p.cont);
        }

        //status = MenuStatus.HIGHLIGHTED;
        //StartCoroutine(LerpFrameHighlighted());

        if (flashPeriod == 0)
        {
            flashPeriod = 1;
        }
        if (flashTimeStep == 0)
        {
            flashTimeStep = 0.1f;
        }
    }
    protected void Update()
    {
        if (active)
        {
            GetInput();
        }
    }
}

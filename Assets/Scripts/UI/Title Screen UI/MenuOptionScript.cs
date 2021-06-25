using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * This class defines the interface used for menu items on the title screen.
 * Derived classes should override the "OnSelected" method.
 * 
 */
public class MenuOptionScript : MonoBehaviour
{
    public MenuOptionsManager mgmt; //reference to menu to which this item belongs
    int optId; //index of this script in the MenuOptionsManager's list of options 


    struct ColorPair
    {
        public Color bg;
        public Color text;
    }
    ColorPair[] cPairs;

    public enum MenuOptionState { NEUTRAL, HOVER, CLICK, SELECTED}
    MenuOptionState menuState;

    //references to UI components
    public Image panelImage; //background to recolor, depending on state of this menu option
    public Text[] texts; //texts to recolor
    

    public void SetOptId(int i)
    {
        optId = i;
    }
    public int GetIndex() { return optId; }
    public void SetState(MenuOptionState state)
    {
        menuState = state;
        UpdateColors(state);
       
    }
    protected void UpdateColors(MenuOptionState state)
    {
        Color cTxt = cPairs[(int)state].text;
        panelImage.color = cPairs[(int)state].bg;
        foreach (Text t in texts)
        {
            t.color = cTxt;
        }
    }

    /*
     * This is probably not a good implementation of this.
     * 
     */

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

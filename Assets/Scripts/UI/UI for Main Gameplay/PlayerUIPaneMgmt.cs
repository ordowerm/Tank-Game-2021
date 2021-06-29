using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 This class controls an individual player's "section" of the HUD at the top of the screen, during the main gameplay.


 
 
 
 
 */
public class PlayerUIPaneMgmt : MonoBehaviour
{
    //Fields used for game mgmt
    public PlayerUIPaneState paneState; //current state of this UI pane
    public bool isActive;
    public LevelUIManager mgmt;
    int playerNumber;

    //References to UI Objects
    public Canvas canvas; //reference to the parent canvas on which the player pane is displayed
    public GameObject[] UIRows; //UI rows to deactivate, as necessary, to hide certain parts of the UI
    public Text PlayerNameText;
    public Text PlayerScoreText;
    public Image PortraitFace; //reference to the player portrait. Update skin color as needed.
    public Image PortraitHair; //reference to the player potrait's hair. Update style and color as needed.
    public Image WeaponIcon; //reference to the picture of the bullet type.
    public Image PortraitBg; //background/image mask for player portrait


    //Miscellaneous parameters
    const char numericDivider = ','; //used for localization purposes. In the US we divide every three digits of numbers > 0 with commas, but in other countries, it's periods or spaces. For this build, we set it as a constant, but if we localize for Europe, for example, we'll have to refactor this.


    //Health display
    /*
     The health display UI consists of 10 segments, each of which represents 1/10 of the player's health.
    As the player takes damage, the rightmost visible segment interpolates from one color to another.
    One color represents maximum health, out of the 1/10th of the total health each segment represents.
    The other color represents minimum health for that health segment.
    Once the health dips below that minimum health corresponding to that one segment, that segment becomes invisible.
     */
    public Image[] HealthSections; //Array of the little pictures for displaying health 
    public Color HealthMaxColor; //color representing "full health"
    public Color HealthMidColor; //color representing between [50,100) percent of health section
    public Color HealthMinColor; //color representing between (0,50) percent of health section
    public float MaxHealth;
    public float CurrentHealth;

    //increment or decrement health
    public void IncrementHealth(float incr, bool increaseMax)
    {
        if (!increaseMax)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + incr, MaxHealth);
        }
        else
        {
            float prop = CurrentHealth / MaxHealth; //CurrentHealth's proportion of the max health. Maintain proportion upon changing MaxHealth.
            MaxHealth += incr;
            CurrentHealth = MaxHealth * prop;
        }
        UpdateHealth();
    }
    
    //Set health to fixed value
    public void SetHealth(float val, bool isMax)
    {
        if (isMax) {
            float prop = CurrentHealth / MaxHealth; //CurrentHealth's proportion of the max health. Maintain proportion upon changing MaxHealth.
            MaxHealth = val;
            CurrentHealth = prop * MaxHealth;

        }
        else { CurrentHealth = val; }
        UpdateHealth();
    } 
    
    //Colors health sections
    public void UpdateHealth()
    {
        float healthStep = MaxHealth / (float)HealthSections.Length; //amount of health, per segment
        for (int i = 0; i < HealthSections.Length; i++)
        {
             //Debug.Log("Current Health: " + CurrentHealth + "; Section Health: " + i*healthStep);
            //uncull health section
            if (HealthSections[i].canvasRenderer.cull == true)
            {
                HealthSections[i].canvasRenderer.cull = false;
            }

            //Set default color for each square
            float prop = ((float)i * healthStep) / MaxHealth;
            if (prop >= 0 &&
                prop < 0.5f)
            {
                HealthSections[i].color = Color.Lerp(HealthMinColor, HealthMidColor, prop * 2.0f);
            }
            else if (
                prop < (MaxHealth - healthStep)/MaxHealth
                )
            {
                HealthSections[i].color = Color.Lerp(HealthMidColor, HealthMaxColor, 2*prop-1);
            }
            else
            {
                HealthSections[i].color = HealthMaxColor;
            }


            if (
                (float)i*healthStep >= CurrentHealth
                
                )
            {
                HealthSections[i].color = new Color(0, 0, 0, 0);
            }
        }

    }



    //Player Portrait Updates
    public void UpdateSkinColor(Color skincolor)
    {
        PortraitFace.color = skincolor;
    }
    public void SetHairdo(Sprite hairdo)
    {
        PortraitHair.sprite = hairdo;
    }
    public void SetHairdo(Sprite hairdo, Color color)
    {
        PortraitHair.sprite = hairdo;
        PortraitHair.color = color;
    }
    public void SetPlayerName(string name)
    {
        PlayerNameText.text = name;
    }
    //TO DO -- player damage animation

    
    //Update displayed score. add commas for the sake of good-ish grammar.
    public void SetScore(uint score)
    {
        //if player surpasses max score, just leave it as is.
        if (score > 999999999)
        {
            PlayerScoreText.text = "999,999,999+";
            return;
        }
        
        //Convert score to string and then add some commas
        string scoreT = score.ToString();
        //Debug.Log(scoreT);
        string newtext = ""; //string to build. Once for loop is completed, set ScoreText string.
        for (int i = 0; i<scoreT.Length; i++)
        {
            //add comma if appropriate
            if (
                i>2 && 
                i%3 == 0 //&& 
                //i < scoreT.Length -3
                )
            {
                newtext = numericDivider + newtext; //add the comma
            }

            //add next digit
            newtext = scoreT[scoreT.Length - 1 - i] + newtext;
        }
        PlayerScoreText.text = newtext;
    }

    //Set image and background color to match weapon
    public void SetWeaponGraphic(BulletData w)
    {
        WeaponIcon.sprite = w.sprite;
        WeaponIcon.color = w.element.primary;
        PortraitBg.color = w.element.primary;
    }

    
    
    /*
     * Call in LevelManager at start of stage, when spawning UI panes.
     * Updates all of the UI parameters simultaneously, so we might want to be sparing about calling this outside of that context.
    */
    public void UpdateFromPlayerVar(PlayerVars p)
    {
        isActive = p.isActive;
        playerNumber = p.playerNumber;
        CurrentHealth = p.playerHealth;
        MaxHealth = p.playerMaxHealth;
        SetPlayerName(p.playerName);
        SetHairdo(p.hairSprite,p.hairColor);
        UpdateSkinColor(p.skinTint);
        SetScore(p.playerScore);
        UpdateHealth();
        SetWeaponGraphic(p.weapondata.bullettype);


        //Set up initial state and UI Pane layout
        if (isActive)
        {
            paneState = PlayerUIPaneState.PLAYER_ACTIVE;
        }
        else
        {
            paneState = PlayerUIPaneState.NO_PLAYER;
            StateHelperPlayerInactive();

        }
    }


    /*
     * Rearranges UI to match UIPaneState when the UIPaneState is changed.
     * 
     */
    public void SetUIPaneState(PlayerUIPaneState ps)
    {
        //If no change required, return
        if (ps == paneState)
        {
            return;
        }

        switch (ps)
        {
            case PlayerUIPaneState.PLAYER_ACTIVE:
                StateHelperPlayerActive();
                break;
            case PlayerUIPaneState.TIME_UP:
            case PlayerUIPaneState.LEVEL_END:
            case PlayerUIPaneState.GAME_OVER:
                break;
            default:
            case PlayerUIPaneState.NO_PLAYER:
                StateHelperPlayerInactive();
                break;
        }



        paneState = ps;

    }
    //Helper functions for SetUIPaneState. Call in the switch statement of the SetUIPaneState method
    void StateHelperPlayerActive()
    {
        //Enable all rows
        foreach (GameObject g in UIRows)
        {
            g.SetActive(true);
        }
        if (!mgmt)
        {
            Debug.LogError("No LevelUIManager assigned.");
        }
        mgmt.UpdateUIParams(playerNumber); //update variables
    }
    void StateHelperPlayerInactive()
    {
        //Disable all rows
        foreach (GameObject g in UIRows)
        {
            g.SetActive(false);
        }

        PlayerNameText.text="Player "+(playerNumber+1)+"\nPress Start";
    }

}

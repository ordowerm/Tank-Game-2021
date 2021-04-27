using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Script for updating the UI panel during gameplay
 * The PlayerVars script should maintain a reference to this.
 * When values are updated, call methods in this script to update UI.
 */
public class PlayerUIPaneMgmt : MonoBehaviour
{
    public Text PlayerNameText;
    public Text PlayerScoreText;
    const char numericDivider = ','; //used for localization purposes. In the US we divide every three digits of numbers > 0 with commas, but in other countries, it's periods or spaces
    public Image PortraitFace; //reference to the player portrait. Update skin color as needed.
    public Image PortraitHair; //reference to the player potrait's hair. Update style and color as needed.
    public Image WeaponIcon; //reference to the picture of the bullet type.

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
    public void UpdateHealth()
    {
        float healthStep = MaxHealth / (float)HealthSections.Length; //amount of health, per segment
        for (int i = 0; i < HealthSections.Length; i++)
        {
            //uncull health section
            if (HealthSections[i].canvasRenderer.cull == true)
            {
                HealthSections[i].canvasRenderer.cull = false;
            }

            //get proportion of health section currently covered
            float healthprop = (CurrentHealth - ((float)i) * healthStep) / healthStep;


            if (healthprop >= 1)
            {
                HealthSections[i].color = HealthMaxColor;
            }
            else if (
                healthprop >= 0.5 &&
                healthprop < 1
                )
            {
                //HealthSections[i].color = HealthMidColor;
                HealthSections[i].color = Color.Lerp(HealthMidColor, HealthMaxColor, 2.0f*healthprop-1);
            }
            else if (
                healthprop > 0 &&
                healthprop < 0.5
                )
            {
                //HealthSections[i].color = HealthMinColor;
                HealthSections[i].color = Color.Lerp(HealthMinColor, HealthMidColor,  healthprop/0.5f);
            }
            else
            {
                HealthSections[i].canvasRenderer.cull = true;
            }
        }

    }
    
    
    //Other UI updates
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
    
    
    //update score. add commas for the sake of good-ish grammar.
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
        Debug.Log(scoreT);
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



}

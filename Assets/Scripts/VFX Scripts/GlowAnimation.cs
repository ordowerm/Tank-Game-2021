using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Little script that interpolates between two colors for a sprite renderer.
 * 
 * It's easier to edit parameters in the inspector than in the animation editor,
 * so I opt for this instead of an AnimatorController for the sake of flexibility.
 * 
 * 
 */
public class GlowAnimation : MonoBehaviour
{
    public Color c0;
    public Color c1;
    public float period; //time in seconds to interpolate from one color to the next
    public SpriteRenderer sprite;
    float timer = 0;
    bool reverse = false;
    public bool active;

    
    public void SetActive(bool isActive)
    {
        active = isActive;
        if (!active)
        {
            sprite.color = c0;
            timer = 0;
            reverse = false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        sprite.color = c0;
        timer = 0;
        reverse = false;
        
        //safeguard against division by 0 when I inevitably forget to set the period value
        if (period == 0)
        {
            period = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) { return; } //if script is not active, stop updating.

        //interpolate colors
        if (reverse)
        {
            sprite.color = Color.Lerp(c1, c0, timer / period);
        }
        else
        {
            sprite.color = Color.Lerp(c0, c1, timer / period);
        }

        //update timer
        timer += Time.deltaTime;
        if (timer > period)
        {
            timer = 0;
            reverse = !reverse;
        }
    }
}

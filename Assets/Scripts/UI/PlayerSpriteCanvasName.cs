using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/*
 * 
 * Script controlling the little arrow thingy that shows where the player is.
 * It assumes that the arrow thingy is contained in a Canvas that contains a VerticalLayoutGroup.
 * It assumes that the VerticalLayoutGroup contains two elements:
 *  - an object containing a Text component that displays the player name
 *  - an object containing an Image component that displays an arrow pointing toward the player sprite
 *  
 *  
 * 
 */

public class PlayerSpriteCanvasName : MonoBehaviour
{
    public VerticalLayoutGroup vert; //invert this when displayAbove is set to false;
    public Text nameText;
    public Image arrow;
    public Camera cam;
    public Color color;
    public float verticalOffset;
    public bool isVisible;
    public bool displayAbove;
    public bool debug;
    RectTransform rt;
    Canvas can;

    void DrawIcon()
    {
        if (!isVisible) { return; } 


        vert.reverseArrangement = !displayAbove; //
        float inversion = 1;
        if (!displayAbove) { inversion = -1; }
        arrow.rectTransform.localScale = new Vector3(1, inversion, 1);
        rt.position = new Vector3(0, inversion * verticalOffset);
    }
    public void SetColor(Color c)
    {
        color = c;
        nameText.color = color;
        arrow.color = color;
    }
    public void SetVisible(bool v)
    {
        isVisible = v;
        can.enabled = v;
    }



    /*
     * TODO:
     * 
     * This function raycasts the Canvas into world space.
     * If the raycast hits enemies or other players, it should swap sides so as not to obscure the actions.
     * If the raycast hits enemies or other players on the OTHER side, too, unsure what to do.
     */
    bool CheckBounds()
    {
        bool freeAbove = true;


        return freeAbove;
    }


    void Awake()
    {
        rt = GetComponent<RectTransform>();
        can = GetComponent<Canvas>();
        SetColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                displayAbove = !displayAbove;
                DrawIcon();
            }
        }
    }
}

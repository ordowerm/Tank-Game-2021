using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkinAttributes : MonoBehaviour
{
    public Color skintint; //color used for player skin
    public GameObject[] skinobjects; //objects to apply skin tint to.
    public Image hairImage;
    public Image faceImage;
    public Color haircolor;
    public Sprite hairtop;
    public GameObject hairobject;


    //function for updating player graphics
    public void SetPlayerStyle()
    {
        for (int i = 0; i < skinobjects.Length; i++)
        {
            skinobjects[i].GetComponent<SpriteRenderer>().color = skintint;
        }
        SpriteRenderer hair = hairobject.GetComponent<SpriteRenderer>();
        hair.sprite = hairtop;
        hair.color = haircolor;
        hairImage.sprite = hairtop;
        faceImage.color = skintint;
    }

    //function for updating player graphics from playerVars
    public void SetPlayerStyle(PlayerVars p)
    {
        for (int i = 0; i < skinobjects.Length; i++)
        {
            skinobjects[i].GetComponent<SpriteRenderer>().color = p.skinTint;
        }
        SpriteRenderer hair = hairobject.GetComponent<SpriteRenderer>();
        hair.sprite = p.hairSprite;
        hair.color = p.hairColor;
        //hairImage.sprite = hairtop;
        //faceImage.color = skintint;
    }



    // Start is called before the first frame update
    void Start()
    {
        //SetPlayerStyle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

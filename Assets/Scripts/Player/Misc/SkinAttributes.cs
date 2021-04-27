using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinAttributes : MonoBehaviour
{
    public Color skintint; //color used for player skin
    public GameObject[] skinobjects; //objects to apply skin tint to.
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
    }




    // Start is called before the first frame update
    void Start()
    {
        SetPlayerStyle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

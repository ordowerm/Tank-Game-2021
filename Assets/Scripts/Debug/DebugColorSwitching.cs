using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Simple script to place in scene to check hairdos and whatnot for player
 * 
 */
public class DebugColorSwitching : MonoBehaviour
{
    public SkinAttributes attr;
    public SpriteList hairtops;
    public ColorList skins;
    int hairid = 0;
    int skinid = 0;

    bool modifyskin = true;
    public KeyCode togglekey;
    public KeyCode increm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //change params
        if (Input.GetKeyDown(togglekey))
        {
            modifyskin = !modifyskin;
        }

        if (Input.GetKeyDown(increm))
        {
            if (modifyskin)
            {
                skinid++;
                skinid = skinid % skins.colors.Length;
                Debug.Log("skin number: " + skinid);
            }
            else
            {
                hairid++;
                hairid = hairid % hairtops.sprites.Length;
                Debug.Log("Hair number:" +hairid);
            }

            attr.skintint = skins.colors[skinid];
            attr.hairtop = hairtops.sprites[hairid];
            attr.SetPlayerStyle();
        }
    }
}

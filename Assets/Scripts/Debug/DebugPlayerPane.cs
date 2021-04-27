using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerPane : MonoBehaviour
{
    uint val = 0;
    KeyCode plus1 = KeyCode.UpArrow;
    KeyCode minus1 = KeyCode.DownArrow;
    KeyCode plusMore = KeyCode.RightArrow;
    KeyCode minusMore = KeyCode.LeftArrow;
    public PlayerUIPaneMgmt mgmt;
    public ParamNameDebugPlayerPane param;
    public uint incrVal=10; //value by which to increment larger changes

    public enum ParamNameDebugPlayerPane
    {
        SCORE,HEALTH
    }

    // Start is called before the first frame update
    void Start()
    {
        if (param == ParamNameDebugPlayerPane.HEALTH) mgmt.SetHealth(100, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(plus1))
        {
            val += 1;
        }
        if (Input.GetKeyDown(plusMore))
        {
            val += incrVal;
        }
        if (Input.GetKeyDown(minus1))
        {
            if (val > 0) val -= 1;
        }
        if (Input.GetKeyDown(minusMore))
        {
            if (val >= incrVal) val -= incrVal;
        }

        switch (param) {
            case ParamNameDebugPlayerPane.SCORE:
            default:
                mgmt.SetScore(val);
                break;
            case ParamNameDebugPlayerPane.HEALTH:
                mgmt.SetHealth(val,false);
                break;
        }
    }
}

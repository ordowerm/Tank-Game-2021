using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Silly little script that makes stuff change color randomly
 * 
 */
public class RainbowColorizer : MonoBehaviour
{
    public float targtime; //how often _target color updates
    public float speed; //multiplier for color interpolation
    public float minV; //minimum V value for HSV
    public float maxV;
    public float minS; //minimum S value for HSV
    public float maxS;
    public float alpha;


    SpriteRenderer sprite;
    Color current;
    Color target;
    float timer = 0;
    float lerpTimer=0;

    private Color RandomColor()
    {
        return Random.ColorHSV(0, 1, minS, maxS, minV, maxV,alpha,alpha);
    }

    private void CLerp()
    {
        sprite.color = Color.Lerp(current, target, lerpTimer);
        lerpTimer = Mathf.Min(1, lerpTimer + speed * Time.deltaTime);
    }

    private void UpdateColors()
    {
        timer += Time.deltaTime;
        if (timer > targtime)
        {
            current = target;
            target = RandomColor();
            timer = 0;
            lerpTimer = 0;
        }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        current = RandomColor();
        target = RandomColor();
        sprite.color = current;
    }

    private void Update()
    {
        UpdateColors();
        CLerp();
    }
}

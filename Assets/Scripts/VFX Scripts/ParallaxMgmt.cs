using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMgmt : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Vector2 playerYBounds; //maximum/minimum value of Player's y-coordinate. Use this to interpolate rotation angles.

    //Ground Layer Mgmt
    /*
     Big idea: ground is a GameObject with a bunch of children, each of which contains a SpriteRenderer.
    Ground rotates about x-axis to create illusion of depth.
     */
    public ParallaxLayers groundData; //
    public Vector2 groundSize; 
    public Vector2 groundPosition; //world space position of ground transform
    public Vector2 groundRotationRange; //Euler angles 
    float groundRotation;
    public GameObject ground; //container for array of ground parallax layers
    

    GameObject makeSpriteObject(ParallaxData p)
    {
        GameObject spriteObject = new GameObject();
        SpriteRenderer rend = spriteObject.AddComponent<SpriteRenderer>();
        rend.sprite = p.sprite;
        rend.drawMode = p.drawMode;
        rend.color = p.spriteTint;
        rend.size = p.size;

        return spriteObject;
    }

    //Lerp ground rotation
    float GetRotationFromPlayerY()
    {
        float result = playerYBounds.x;
        float prop = (player.transform.position.y - playerYBounds.x) / (playerYBounds.y - playerYBounds.x);
        result = Mathf.Lerp(groundRotationRange.x, groundRotationRange.y, prop);
        return result;
    }

    private void Start()
    {
        if (!ground)
        {
            ground = new GameObject();
            ground.name = "Ground";
        }

    }

    private void Update()
    {
        ground.transform.rotation = Quaternion.Euler(GetRotationFromPlayerY(), 0, 0);
    }
}

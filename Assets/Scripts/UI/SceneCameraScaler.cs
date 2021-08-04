using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Scale scene camera to force 1:1 aspect ratio, with black borders
 * 
 */
public class SceneCameraScaler : MonoBehaviour
{
    public Camera mainCam; //reference to the default main camera in the scene
    public Camera sceneCam; //reference to this script's gameObject's Camera component
    public float targetAspect; //_target aspect ratio --> UI is designed to match 1:1
    float  screenAspect;


    private void Awake()
    {
        //avoid user-error
        if (targetAspect <= 0)
        {
            targetAspect = 1;
        }

        Resize();

    }

    private void Update()
    {    
        //only rescale if there's a change in resolution
        if (screenAspect != mainCam.aspect)
        {
            Resize();
        }
    }


    private void Resize()
    {
        if (mainCam.aspect >= 1)
        {
            sceneCam.rect = new Rect((1 - targetAspect / mainCam.aspect) / 2.0f, 0, targetAspect / mainCam.aspect, 1);
        }
        else
        {
            sceneCam.rect = new Rect(0,(1-targetAspect*mainCam.aspect)/2.0f, 1, targetAspect * mainCam.aspect);
        }
        screenAspect = mainCam.aspect;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    public RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Width: " + rect.rect.width + ". Height: " + rect.rect.height); 
    }
}

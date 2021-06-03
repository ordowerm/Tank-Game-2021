using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAccessibleMaterials : MonoBehaviour
{
    public ElementData element;
    public SpriteRenderer[] renderers;
    public bool accessible;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool press = Input.GetKeyDown(KeyCode.G);
        if (press)
        {
            accessible = !accessible;
            SetAccessible(accessible);
        }
    }

    void SetAccessible(bool val)
    {
        if (val)
        {
            foreach (SpriteRenderer s in renderers)
            {
                s.color = Color.white;
                mat = s.material;
                mat.SetColor("_Color", Color.white);
                mat.SetTexture("_ElementTexture",element.elemTexture);
                mat.SetFloat("_AccessibleModeEnabled", 1);
            }
        }
        else
        {
            foreach (SpriteRenderer s in renderers)
            {
                s.color = Color.white;
                mat = s.material;
                mat.SetColor("_Color", element.primary);
                mat.SetFloat("_AccessibleModeEnabled", 0);
            }
        }
    }
}

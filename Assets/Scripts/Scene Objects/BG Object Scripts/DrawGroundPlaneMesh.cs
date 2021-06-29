using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 Programmatically creates the mesh.
 
 
 
 */
public class DrawGroundPlaneMesh : MonoBehaviour
{
    public Material mat;
    public MeshRenderer mesh;
    public Vector3[] vertices;
    public Vector2[] uvs;
    public Vector3[] normals;
    public int[] triangles;
    public float width;
    public float height;

    public Mesh MakeMesh()
    {
        Mesh result = new Mesh();

        result.vertices = vertices;
        result.uv = uvs;
        result.normals = normals;
        result.triangles = triangles;


        return result;
    }



    // Start is called before the first frame update
    void Start()
    {
        /*MeshRenderer meshR = gameObject.AddComponent<MeshRenderer>();
        meshR.material = mat;
        MeshFilter filt = gameObject.AddComponent<MeshFilter>();
        filt.mesh = MakeMesh();*/
        QuadMaker();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Taken from unity website
    void QuadMaker()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mat;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-width/2.0f, -height/2.0f, 0),
            new Vector3(width/2.0f, -height/2.0f, 0),
            new Vector3(-width/2.0f, height/2.0f, 0),
            new Vector3(width/2.0f, height/2.0f, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(5, 0),
            new Vector2(0, 1),
            new Vector2(5, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}

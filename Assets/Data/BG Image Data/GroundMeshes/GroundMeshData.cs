using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ground Mesh Data",menuName ="Game Data/Ground Mesh")]
public class GroundMeshData : ScriptableObject
{
    public QuadData[] quads;

    

    



}

[System.Serializable]
public class QuadData
{
    public Material material;
    public Vector2[] vertices;
    public Vector2[] uvs;
    public Vector2[] tileSize;
}


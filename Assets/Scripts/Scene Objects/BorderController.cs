using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * 
 * This class controls the "Walls" that box a player into a given region.
 * 
 * It should be loaded as part of the scene.
 * 
 *
 */
public class BorderController : MonoBehaviour
{
    //References to external objects in the scene
    public LevelManager mgmt;
    Camera sceneCamera;
    public float colliderThickness;

    //Background sprite
    public Sprite bgSprite;
    GameObject bgSpriteObject;
    public Color bgTint;


    /*
     * Even though Unity's Gizmos functionality can show colliders, I have to switch to the scene view to see them during runtime.
     * Accordingly, here are some debug thingies for displaying the colliders during gameplay
     * 
     */
    public bool debug;
    public Sprite debugSprite;
    void UseSpritesToDebugColliders()
    {
        foreach (WallStruct w in Walls)
        {
            SpriteRenderer sprender = w.Wall.GetComponent<SpriteRenderer>();
            
            //If no sprite renderer has been attached, create it.
            if (!sprender)
            {
                sprender = w.Wall.AddComponent<SpriteRenderer>();
                 sprender.sprite = debugSprite ;

            }
            sprender.size = w.collider.size;
        }
    }

    /*
     * References to the walls
     * 
     * I've stored them as a struct consisting of a GameObject and its BoxCollider so that I don't have to call GetComponent<BoxCollider2D> whenever the Colliders need to get resized.
     * It's a memory hit, but I'd rather take the memory hit than have an extra function call, not that it really matters given how infrequently it'll be called anyway.
     */
    private enum Dirs { North,South,East,West } //Use this to index walls if desired
    struct WallStruct
    {
        public GameObject Wall;
        public BoxCollider2D collider;
    }
    WallStruct[] Walls;


    //Spawns the walls and provides reasonable information for them.
    public void SpawnWalls()
    {
        Walls = new WallStruct[4];
        for (int i = 0; i < 4; i++)
        {
            Walls[i].Wall = new GameObject(); //spawns a new game object
            Walls[i].Wall.name = ((Dirs)i).ToString()+"Wall"; //names the walls
            Walls[i].Wall.transform.parent = this.transform; //makes each wall a child of this game object
            Walls[i].Wall.layer = this.gameObject.layer; //sets the layer of each wall equal to that of the border game object
            Walls[i].collider = Walls[i].Wall.AddComponent<BoxCollider2D>();
        }
    }

    //Sets the size of each wall object's BoxCollider to match the Camera bounds in world space.
    //This isn't the most efficient way to do things, assuming the camera remains orthographic, but if I switch to perspective for 3D effects, this could be useful, maybe.
    public void ResizeColliders()
    {
        //Get corners of viewport. Not strictly necessary
        Vector3[] corners = new Vector3[4]
        {
            new Vector3( 0,1,0 ),
            new Vector3( 1,1,0 ),
            new Vector3(1,0,0),
            new Vector3(0,0,0)
        };

        //Convert viewport extents to world coordinates to construct size boxes for each collider
        //Add a little extra size to the ends of each collider just in case
        float worldWidth = 2*sceneCamera.ViewportToWorldPoint(corners[1]-corners[0]).x;
        float worldHeight = 2*sceneCamera.ViewportToWorldPoint(corners[1] - corners[3]).y;
        Vector2 sizeHoriz = new Vector2(worldWidth+2*colliderThickness,colliderThickness);
        Vector2 sizeVert = new Vector2(colliderThickness, worldHeight + 2 * colliderThickness);

        //Assign colliders and place walls in respective location
        for (int i = 0; i<4; i++)
        {
            Walls[i].Wall.transform.localScale = new Vector3(1, 1, 1);
            switch ((Dirs)i)
            {
                case Dirs.East:
                    Walls[i].collider.size = sizeVert;
                    Walls[i].Wall.transform.position = new Vector3(sceneCamera.transform.position.x + worldWidth / 2.0f + colliderThickness / 2.0f, sceneCamera.transform.position.y, 0);
                    break;
                case Dirs.North:
                    Walls[i].collider.size = sizeHoriz;
                    Walls[i].Wall.transform.position = new Vector3(sceneCamera.transform.position.x , sceneCamera.transform.position.y + worldHeight/2.0f + colliderThickness / 2.0f, 0);
                    break;
                case Dirs.South:
                    Walls[i].collider.size = sizeHoriz;
                    Walls[i].Wall.transform.position = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y - worldHeight / 2.0f - colliderThickness / 2.0f, 0);

                    break;
                case Dirs.West:
                    Walls[i].collider.size = sizeVert;
                    Walls[i].Wall.transform.position = new Vector3(sceneCamera.transform.position.x - worldWidth / 2.0f - colliderThickness / 2.0f, sceneCamera.transform.position.y, 0);

                    break;

            }
        }

        ResizeBackgroundSprite(worldWidth, worldHeight);

        if (debug)
        {
            UseSpritesToDebugColliders();
        }

    }

    //Set scene camera and resize colliders
    public void SetSceneCamera(Camera c)
    {
        sceneCamera = c;
        ResizeColliders();
    }


    //Sizes the background sprite and places it in center of camera
    public void ResizeBackgroundSprite(float w, float h)
    {
        SpriteRenderer sr;

        if (!bgSpriteObject) {

            bgSpriteObject = new GameObject();
            sr = bgSpriteObject.AddComponent<SpriteRenderer>();
            bgSpriteObject.name = "Background Sprite";
            bgSpriteObject.transform.localScale = new Vector3(1, 1, 1);
            sr.color = bgTint;
            sr.sprite = bgSprite;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.sortingOrder = -10;
        }
        bgSpriteObject.transform.position = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, -3);
        sr = bgSpriteObject.GetComponent<SpriteRenderer>();
       
        sr.size = new Vector2(w, h);
    }


    private void Awake()
    {
        //Corrects collider thickness if an invalid value is provided
        if(colliderThickness <= 0)
        {
            colliderThickness = 1;
        }


        SpawnWalls();
    }
 }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 This spawns a stage, based on stage data.
 
 
 
 
 */
public class LevelManager : MonoBehaviour
{  
    public GameSettings settings; //On scene start, have the existing GameSettings instance pass a reference to itself into this LevelManager

    //Prefabs for constructing the game objects
    public GameObject uiMgmtPrefab;
    public GameObject playerPrefab;
    public StageData stageData;

    //Instances of each prefab
    GameObject uiMgmt;

    //Runtime Variables
    public bool sceneActive;


    public GameObject[] enemyObjects; //for testing purposes
    public GameObject[] playerDudes; //for testing purposes
    public PlayerVars[] playerVars; //stores current values of player parameters

    //Variables for facilitating lock-on functionality
    protected Dictionary<int, GameObject> enemyList;
    protected Dictionary<int, GameObject> playerList;
    protected Queue<int> returnedEnemyIds; //when an enemy is destroyed, return its ID number to a pool
    protected int enemyCount;

    public LevelUIManager levelUI;
    public float timeLimit; //timer's maximum value
    float timer;


    //Methods for building the stage from the stage data
    public void SetUpScene()
    {
        InstantiateUI();
    } 
    
    //Helper functions to call in SetUpScene()
    void InstantiateUI()
    {
        uiMgmt = Instantiate(uiMgmtPrefab);
        levelUI = uiMgmt.GetComponent<LevelUIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyList = new Dictionary<int, GameObject>();
        playerList = new Dictionary<int, GameObject>();
        returnedEnemyIds = new Queue<int>();

        //Test game object notification, etc.
        for (int i = 0; i<enemyObjects.Length; i++)
        {
            enemyObjects[i].GetComponent<EnemyStateMachine>().SetIDNumber(i);
            enemyList.Add(i, enemyObjects[i]);
        }
        for (int i = 0; i < playerDudes.Length; i++)
        {
            playerList.Add(i, playerDudes[i]);
        }

        timer = timeLimit;

    }

    //Send information of active enemies to the players 
    public void UpdatePlayerEnemyLists()
    {
        foreach (KeyValuePair<int,GameObject> p in playerList)
        {
            PlayerSM psm = p.Value.GetComponent<PlayerSM>(); //obtain reference to each player's state machine
            psm.SetEnemyPool();
        }
    }

    //When enemies are destroyed, call this from their OnDestroy method to tell the LevelManger to update the enemy list.
    public void NotifyEnemyDestroyed(int id)
    {
        enemyList.Remove(id);
        returnedEnemyIds.Enqueue(id);
        enemyCount--;
        UpdatePlayerEnemyLists();
    }

    //Gets the enemy list
    public Dictionary<int,GameObject> GetEnemyList()
    {
        return enemyList;
    }


    




    /*
     * Functionality related to funky camera stuff
     * 
     */
    /*
    public bool useFunkyCamera;
    public Camera mainCamera;
    public float maxCamAngle; 
    float maxElevation; //maximum elevation for camera
    public float playerMaxY;
    public Vector3 origin;
    public GameObject[] nonRotatables; //list of objects that should not be rotated 
    protected float GetPlayerAverageY()
    {
        float result = 0;
        foreach (KeyValuePair<int,GameObject> k in playerList)
        {
            GameObject g = k.Value;
            result += g.transform.position.y-origin.y;
        }
        result /= Mathf.Max(1, playerList.Count);

        return result;
    }
    protected void AdjustCameraY()
    {
        float prop = (playerMaxY+GetPlayerAverageY()) / (2.0f*playerMaxY); //proportion of max Y averaged by players
        float theta = maxCamAngle-prop*maxCamAngle;
        float newCamY = Mathf.Tan(theta * Mathf.PI / 180.0f) * (mainCamera.transform.position.z - origin.z); 
        mainCamera.transform.position = new Vector3(origin.x, newCamY, mainCamera.transform.position.z);
        mainCamera.transform.rotation = Quaternion.Euler(new Vector3(-theta, 0, 0));
        foreach (GameObject g in nonRotatables)
        {
            if (g)
            {
                g.transform.rotation = Quaternion.Euler(new Vector3(-theta, 0, 0));
            }
        }
    }
    */

    private void Update()
    {
        if (sceneActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                levelUI.SetTimerText(timer);
            }
            else if (timer < 0) { 
                timer = 0;
                sceneActive = false;
                levelUI.SetTimerText("Time\'s Up!");
                //Time.timeScale = 0.3f;
            }
            else
            {
                levelUI.SetTimerText("Time\'s Up!");

            }

        }

        /*if (useFunkyCamera)
        {
            AdjustCameraY();
        }*/
    }

}

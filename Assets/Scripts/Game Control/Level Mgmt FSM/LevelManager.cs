using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 This spawns a stage, based on stage data.
 
 It also manages the timing of each level
 
 
 */
public class LevelManager : GameStateMachine
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
    public BorderController border;
    int stageRegionNumber = 0; //index of currently-occupied area in the stage
    int enemyWaveNumber = 0; //index of the current wave of enemies

    //FSM States
    public LevelMGMTPregameState pregameState;
    public LevelMGMTRunState runState;
    public LevelMGMTPostgameState postgameState;


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



    //Methods for building the stage from the stage data at the start of a scene
    public void SetUpScene()
    {
        //Initialize maps
        enemyList = new Dictionary<int, GameObject>();
        playerList = new Dictionary<int, GameObject>();
        returnedEnemyIds = new Queue<int>();

        //Create game states
        pregameState = new LevelMGMTPregameState(this.gameObject, this);
        runState = new LevelMGMTRunState(this.gameObject, this);
        postgameState = new LevelMGMTPostgameState(this.gameObject, this);

        //Sets up the stage
        InstantiateUI();
        levelUI.SpawnPlayerPanes(settings.playerVars);
        border.SetSceneCamera(levelUI.mainCamera); //send reference of scene camera to wall object
        SpawnPlayers();

        //Marks scene as ready
        Initialize(pregameState);
        sceneActive = true;
    } 
    
    //Helper functions to call in SetUpScene()
    void InstantiateUI()
    {
        uiMgmt = Instantiate(uiMgmtPrefab);
        levelUI = uiMgmt.GetComponent<LevelUIManager>();
        levelUI.sceneMessage.levelManager = this; //pass reference to scene message manager
    }

    /*
     * Call when loading stage to spawn players
     * 
     * How it works:
     * Spawns an instance of the Player prefab, then grabs its PlayerSM and SkinAttributes scripts.
     * It assigns values to those scripts based on the values in the attached settings object.
     * 
     * 
     */
    public void SpawnPlayers()
    {
        int i = 0;
        foreach (PlayerVars p in settings.playerVars)
        {
            //Spawn player if the corresponding playerVars is active
            if (p.isActive)
            {
                GameObject newPlayerGuy = Instantiate(playerPrefab);
                SkinAttributes skin = newPlayerGuy.GetComponent<SkinAttributes>();
                skin.SetPlayerStyle(p);
                PlayerSM playerSM = newPlayerGuy.GetComponent<PlayerSM>();
                playerSM.playerId = i;
                playerSM.levelManager = this;
                playerList.Add(i, newPlayerGuy);
            }

            i++;
        }
    }




    //Methods for keeping track of players and enemies
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

        //If enemy count is down to 0, notify the current state that the enemy wave has ended
        if (enemyCount < 1)
        {
            Debug.Log("Level manager: No enemies left");
            enemyWaveNumber++;
            ((LevelMgmtState)currentState).NotifyEnemyWaveCleared();
        }

    }
    
    //The Stage Data defines various "regions" containing different enemy spawn behaviors. They're indexed by these methods:
    public void AdvanceRegion()
    {
        enemyWaveNumber = 0;
        stageRegionNumber++;
    }
    
    

    //Call when spawning a new enemy wave to display the number (+1, because humans usually count from 1)
    public int GetWaveNumber() { return enemyWaveNumber+1; }
    
    //Spawns an individual enemy and registers it to the list of active enemies
    public void SpawnEnemy(EnemySpawnData en)
    {
        
           GameObject newEnemy = Instantiate(en.enemy);
           newEnemy.transform.position = en.spawnLocation;
           int idNo;    
           
           //If there are returned enemy id numbers (from an enemy getting destroyed, for example), use one of the returned IDs; otherwise our Dictionary will keep getting bigger.
           if (returnedEnemyIds.Count > 0)
           {
               idNo = returnedEnemyIds.Dequeue();
           }
           else
           {
               idNo = enemyCount; 
           }
        EnemyStateMachine nsm = newEnemy.GetComponent<EnemyStateMachine>();
        nsm.SetOrientation(en.orientation);
        nsm.SetIDNumber(idNo);
        nsm.levelManager = this;
        nsm.data = en.edata;
        enemyCount++;
        enemyList.Add(idNo, newEnemy);
    }

    
    //Spawns a wave of enemies
    public void SpawnEnemyWave() {
       
        try
        {
            foreach (EnemySpawnData en in stageData.regions[stageRegionNumber].waves[enemyWaveNumber%stageData.regions[stageRegionNumber].waves.Length].enemies)
            {
                SpawnEnemy(en);

            }
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError("Region number: " + stageRegionNumber);
            Debug.LogError("Enemy Wave Number: " + enemyWaveNumber);
        }
    }
    
    //Gets the enemy list
    public Dictionary<int,GameObject> GetEnemyList()
    {
        return enemyList;
    }



    //Methods for updating other variables
    public void IncrementPlayerScore(int playerId, int amt)
    {
        try
        {
            playerVars[playerId].playerScore += amt;
            uiMgmt.GetComponent<LevelUIManager>().UpdateUIParams(playerId);
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            Debug.Log("In IncrementPlayerScore: playerId out of a range: " + e);
        }
    }








    //Methods for controlling the textbox that appears in the center of the playfield.
    //Sends message to LevelUIManager
    public void SendUIMessage(SceneOverlayMessage s)
    {
        levelUI.EnqueueOverlayMessage(s);
        levelUI.RunOverlayMessage();
    }
    public void SendUIMessages(SceneOverlayMessage[] s)
    {
       // Debug.Log("LevelManager: Sending messages");
        //Debug.Log(levelUI.ToString());
        levelUI.EnqueueOverlayMessages(s);
        levelUI.RunOverlayMessage();
    }
    //Call when scene overlay message is finished displaying
    public void NotifySceneMessageFinishedRendering()
    {
        //Debug.Log("In level Manger: scene message done rendering. Notifying the state: "+currentState.ToString());
        ((LevelMgmtState)currentState).NotifyMessageFinished();
        
    }




    //MonoBehaviour overrides
    override protected void Update()
    {
       if (sceneActive)
        {
            base.Update();
        }
      
    }

    protected override void FixedUpdate()
    {
        if (sceneActive)
        {
            base.FixedUpdate();

        }
    }

}



/*
 * Functionality related to funky camera stuff that I got rid of.
 * It was causing motion sickness.
 * 
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

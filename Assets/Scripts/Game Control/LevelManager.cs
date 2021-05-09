using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] enemyObjects; //for testing purposes
    public GameObject[] playerDudes; //for testing purposes

    protected Dictionary<int, GameObject> enemyList;
    protected Dictionary<int, GameObject> playerList;
    protected Queue<int> returnedEnemyIds; //when an enemy is destroyed, return its ID number to a pool
    protected int enemyCount;
    
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

        

    }

    public void UpdatePlayerEnemyLists()
    {
        foreach (KeyValuePair<int,GameObject> p in playerList)
        {
            PlayerSM psm = p.Value.GetComponent<PlayerSM>(); //obtain reference to each player's state machine
            psm.SetEnemyPool();
        }
    }

    public void NotifyEnemyDestroyed(int id)
    {
        enemyList.Remove(id);
        returnedEnemyIds.Enqueue(id);
        enemyCount--;
        UpdatePlayerEnemyLists();
    }

    public Dictionary<int,GameObject> GetEnemyList()
    {
        return enemyList;
    }



    /*
     * Functionality related to funky camera stuff
     * 
     */
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

    private void Update()
    {
        if (useFunkyCamera)
        {
            AdjustCameraY();
        }
    }

}

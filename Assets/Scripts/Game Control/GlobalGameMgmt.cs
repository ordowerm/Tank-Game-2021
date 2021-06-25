using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


/*
 This script should be called at the start of the game. It should be attached to the game manager.
  
 */

public class GlobalGameMgmt : MonoBehaviour
{
    public bool debugMode;
    public GameSettings settings;

    /*
     This string should contain the name of the scene that we use as a template for programmatically generating the level.
     The scene should contain a GameObject with a LevelManager component.
     Upon loading the template scene, this script should call GameObject.FindObjectByTag to locate the GameObject in the template scene
     that contains a LevelManager component.
          */
    public string levelTemplateScenePath;
    const string levelMgmtTag = "LevelMgmt"; //hardcoded tag for the LevelMgmt object for use with FingObjectByTag



    LevelManager levelMgmt; //assign this when a scene containing a LevelManager is loaded. Since the scene should have very few active Game Objects, using GameObject.Find shouldn't cause a big performance hit.


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); //mark game manager as persistent across scenes
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
        {

        }
    }

 
    //Sets the game settings from a file path
    public bool LoadGameSettingsFromFile(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = File.OpenRead(path);
                settings = (GameSettings)bf.Deserialize(stream);
                stream.Close();
                return true;
            }
            catch (System.InvalidCastException e)
            {
                Debug.Log("Invalid game settings: "+e);
                return false;
            }
        }
        else
        {
            Debug.Log("Invalid file path when loading game settings.");
            return false;
        }

    }


    //Causes scene change 
    public void PrepareStage(StageData sd)
    {
        StartCoroutine(LoadScene(sd));
        
    }

    //Use coroutine to defer instantiation of UI stuff until the scene is fully loaded
    IEnumerator LoadScene(StageData sd)
    {
       AsyncOperation async =  SceneManager.LoadSceneAsync(levelTemplateScenePath, LoadSceneMode.Single);
        while (!async.isDone)
        {
            yield return null;
        }
        
        GameObject temp = GameObject.FindGameObjectWithTag(levelMgmtTag);
        if (!temp)
        {
            Debug.Log("Error: no manager found");
        }
        levelMgmt = temp.GetComponent<LevelManager>();
        levelMgmt.settings = this.settings;
        if (!levelMgmt.stageData && sd)
        {
            levelMgmt.stageData = sd;
        }
        
        levelMgmt.SetUpScene();
    }


}

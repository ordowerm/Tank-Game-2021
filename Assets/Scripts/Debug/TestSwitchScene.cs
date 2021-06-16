using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSwitchScene : MonoBehaviour
{
    public bool pressed;
    public string nextScene;
    public KeyCode key;
    public GlobalGameMgmt mgmt;
    public StageData sd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && !pressed)
        {
            mgmt.PrepareStage(sd);
            //SceneManager.LoadScene(sceneName:nextScene, LoadSceneMode.Single);
            pressed = true;
        }
    }
}

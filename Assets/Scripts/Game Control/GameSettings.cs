using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This script should be called on the title screen.
 * Mark it "DontDestroyOnLoad".
 * It should have references to the default settings for each character, level, etc.
 * Pass variables into level manager of standard game scenes.
 * 
 */

public class GameSettings : MonoBehaviour
{
    public float timeLimit;
    public PlayerVars[] playerVars;
    public bool accessibleMode; //turning this on causes the game to load alternate shaders for the color-impaired
    public float sfxVolume; //volume of sound effects
    public float musicVolume;
    public bool enableParticleEffects;
    public bool deterministicAI; //when set to true, this should force enemies to have super-predictable movement -- minimize calls to Random


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This contains runtime-specific game settings.
 * The GameManager should have a MonoBehaviour containing a GameSettings field.
 * 
 */

[System.Serializable]
public class GameSettings
{
    public PlayerVars[] playerVars;
    public bool accessibleMode; //turning this on causes the game to load alternate shaders for the color-impaired
    public float sfxVolume; //master volume for sound effects
    public float musicVolume;
    public bool enableParticleEffects;
    public bool deterministicAI; //when set to true, this should force enemies to have super-predictable movement -- minimize calls to Random
    public bool useLighting; //I might not end up using this, but if I use diffuse shaders, I want them to be toggleable.
    public bool useTimeLimits; //if this is activated, then each level will have a timer, and if the timer reaches zero, then it's game over.
    public bool oneHitKO; //if this is activated, players will die in one hit 
}

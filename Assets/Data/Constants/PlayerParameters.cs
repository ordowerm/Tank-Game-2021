using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerParams", menuName ="GameParameters/PlayerParams")]
public class PlayerParameters : ScriptableObject
{
    public float walkspeed;
    public float rollspeed; //max speed of roll
    public float rolltime;
    public AnimationCurve rollcurve;
    public float rolldelay; //delay time before you can roll again

    //Aiming parameters
    /*
     * Controller lock on is accomplished by casting rays from a player's gun to all enemies on screen.
     * It then sorts the enemies, first by angle error, then by distance, to determine which enemy to lock onto first.
     * 
     * maxAngleError -> this value determines the maximum angle error, between the angle the player's gun is facing and a given enemy position, before the game says, "You can't lock onto that enemy."
     * lockOnCancelTime-> if the lock-on button is released and then tapped again within lockOnCancelTime seconds, then instead of cancelling the lock-on, the focus will advance to the next-closest enemy
     */
    public float maxAngleError;
    public float lockOnCancelTime; 

}

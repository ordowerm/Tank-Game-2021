using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * 
 * This script mostly contains physics constants for use by player-controlled characters.
 * Player State Machine (PlayerSM) should maintain a reference to these settings.
 * Indivdual PlayerStates should request their parent PlayerSM's reference to this script and then use it to perform their actions.
 * 
 */

[CreateAssetMenu(fileName ="PlayerParams", menuName ="Game Parameters/Player Physics Params")]
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
     * It then sorts the enemies, using a heuristic function approximating the best enemy to aim at first.
     * The "score" from the heuristic considers enemy distance from player + how far away the enemy is from the player's aiming angle.
     * The degree to which these two variables are weighted are defined in these player parameters.
     * 
     * maxAngleError -> this value determines the maximum angle error, between the angle the player's gun is facing and a given enemy position, before the game says, "You can't lock onto that enemy."
     * lockOnCancelTime-> if the lock-on button is released and then tapped again within lockOnCancelTime seconds, then instead of cancelling the lock-on, the focus will advance to the next-closest enemy
     *
     * The aiming heuristic weights are used when determining the priorities of which enemies to lock onto.
     *
     */
    public float maxAngleError;
    public float maxEnemyDistanceForLockOn;
    public float lockOnCancelTime;
    public float lockOnResetTime; //if player stops locking on and then waits for a bit, lock on to nearest enemy again
    public float aimingHeuristicAngleErrorWeight;
    public float aimingHeuristicEnemyDistanceWeight;

}

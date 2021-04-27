using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This class contains current values for player variables.
 This should be stored in an array or list of currently active players in the game controller.
 It should contain references to the Player's UI pane, State Machine, etc.
 It should update those fields as needed.
 */

[System.Serializable]
public class PlayerVars
{
    public string PlayerName;
    public int PlayerNumber;

}


/*
 * Script for updating the UI panel during gameplay
 * The PlayerVars script should maintain a reference to this.
 * When values are updated, call methods in this script to update UI.
 */

//Enumerate states controlling UI layout
public enum PlayerUIPaneState
{
    NO_PLAYER, //if no player is active
    PLAYER_ACTIVE, //regular in-game state
    TIME_UP, //if player is active, but time is up
    LEVEL_END, //if player is active, and level has been won
    GAME_OVER //
}

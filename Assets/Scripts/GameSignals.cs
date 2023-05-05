using Enums;
using UnityEngine.Events;

public class GameSignals
{
    public static UnityAction<int> OnUpdateMoveCount;
    public static UnityAction<bool> OnCanUndoSwap;

    public static UnityAction<GameStates> OnGameState;
    public static UnityAction<int> OnMoveCount;
    public static UnityAction<int> OnScore;

    public static UnityAction<int> OnLiveEnemy;
    public static UnityAction<int> OnDay;

    public static UnityAction<bool> OnRemoveCell;

    public static UnityAction OnGameStarting;

    public static UnityAction OnGameOver;
}

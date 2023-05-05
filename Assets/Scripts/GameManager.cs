using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int moveCount;
    public int score;
    public int day;
    public bool canUndoSwap;
    public bool isGameStarting;

    public List<Cell> basicCells;

    public GameStates gameState;
    public GameStates GameState { 
        get { return gameState; } 
        set 
        {
            gameState = value;
            GameSignals.OnGameState.Invoke(gameState);
        } 
    }

    public int liveEnemyCount;

    private void OnEnable()
    {
        GameSignals.OnUpdateMoveCount += UpdateMoveCount;
        EnemySignals.OnUpdateLiveEnemyCount += UpdateLiveEnemyCount;
        EnemySignals.OnCurrentWave += (wave) => moveCount = wave.waveMoveCount;
    }

    private void OnDisable()
    {
        GameSignals.OnUpdateMoveCount -= UpdateMoveCount;
        EnemySignals.OnUpdateLiveEnemyCount -= UpdateLiveEnemyCount;
        EnemySignals.OnCurrentWave -= (wave) => moveCount = wave.waveMoveCount;
    }

    private void Start()
    {
        day = 1;
        GameState = GameStates.matching;
        GameSignals.OnScore?.Invoke(0);
        GameSignals.OnDay?.Invoke(day);
    }

    private void UpdateLiveEnemyCount(int count)
    {
        liveEnemyCount += count;

        //enemy yok edilidiğinde
        if (count < 0)
        {
            score += 10;
            GameSignals.OnScore?.Invoke(score);
        }

        //canlı enemyler bittiğinde
        if (liveEnemyCount <= 0)
        {
            Debug.Log("MATCHİNG..........!");

            GameState = GameStates.matching;
            EnemySignals.OnClearCurrentWave?.Invoke();
            UpdateDay();
        }
    }

    public void UpdateDay()
    {
        day++;
        GameSignals.OnDay?.Invoke(day);
    }

    private void UpdateMoveCount(int count)
    {
        moveCount += count;

        if (moveCount <= 0)
        {
            StartCoroutine(MoveCountControl());
        }
        GameSignals.OnMoveCount?.Invoke(moveCount);
    }

    private bool CellsMovingControl()
    {
        var grid = Board.Instance.grid;

        for (int i = 0; i < grid.GetLength(0) * grid.GetLength(1); i++)
        {
            int row = i / grid.GetLength(1);
            int col = i % grid.GetLength(1);

            if (grid[row, col].moving)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator MoveCountControl()
    {
        Debug.Log("WAİTİNG.........!");
        yield return new WaitForSecondsRealtime(1.5f);
        if (moveCount <= 0)
        {
            Debug.Log("DEFENDİNG............!");
            GameState = GameStates.defending;
        }
    }
}

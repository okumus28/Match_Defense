using UnityEngine;
using Enums;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public Wave[] waves;

    public int currentWaveIndex;

    public float offsetY;

    public ObjectPool<Enemy> enemies;

    private void OnEnable()
    {
        GameSignals.OnGameState += GetCurrentState;
        EnemySignals.OnClearCurrentWave += ClearCurrentLevel;
    }

    private void OnDisable()
    {
        GameSignals.OnGameState -= GetCurrentState;
        EnemySignals.OnClearCurrentWave -= ClearCurrentLevel;
    }

    private void Start()
    {
        enemies = new (waves[0].enemyPrefabs, 10, this.transform);

        GameSignals.OnUpdateMoveCount?.Invoke(GetCurrentWave().waveMoveCount);
    }
    private void ClearCurrentLevel()
    {
        currentWaveIndex++;
        GameSignals.OnUpdateMoveCount?.Invoke(GetCurrentWave().waveMoveCount);
    }

    public Wave GetCurrentWave()
    {
        currentWaveIndex = currentWaveIndex > waves.Length ? 0 : currentWaveIndex;
        return waves[currentWaveIndex];
    }

    private void GetCurrentState(GameStates gameState)
    {
        if (gameState == GameStates.defending)
        {
            SpawnWaveEnemies(currentWaveIndex);
        }
    }
    public void SpawnWaveEnemies(int index)
    {
        if (!waves[index].boss)
        {
            for (int i = 1; i <= waves[index].enemyCount; i++)
            {
                float rndX = Random.Range(-1.75f, 3.25f);
                //e.transform.localPosition = new Vector3(rndX, offsetY - Random.Range(1f,5f), 0);
                Enemy e = enemies.GetObject(new Vector3(rndX, offsetY - Random.Range(1f, 5f), 0));
                e.Init();
            }
        }
    }
}

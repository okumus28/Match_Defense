using System;
using UnityEngine.Events;

public class EnemySignals
{
    public static UnityAction<int> OnUpdateLiveEnemyCount;
    public static UnityAction<int> OnCurrentWaveIndex;
    public static UnityAction<Wave> OnCurrentWave;
    public static UnityAction OnClearCurrentWave;
}

using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IBuilding : MonoBehaviour
{
    public BuildingState currentState;

    private void OnEnable()
    {
        GameSignals.OnGameState += GetGameState;
    }

    private void OnDisable()
    {
        GameSignals.OnGameState -= GetGameState;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BuildingState.matching:
                BuildingMatch();
                StopAllCoroutines();
                break;
            case BuildingState.attacking:
                AttackTarget();
                break;
            case BuildingState.finding:
                FindTarget();
                break;
            default:
                break;
        }
    }

    protected abstract void BuildingMatch();

    protected abstract void FindTarget();

    protected abstract void AttackTarget();

    private void GetGameState(GameStates gameStates)
    {
        if (gameStates == GameStates.matching)
        {
            currentState = BuildingState.matching;
        }
        else
        {
            currentState = BuildingState.finding;
        }
    }
}
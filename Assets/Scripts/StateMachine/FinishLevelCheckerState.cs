using System;
using UnityEngine;

public class FinishLevelCheckerState : IEnterableState
{
    public Action OnLevelFinish;

    private readonly MapBuilder _mapBuilder;
    private StateMachine _stateMachine;

    public FinishLevelCheckerState(MapBuilder mapBuilder)
    {
        _mapBuilder = mapBuilder;
    }

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        StartCheck();
    }

    public void MoveToStartLevelState()
    {
        _stateMachine.Enter<StartLevelState>();
    }

    private void StartCheck()
    {
        if (CheckFinishGame())
        {
            Debug.Log("finish");
            OnLevelFinish?.Invoke();
        }
        else
        {
            _stateMachine.Enter<CatchStartPointState>();
        }
    }

    private bool CheckFinishGame()
    {
        var mapChips = _mapBuilder.MapChips;
        var miniMapChips = _mapBuilder.MinimapChips;

        for (int i = 0; i < mapChips.Count; i++)
        {
            var currentChipPosition = mapChips[i].CurrentPosition;
            var winChipPosition = miniMapChips[i].CurrentPosition;

            if (currentChipPosition != winChipPosition)
            {
                return false;
            }
        }

        return true;
    }
}
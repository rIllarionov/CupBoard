using System;
using UnityEngine;

public class FinishLevelCheckerState : MonoBehaviour, IEnterableState, IExitableState
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private UiController _uiController;

    private Action _onLevelFinish;

    private StateMachine _stateMachine;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        _onLevelFinish += _uiController.ShowButton;
        StartCheck();
    }

    public void OnExit()
    {
        _onLevelFinish -= _uiController.ShowButton;
    }

    public void MoveToStartLevelState()
    {
        _stateMachine.Enter<StartLevelState>();
    }

    private void StartCheck()
    {
        if (CheckFinishGame())
        {
            _onLevelFinish?.Invoke();
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
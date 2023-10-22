using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : MonoBehaviour, IEnterebleStateWithContext, IExitableState
{
    [SerializeField] private Mover _mover;
    [SerializeField] private MapBuilder _mapBuilder;

    private Action<Transform, List<Point>, float> _onMove;

    private Point _startPoint;
    private List<Point> _path;

    private StateMachine _stateMachine;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnExit()
    {
        _onMove -= _mover.Move;
    }

    public void OnEnter(IExitableStateWithContext exitableState)
    {
        _onMove += _mover.Move;
        ReadContext(exitableState);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var moveDuration = 1f;

        var chipTransform = _startPoint.Chip.transform;
        _onMove?.Invoke(chipTransform, _path, moveDuration);
        ReplaceChip();

        yield return new WaitForSeconds(moveDuration);

        _stateMachine.Enter<FinishLevelCheckerState>();
    }

    private void ReadContext(IExitableStateWithContext exitableState)
    {
        var pathFinderState = (PathFinderState)exitableState;

        _startPoint = pathFinderState.StartPoint;
        _path = pathFinderState.Path;
    }

    private void ReplaceChip()
    {
        var currentChip = _startPoint.Chip;
        _path[^1].SetChip(currentChip);
        currentChip.CurrentPosition = _mapBuilder.MapPoints.IndexOf(_path[^1]);
        _startPoint.ClearPoint();
    }
}
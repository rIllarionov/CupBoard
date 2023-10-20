using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : MonoBehaviour, IEnterebleStateWithContext, IExitableState
{
    //передать ссылки иначе?
    
    [SerializeField] private Mover _mover;
    [SerializeField] private MapBuilder _mapBuilder;

    //медиатор?
    
    public Action<Transform, List<Point>, float> OnMove;

    //поле финиш лишнее
    
    private Point _startPoint;
    private Point _finishPoint;
    private List<Point> _path;

    private StateMachine _stateMachine;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnExit()
    {
        OnMove -= _mover.Move;
    }

    public void OnEnter(IExitableStateWithContext exitableState)
    {
        OnMove += _mover.Move;
        ReadContext(exitableState);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        var moveDuration = 1f;

        var chipTransform = _startPoint.Chip.transform;
        OnMove?.Invoke(chipTransform, _path, moveDuration);
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
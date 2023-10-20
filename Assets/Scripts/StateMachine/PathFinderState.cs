using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderState : MonoBehaviour, IEnterebleStateWithContext, IExitableStateWithContext
{
    //передать ссылки иначе?
    
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private HighLighter _highLighter;

    public Point StartPoint { get; private set; }
    
    //финиш не требуется для передачи
    public Point FinishPoint { get; private set; }
    public List<Point> Path { get; private set; }

    private readonly PathFinder _pathFinder = new PathFinder();
    private StateMachine _stateMachine;

    private bool _isActive;

    private List<Point> _availablePaths;

    private Action<List<ILightable>, bool> _turnAllLights;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(IExitableStateWithContext exitableState)
    {
        _turnAllLights += _highLighter.TurnAllLights;

        ReadContext(exitableState);
        FindPathsFrom(StartPoint);

        _isActive = true;
    }

    public IExitableStateWithContext OnExit()
    {
        _isActive = false;
        _turnAllLights -= _highLighter.TurnAllLights;
        return this;
    }

    private void Update()
    {
        if (_isActive && Input.GetMouseButtonDown(0))
        {
            var finishPoint = GetFinishPoint();

            if (finishPoint != null)
            {
                FinishPoint = finishPoint;
                Path = _pathFinder.GetPath(StartPoint, FinishPoint);

                _turnAllLights?.Invoke(new List<ILightable>(_mapBuilder.MapPoints), false);
                _stateMachine.Enter<MovingState>();
            }
            else
            {
                _turnAllLights?.Invoke(new List<ILightable>(_mapBuilder.MapPoints), false);
                _stateMachine.Enter<CatchStartPointState>();
            }
        }
    }

    private Point GetFinishPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info))
        {
            if (info.collider.CompareTag("Point"))
            {
                var point = info.collider.GetComponent<Point>();

                if (_availablePaths.Contains(point))
                {
                    return point;
                }
            }
        }

        return null;
    }

    private void ReadContext(IExitableStateWithContext exitableState)
    {
        var catchStartPointState = (CatchStartPointState)exitableState;
        StartPoint = catchStartPointState.StartPoint;
    }

    private void FindPathsFrom(Point startPoint)
    {
        _availablePaths = _pathFinder.FindAvailablePoints(startPoint);
        _turnAllLights?.Invoke(new List<ILightable>(_availablePaths), true);
    }
}
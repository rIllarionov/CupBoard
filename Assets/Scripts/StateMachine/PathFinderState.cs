using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderState : MonoBehaviour, IEnterebleStateWithContext, IExitableStateWithContext
{
    [SerializeField] private MapBuilder _mapBuilder;
    public Point StartPoint { get; private set; }
    public List<Point> Path { get; private set; }

    private readonly PathFinder _pathFinder = new();
    private readonly HighLighter _highLighter = new();

    private StateMachine _stateMachine;

    private Action<List<ILightable>, bool> _turnAllLights;

    private List<Point> _availablePaths;

    private bool _isActive;


    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(IExitableStateWithContext exitableState)
    {
        _turnAllLights += _highLighter.SwitchLights;

        ReadContext(exitableState);
        FindPathsFrom(StartPoint);

        _isActive = true;
    }

    public IExitableStateWithContext OnExit()
    {
        _isActive = false;
        _turnAllLights -= _highLighter.SwitchLights;
        return this;
    }

    private void Update()
    {
        if (_isActive && Input.GetMouseButtonDown(0))
        {
            _turnAllLights?.Invoke(new List<ILightable>(_mapBuilder.MapPoints), false);
            
            var finishPoint = GetFinishPoint();

            if (finishPoint != null)
            {
                Path = _pathFinder.GetPath(StartPoint, finishPoint);
                _stateMachine.Enter<MovingState>();
            }
            else
            {
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

                //вынести в отдельный метод?
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
using System;
using System.Collections.Generic;
using UnityEngine;

public class CatchStartPointState : MonoBehaviour, IEnterableState, IExitableStateWithContext
{
    [SerializeField] private MapBuilder _mapBuilder;
    public Point StartPoint { get; private set; }

    private StateMachine _stateMachine;
    private readonly HighLighter _highLighter = new();
    private Action<List<ILightable>, bool> _turnAllLights;
    private bool _isActive;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        _turnAllLights += _highLighter.SwitchLights;
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
        if (_isActive && Input.GetMouseButtonUp(0))
        {
            _turnAllLights?.Invoke(new List<ILightable>(_mapBuilder.MapPoints), false);

            var startPoint = GetStartPoint();

            if (startPoint != null)
            {
                StartPoint = startPoint;

                _turnAllLights?.Invoke(new List<ILightable> { StartPoint.Chip }, true);
                _stateMachine.Enter<PathFinderState>();
            }
        }
    }

    private Point GetStartPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info))
        {
            if (info.collider.CompareTag("Point"))
            {
                var point = info.collider.GetComponent<Point>();
                if (!point.IsAvailable)
                {
                    return point;
                }
            }
        }

        return null;
    }
}
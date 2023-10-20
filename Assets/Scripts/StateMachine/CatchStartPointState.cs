using System;
using System.Collections.Generic;
using UnityEngine;

public class CatchStartPointState : MonoBehaviour, IEnterableState, IExitableStateWithContext
{
    //передать ссылки иначе?
    
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private HighLighter _highLighter;

    public Point StartPoint { get; private set; }

    //подписаться в медиаторе?
    private Action<List<ILightable>, bool> _turnAllLights;
    private Action<ILightable, bool> _onGetPoint;

    private StateMachine _stateMachine;
    private bool _isActive;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        _turnAllLights += _highLighter.TurnAllLights;
        _onGetPoint += _highLighter.TurnLight;

        _isActive = true;
    }

    public IExitableStateWithContext OnExit()
    {
        _isActive = false;
        _turnAllLights -= _highLighter.TurnAllLights;
        _onGetPoint += _highLighter.TurnLight;

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

                _onGetPoint?.Invoke(StartPoint.Chip, true);
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
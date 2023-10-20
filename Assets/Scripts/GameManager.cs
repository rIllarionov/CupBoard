using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StateMachine _stateMachine;

    private void Start()
    {
        _stateMachine =
            new StateMachine(GetComponent<StartLevelState>(),
                GetComponent<CatchStartPointState>(),
                GetComponent<PathFinderState>(),
                GetComponent<MovingState>(),
                GetComponent<FinishLevelCheckerState>());

        _stateMachine.Enter<StartLevelState>();
    }
}
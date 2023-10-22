using UnityEngine;

public class GameManager : MonoBehaviour
{
    //можно переклить все связи с событиями и подписываться тут 
    
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
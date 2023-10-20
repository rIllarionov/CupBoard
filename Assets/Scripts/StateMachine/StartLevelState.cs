using System.Collections.Generic;
using UnityEngine;

public class StartLevelState : MonoBehaviour, IEnterableState, IExitableState
{
    //передать ссылки иначе + переключить ui на события?
    
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private UiController _uiController;

    private GameSettingsHolder _gameSettingsHolder;

    private StateMachine _stateMachine;

    private List<string[]> _levelsSettings;

    private int _levelNumber;

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        ReadLevelsSettings();
    }

    public void OnExit()
    {
        _levelNumber++;
    }

    public void OnEnter()
    {
        //разбить на методы? 
        
        if (_levelNumber > 0)
        {
            _mapBuilder.DestroyMap();
        }

        if (_levelNumber == _levelsSettings.Count)
        {
            _mapBuilder.DestroyMap();
            _uiController.ShowTitle();
            return;
        }

        var settings = _levelsSettings[_levelNumber];

        _gameSettingsHolder = new GameSettingsHolder();
        _gameSettingsHolder.Initialize(settings);
        _mapBuilder.Build(_gameSettingsHolder);

        _stateMachine.Enter<CatchStartPointState>();
    }

    private void ReadLevelsSettings()
    {
        _levelsSettings = new List<string[]>
        {
            FileRider.ReadFile("Assets/GameSettings/test.txt"),
            FileRider.ReadFile("Assets/GameSettings/test1.txt")
        };
    }
}
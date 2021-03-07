using UnityEngine;
using UnityEngine.Events;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private CutSceneOfLevelChanged _cutSceneOfLevelChanged;
    [SerializeField] private Player _player;
    [SerializeField] private SingTable _singTable;

    private int _currentLevelIndex = 0;
    private Level _currentLevel;

    public event UnityAction LevelsEnding;
    public event UnityAction LevelEnding;

    private void OnEnable()
    {
        TryGenerateLevel();
    }

    private void OnDisable()
    {
        foreach(var level in _levels)
        {
           level.LevelCompletion -= LevelComplited;
        }
    }

    private void TryGenerateLevel()
    {
        if(_currentLevelIndex <_levels.Length & _currentLevelIndex >= 0)
        {
            _currentLevel =Instantiate(_levels[_currentLevelIndex], transform);

            _currentLevel.Generate(_player);
            _currentLevel.LevelCompletion += LevelComplited;

            _currentLevelIndex++;
        }
        else if(_currentLevelIndex>=_levels.Length)
        {
            EndLevels();
        }        
    }

    private void LevelComplited()
    {
        _cutSceneOfLevelChanged.ShowCutScene();
        _currentLevel.LevelCompletion -= LevelComplited;
        LevelEnding?.Invoke();

        _currentLevel.gameObject.SetActive(false);
        TryGenerateLevel();
    }

    private void EndLevels()
    {
        LevelsEnding?.Invoke();

    }

    public void SpawnSingTable(Transform playerTransform)
    {
        _currentLevel.SpawnSignTable( _singTable, playerTransform);
    }
}
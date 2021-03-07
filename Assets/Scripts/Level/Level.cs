using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private EnemysOnLevel[] _enemysOnLevel;
    [SerializeField] private ChestOnLevel _chestOnLevel;
    [SerializeField] private int _heightMaze;
    [SerializeField] private int _widthMaze;
    [SerializeField] private Maze _maze;
    [SerializeField] private float _durationExitAnimation;

    private Door _exitOnLevel;
    private Player _player;

    public event UnityAction LevelCompletion;

    private void OnDisable()
    {
        _exitOnLevel.Opened -= WaitingEndExitAnimation;
    }

    private void WaitingEndExitAnimation()
    {
        StartCoroutine(WaitingEndAnimationCoroutine());
    }

    private IEnumerator WaitingEndAnimationCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < _durationExitAnimation)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ExitOfLevel();
    }

    private void ExitOfLevel()
    {
        _exitOnLevel.Opened -= ExitOfLevel;
        LevelCompletion?.Invoke();
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < _enemysOnLevel.Length; i++)
        {
            _maze.SpawnEnemy(_enemysOnLevel[i].EnemyTemplate, _enemysOnLevel[i].Count);
        }
    }

    public void Generate(Player player)
    {
        _player = player;
        _maze.GenerateNewMaze(_widthMaze, _heightMaze,_player);
        _maze.SpawnMaze();
        SpawnChests();
        SpawnEnemies();
        _exitOnLevel = _maze.Door;
        _exitOnLevel.Opened += WaitingEndExitAnimation;
    }

    public void SpawnSignTable(SingTable tablePrefab, Transform playerTransform)
    {
        var table =Instantiate(tablePrefab, transform);

        table.transform.position = playerTransform.position;
        table.transform.rotation = playerTransform.rotation;
    }

    public void SpawnChests()
    {
            _maze.SpawnChests(_chestOnLevel.Chest,_chestOnLevel.Count);
    }
}

[System.Serializable]
public class EnemysOnLevel
{
    [SerializeField] private Enemy _template;
    [SerializeField] private int _count;

    public Enemy EnemyTemplate => _template;
    public int Count => _count;
}

[System.Serializable] 
public class ChestOnLevel
{
    [SerializeField] private Chest _template;
    [SerializeField] private int _count;

    public Chest Chest => _template;
    public int Count => _count;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private GameEndingPanel _gameEndingPanel;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerSpawnPoint;

    private float _elapsedTime = 0;

    private void OnEnable()
    {
        _levelGenerator.LevelsEnding += FinishGame;
        _levelGenerator.LevelEnding += SpawnPlayerOnStartPosition;
        SpawnPlayerOnStartPosition();
    }
    private void OnDisable()
    {
        _levelGenerator.LevelsEnding -= FinishGame;
        _levelGenerator.LevelEnding -= SpawnPlayerOnStartPosition;
    }

    private void SpawnPlayerOnStartPosition()
    {
        _player.transform.position = _playerSpawnPoint.position;
    }

    private void FinishGame()
    {
        _gameEndingPanel.Activate((int)_elapsedTime);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }
}
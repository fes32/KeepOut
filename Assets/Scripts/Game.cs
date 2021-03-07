using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private GameEndingPanel _gameEndingPanel;

    private float _elapsedTime = 0;

    private void OnEnable()
    {
        _levelGenerator.LevelsEnding += FinishGame;
    }
    private void OnDisable()
    {
        _levelGenerator.LevelsEnding -= FinishGame;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
        _player.Dying += GameOver;
    }
    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
        _resumeButton.onClick.RemoveListener(OnResumeButtonClick);
        _player.Dying -= GameOver;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnResumeButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
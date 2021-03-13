using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private PlayerInput _playerController;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Toggle _music;

    public event UnityAction MusicOn;
    public event UnityAction MusicOff;

    private void OnEnable()
    {
        _playerController.Pause +=OnPause;
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
        _music.onValueChanged.AddListener(OnMusicToggleClick);
    }
    private void OnDisable()
    {
        _playerController.Pause -= OnPause;
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
        _music.onValueChanged.RemoveListener(OnMusicToggleClick);
        _resumeButton.onClick.RemoveListener(OnResumeButtonClick);
    }

    private void OnMusicToggleClick(bool flag)
    {
        if (flag == true)
            MusicOn?.Invoke();
        else
            MusicOff?.Invoke();
    }

    private void OnPause()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnResumeButtonClick()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}

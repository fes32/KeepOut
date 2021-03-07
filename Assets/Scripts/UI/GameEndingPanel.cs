using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndingPanel : MonoBehaviour
{
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private GameObject _gameEndingPanel;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    private void SetTime(int elapsedSec)
    {
        int minutes = elapsedSec / 60;
        int sec = elapsedSec - minutes * 60;
        int hour = minutes / 60;
        int newMinnutes = minutes - hour * 60;

        _time.text = " "+hour +":"+ newMinnutes + ":"+sec;
    }

    public void Activate(int elapsedTime)
    {
        _playerUI.SetActive(false);
        _gameEndingPanel.SetActive(true);
        SetTime(elapsedTime);
    }
}
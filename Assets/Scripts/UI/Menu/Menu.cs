using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private float _disapperaringTime;
    [SerializeField] private Door _door;
    [SerializeField] private float _timeAfterDoorOpening;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonCLick);
        _door.Opened += WaitEndAnim;    
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
        _exitButton.onClick.RemoveListener(OnExitButtonCLick);
        _door.Opened -= WaitEndAnim;
    }

    private void OnPlayButtonClick()
    {
        StartCoroutine(DisapperaringMenuPanel());
    }

    private void OnExitButtonCLick()
    {
        Application.Quit();
    }

    private IEnumerator DisapperaringMenuPanel()
    {
        float elapsedTime = 0;

        while (elapsedTime < _disapperaringTime)
        {
            elapsedTime += Time.deltaTime;

            _canvas.alpha -= _disapperaringTime * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator WaitingEndAnimation()
    {
        float elapsedTime = 0;
        while (elapsedTime<_timeAfterDoorOpening)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void WaitEndAnim()
    {
        StartCoroutine(WaitingEndAnimation());
    }
}
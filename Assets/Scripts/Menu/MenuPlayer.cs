using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _mouseTracker;
    [SerializeField] private GameObject _head;
    [SerializeField] private Animator _animator;
    [SerializeField] private MenuCamera _camera;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClick);
        _camera.StartAnimation();
        _mouseTracker.SetActive(false);
        _head.transform.rotation = new Quaternion(0, 0, 0,0);
        _animator.Play("CutScene");
    }
}
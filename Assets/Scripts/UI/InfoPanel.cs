using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _enterButton;

    private bool _isActive;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _enterButton.onClick.AddListener(OnEnterButtonClick);
    }
    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
        _enterButton.onClick.RemoveListener(OnEnterButtonClick);
    }

    private void OnExitButtonClick()
    {
        _infoPanel.SetActive(false);
        _isActive = false;
    }

    private void OnEnterButtonClick()
    {
        if (_isActive == false)
        {
            _infoPanel.SetActive(true);
            _isActive = true;
        }
    }
}

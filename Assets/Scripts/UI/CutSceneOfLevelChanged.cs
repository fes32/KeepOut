using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutSceneOfLevelChanged : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _durationCutScene;
    [SerializeField] private float _speed;

    public void ShowCutScene()
    {
        StartCoroutine(ChangeCanvasGroupAlfa());
    }

    private IEnumerator ChangeCanvasGroupAlfa()
    {
        _canvasGroup.alpha = 1;
        float elapsedTime = 0;

        while(elapsedTime <_durationCutScene)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha -= elapsedTime/_speed *Time.deltaTime;
            yield return null;
        }
    }   
}
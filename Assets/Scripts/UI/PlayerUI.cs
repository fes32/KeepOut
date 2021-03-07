using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;
    [SerializeField] TMP_Text _countHealthPotions;
    [SerializeField] TMP_Text _countSingTables;

    private int _currentHealth = 100;

    private void OnEnable()
    {
        _player.HealthChanged += ChangedHealthHandler;
        _player.SingTablesCountChanged += ChangedSingTablesCount;
        _player.HealthPotionCountChanged += ChangedHealthPotionsCount;
    }
    private void OnDisable()
    {
        _player.HealthChanged -= ChangedHealthHandler;
        _player.SingTablesCountChanged -= ChangedSingTablesCount;
        _player.HealthPotionCountChanged -= ChangedHealthPotionsCount;
    }

    private void ChangedHealthHandler(int currentHealth)
    {
        _currentHealth = currentHealth;

        StopCoroutine(HealthChange());
        StartCoroutine(HealthChange());
    }

    private void ChangedHealthPotionsCount(int currentCount)
    {
        _countHealthPotions.text = currentCount.ToString();
    }

    private void ChangedSingTablesCount(int currentCount)
    {
        _countSingTables.text = currentCount.ToString();
    }


    private IEnumerator HealthChange()
    {
        while (_currentHealth != _healthBar.value)
        {
            _healthBar.value = Mathf.MoveTowards(_healthBar.value, _currentHealth, _speed * Time.deltaTime);

            yield return null;
        }
    }
}
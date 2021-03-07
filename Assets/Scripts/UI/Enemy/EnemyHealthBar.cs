using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _speed;

    public  Player _target;
    private float targetHealth;
    
    private void Update()
    {
        if(_target!=null)
        _healthBar.transform.LookAt(_target.transform);
    }

    private IEnumerator ChangeHealth()
    {
        while (_healthBar.value != targetHealth)
        {
            _healthBar.value = Mathf.MoveTowards(_healthBar.value, targetHealth, _speed*Time.deltaTime);
            yield return null;
        }
    }

    public void Activate(Player target)
    {
        _target = target;

        _healthBar.gameObject.SetActive(true);
    }


    public void ChangeHealth(float currentHealth)
    {
        targetHealth = (int)(_healthBar.maxValue / 100 * currentHealth);
        StopCoroutine(ChangeHealth());
        StartCoroutine(ChangeHealth());
    }
}
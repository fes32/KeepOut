using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _minHealth;
    [SerializeField] private EnemyModel _model;
    [SerializeField] private EnemyHealthBar _enemyHealthBar;
    [SerializeField] private StateMachine _stateMachine;

    private Player _target;

    private bool _canAttack=true;
    private bool _isDead = false;
    private bool _isSleep = true;
    private bool _canMove = false;

    public Player Target => _target;
  
    private IEnumerator WaitingEndDieAnimation()
    {
        float elapsedTime = 0;
        float duratioinAnimation=0.9f;
        while (elapsedTime < duratioinAnimation)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void Die()
    {
        _model.Die();
        _isDead = true;
        StartCoroutine(WaitingEndDieAnimation());
    }

    public void TakeDamage(int damage)
    {
        _model.TakeDamage();
        _health -= damage;
        int _healthProcent = (int)(_health / ((float)_maxHealth / 100));
        _enemyHealthBar.ChangeHealth(_healthProcent);

        if (_health < _minHealth)
            Die();
    }

    public void Init(Player player)
    {
        _target = player;
        _enemyHealthBar = GetComponent<EnemyHealthBar>();
        _enemyHealthBar.Activate(_target);
        _stateMachine.enabled = true;
    }
}
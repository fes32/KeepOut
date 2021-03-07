using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _minHealth;
    [SerializeField] private float _durationAttack;
    [SerializeField] private float _distanceForAttack;
    [SerializeField] private float _attackCooldownTime;
    [SerializeField] private float _speed;
    [SerializeField] private EnemyModel _model;
    [SerializeField] private int _attackDamage;
    [SerializeField] private int _maxHealth;
    [SerializeField] private EnemyHealthBar _enemyHealthBar;

    private Cell _parent;
    private Player _target;
    private State _currentState = State.Sleep;
    private float _elapsedTimeAfterLastAttack=0;
    private bool _canAttack=true;
    private float _walkTime = 0, _walkCooldown = 0.3f;
    private bool _isDead = false;


    private void OnDisable()
    {
        if (_parent != null)
            _parent.CellVisited -= OnCellVisited;
    }

    private void OnCellVisited()
    {
        _parent.CellVisited -= OnCellVisited;
        _enemyHealthBar.Activate(_target);
        _currentState = State.Idle;
    }

    private void Move()
    {
            _currentState = State.Move;
            transform.LookAt(_target.transform);

            Vector3 targetPosition = transform.position + transform.forward * _speed * Time.deltaTime;

            transform.position = targetPosition;
            _model.Move();
    }

    private void Attack()
    {
        _currentState = State.Attack;
        _elapsedTimeAfterLastAttack = 0;
        _canAttack = false;
        _model.Attack();
        StartCoroutine(WaitingEndAttackAnimation());
    }

    private void Idle()
    {
        _walkTime = 0;
        _currentState = State.Idle;
        _model.Idle();
        transform.LookAt(_target.transform);
    }

    private void Update()
    {
        if (_currentState == State.Sleep)
            return;

        else if (!_isDead)
        {

            if (_currentState == State.Idle)
            {
                transform.LookAt(_target.transform);
                if ((Vector3.Distance(transform.position, _target.transform.position) > _distanceForAttack))
                    Move();
                else if (_canAttack)
                    Attack();
            }
            else if (_currentState == State.Move)
            {
                _walkTime += Time.deltaTime;
                if (_walkTime > _walkCooldown)
                    Idle();
                else
                    Move();
            }
            else if (_currentState == State.Attack)
            {
                if (_elapsedTimeAfterLastAttack > _durationAttack)
                    Idle();
            }

            if (_elapsedTimeAfterLastAttack > _attackCooldownTime)
                _canAttack = true;

            _elapsedTimeAfterLastAttack += Time.deltaTime;
        }
    }

    
    private IEnumerator WaitingEndAttackAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime <= 0.6f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward * _distanceForAttack, out RaycastHit hit, 10000))
        {
            if (hit.collider.TryGetComponent(out DestructibleObject destructibleObject))
                destructibleObject.Hit();
            else if (hit.collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(_attackDamage);
                _model.PlayAttackSound();
            }
        }
    }

    private IEnumerator WaitingEndDieAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.9)
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

    public void SetParent(Cell cell)
    {
        _parent = cell;
        _parent.CellVisited += OnCellVisited;
    }

    public void SetTarget(Player player)
    {
        _target = player;
        _enemyHealthBar = GetComponent<EnemyHealthBar>();
    }

    enum State
    {
        Sleep,
        Idle,
        Move,
        Attack,
    }
}
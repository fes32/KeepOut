using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Character _character;
    [SerializeField] private SingTable _singTable;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _restoredHealth;
    [SerializeField] private int _damage;
    [SerializeField] private int _countSingTable;

    private int _health = 100;
    private int _minHealth = 1;
    private int _maxHealth = 100;
    private int _countHealthPotions = 2;
    private int _maxCoutnHealthPotion = 10;
    private float _elapsedTimeAfterAttack = 0;
    private float _walkTime = 0, _walkCooldown = 0.2f;
    private MoveState _moveState = MoveState.Idle;
    private Vector3 _rotation;
    private bool _canAttack = true;

    public event UnityAction Dying;
    public event UnityAction<int> HealthChanged;
    public event UnityAction<int> HealthPotionCountChanged;
    public event UnityAction<int> SingTablesCountChanged;

    private void OnEnable()
    {       
        _levelGenerator.LevelEnding += SpawnOnStartPosition;

        SpawnOnStartPosition();
        StartCoroutine(SetValues());
    }

    private void OnDisable()
    {
        if (_levelGenerator != null)
            _levelGenerator.LevelEnding -= SpawnOnStartPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthPotion healthPotion))
        {
            TakeHealthPotion(1);
            healthPotion.Taked();           
        }
    }    

    private IEnumerator SetValues()
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HealthChanged?.Invoke(_health);

        HealthPotionCountChanged?.Invoke(2);
        SingTablesCountChanged?.Invoke(_countSingTable);
    }

    private IEnumerator AttackAfterEndAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime <= _attackDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward * _attackDistance, out RaycastHit hit, _attackDistance))
        {
            if (hit.collider.TryGetComponent(out DestructibleObject destructibleObject))
            {
                destructibleObject.Hit();
            }
            else if (hit.collider.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
            }
            else if (hit.collider.TryGetComponent(out Chest chest))
            {
                chest.Open();
            }
            else if(hit.collider.TryGetComponent(out SingTable singTable))
            {
                singTable.Destroy();
                GetSingTables(1);
            }               
        }

        Idle();
    }

    private IEnumerator AttackCooldown()
    {
        float elapsedTime = 0;

        while (elapsedTime < _attackCooldown)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _canAttack = true;
    }

    private void Update()
    {
        if (_moveState == MoveState.Attack)
        {
            if (_elapsedTimeAfterAttack >= _attackDuration)
                Idle();
            else
                _elapsedTimeAfterAttack += Time.deltaTime;
        }
        else if (_moveState == MoveState.Walk)
        {
            _walkTime -= Time.deltaTime;
            if (_walkTime <= 0)
            {
                Idle();
            }
        }
    }

    private void SpawnOnStartPosition()
    {
        transform.position = _spawnPoint.position;        
    }

    private void Idle()
    {
        _moveState = MoveState.Idle;
        _character.Idle();
        _elapsedTimeAfterAttack = 0;
    }

    private void TakeHealthPotion(int countHealthPotion)
    {
        _countHealthPotions += countHealthPotion;
        if (_countHealthPotions > _maxCoutnHealthPotion)
            _countHealthPotions = _maxCoutnHealthPotion;

        HealthPotionCountChanged(_countHealthPotions);
    }

    public void Look(Vector2 rotate)
    {
        float scaleRotateSpeed = _rotateSpeed * Time.deltaTime;
        _rotation.y += rotate.x * scaleRotateSpeed;
        _rotation.x = 0;
        transform.localEulerAngles = _rotation;
    }

    public void Move(Vector2 direction)
    {
        if (_moveState != MoveState.Attack)
        {
            if (direction.magnitude > 0.01)
            {
                _moveState = MoveState.Walk;

                float scaleMoveSpeed = _walkSpeed * Time.deltaTime;
                Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

                Vector3 targetPosition = transform.position + move * scaleMoveSpeed;

                transform.position = targetPosition;
                _walkTime = _walkCooldown;
                _character.Move();
            }
            else
                Idle();
        }
    }

    public void TryAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;

            StartCoroutine(AttackCooldown());

            _moveState = MoveState.Attack;

            _character.Attack();
            StartCoroutine(AttackAfterEndAnimation());
        }
    }

    public void TryHealing()
    {
        if (_countHealthPotions > 0)
        {
            _health += _restoredHealth;

            if (_health >= _maxHealth)
                _health = _maxHealth;

                _countHealthPotions--;
            _character.Healing();
                HealthChanged?.Invoke(_health);
            HealthPotionCountChanged?.Invoke(_countHealthPotions);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < _minHealth)
            Dying?.Invoke();
        else
            HealthChanged?.Invoke(_health);
    }

    public void SetSingTable()
    {
        if (_countSingTable > 0)
        {
            _levelGenerator.SpawnSingTable(transform);
            _character.SetTable();
            _countSingTable--;
            SingTablesCountChanged?.Invoke(_countSingTable);
        }
    }

    public void GetSingTables(int count)
    {
        _countSingTable += count;
        SingTablesCountChanged?.Invoke(_countSingTable);
    }

    enum MoveState
    {
        Idle,
        Walk,
        Attack
    }
}
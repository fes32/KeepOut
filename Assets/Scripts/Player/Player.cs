using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private Character _character;
    [SerializeField] private int _restoredHealth;

    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Inventory _inventory;

    private int _health = 100;
    private int _maxHealth = 100;
    private int _minHealth = 1;

    private bool _canMove = true;
    private bool _isIdle = true;

    public event UnityAction Dying;
    public event UnityAction<int> HealthChanged;
   
    private void OnEnable()
    {       
        HealthChanged?.Invoke(_health);

        _playerAttack.EndAttack += EndAttack;
        _playerAttack.TakeSingTable += GetSingTables;

    }

    private void OnDisable()
    {
        _playerAttack.EndAttack -= EndAttack;
        _playerAttack.TakeSingTable -= GetSingTables;
    }

    private void EndAttack()
    {
        _canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthPotion healthPotion))
        {
            TakeHealthPotion(healthPotion);
            healthPotion.Taked();           
        }
    }        

    private void TakeHealthPotion(DropItem item)
    {
        _inventory.AddItem(Inventory.InventoryItemType.Health, 1);
    }

    public void Move(Vector3 direction)
    {
        if (_canMove)
        {
            if (direction.magnitude > _playerMovement.MinimalValueForMove)
            {
                _playerMovement.Move(direction);
                _character.Move();
            }
            else
                _character.Idle();            
        }      
    }

    public void Look(Vector3 rotate)
    {
        _playerMovement.Look(rotate);
    }

    public void Attack()
    {
        if (_playerAttack.CanAttack)
        {
            _playerAttack.Attack();
            _character.Attack();
            _canMove = false;
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < _minHealth)
            Dying?.Invoke();

        HealthChanged?.Invoke(_health);
    }

    public void Healing()
    {
        if (_inventory.RemoveItem(Inventory.InventoryItemType.Health))
        {
            _health += _restoredHealth;

            if (_health >= _maxHealth)
                _health = _maxHealth;

            _character.Healing();
            HealthChanged?.Invoke(_health);
        }
    }

    public void SetSingTable()
    {
        if (_inventory.RemoveItem(Inventory.InventoryItemType.SingTable))
        {
            _levelGenerator.SpawnSingTable(transform);
            _character.SetTable();
        }
    }

    public void GetSingTables(int count)
    {
       _inventory.AddItem(Inventory.InventoryItemType.SingTable,count);
    }
}
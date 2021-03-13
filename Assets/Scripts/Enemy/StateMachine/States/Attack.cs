using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    [SerializeField] private EnemyAttack _enemy;

    private void OnEnable()
    {
        _enemy.Attack();
    }
}
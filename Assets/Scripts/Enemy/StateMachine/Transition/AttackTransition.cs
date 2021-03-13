using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : Transition 
{
    [SerializeField] private float _distanceToAttack;
    [SerializeField] private EnemyAttack _enemy;

    private void Update()
    {
        if ((Vector3.Distance(Target.transform.position, transform.position) <= _distanceToAttack) & _enemy.CanAttack & _enemy.IsAttack==false)
            NeedTransit = true;
    }
}
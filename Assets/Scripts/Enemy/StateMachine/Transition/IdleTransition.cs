using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTransition : Transition
{
    [SerializeField] private float _stopDistance;
    [SerializeField] private EnemyAttack _enemy;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance <= _stopDistance & _enemy.IsAttack == false)
            NeedTransit = true;
    }
}
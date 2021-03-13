using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : State
{
    [SerializeField] private float _speed;
    [SerializeField] private EnemyMovement _enemyMovement;

    private void OnEnable()
    {
        _enemyMovement.StartMove();
    }

    private void OnDisable()
    {
        _enemyMovement.StopMove();
    }

    private void Update()
    {
        transform.LookAt(Target.transform);

        Vector3 targetPosition = transform.position + transform.forward * _speed * Time.deltaTime;

        transform.position = targetPosition;
    }
}

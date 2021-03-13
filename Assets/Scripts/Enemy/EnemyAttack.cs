using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _durationAttack;
    [SerializeField] private float _attackCooldownTime;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackDistance;
    [SerializeField] private EnemyModel _model;

    public bool CanAttack { get; private set; } = true;
    public bool IsAttack { get; private set; } = false;

    private IEnumerator WaitingEndAttackAnimation()
    {
        CanAttack = false;
        IsAttack = true;

        float elapsedTime = 0;
        while (elapsedTime <= _durationAttack)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward * _attackDistance, out RaycastHit hit, _attackDistance))
        {
            if (hit.collider.TryGetComponent(out DestructibleObject destructibleObject))
                destructibleObject.Hit();
            else if (hit.collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(_attackDamage);
            }
        }

        StartCoroutine(AttackCooldown());
        IsAttack = false;
    }

    private IEnumerator AttackCooldown()
    {
        float elapsedTime = 0;

        while (elapsedTime <= _attackCooldownTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CanAttack = true;        
    }

    public void Attack()
    {
        _model.Attack();
        StartCoroutine(WaitingEndAttackAnimation());
    }
}
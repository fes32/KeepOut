using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damage;

    private float _elapsedTimeAfterAttack = 0;

    public bool CanAttack { get; private set; } = true;

    public event UnityAction<int> TakeSingTable;
    public event UnityAction EndAttack;

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
            else if (hit.collider.TryGetComponent(out SingTable singTable))
            {
                singTable.gameObject.SetActive(false);
                TakeSingTable?.Invoke(1);
            }
        }

        EndAttack?.Invoke();
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        float elapsedTime = 0;

        while (elapsedTime < _attackCooldown)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        CanAttack = true;
    }

    public void Attack()
    {
        CanAttack = false;
        StartCoroutine(AttackAfterEndAnimation());
    }
}
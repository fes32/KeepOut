using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _stopDistanceToTarget;

    public bool IsMove { get; private set; }

    public float StopDistanceToTarget => _stopDistanceToTarget;

    public void StartMove()
    {
        IsMove = true;
    }
    
    public void StopMove()
    {
        IsMove = false;
    }
}
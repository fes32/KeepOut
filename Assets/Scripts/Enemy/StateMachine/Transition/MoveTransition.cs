using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransition : Transition
{
    [SerializeField]private float _startMoveDistanceToTarget;

    private void Update()
    {
        if (Vector3.Distance(Target.transform.position, transform.position) > _startMoveDistanceToTarget)
            NeedTransit = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTransition : Transition
{
    [SerializeField] private float _activateDistance;

    private void Update()
    {
        if (Vector3.Distance(Target.transform.position, transform.position) <= _activateDistance)
            NeedTransit = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private void Update()
    {
        transform.LookAt(Target.transform);
    }
}
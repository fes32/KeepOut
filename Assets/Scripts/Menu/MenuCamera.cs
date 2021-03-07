using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void StartAnimation()
    {
        _animator.Play("Camera");
    }
}
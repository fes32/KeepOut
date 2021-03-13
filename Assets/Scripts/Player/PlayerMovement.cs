using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _walkSpeed;

    private Vector3 _rotation;
    public float MinimalValueForMove { get; private set; } = 0.01f;

    public void Look(Vector2 rotate)
    {
        float scaleRotateSpeed = _rotateSpeed * Time.deltaTime;
        _rotation.y += rotate.x * scaleRotateSpeed;
        _rotation.x = 0;
        transform.localEulerAngles = _rotation;
    }

    public void Move(Vector2 direction)
    {
        float scaleMoveSpeed = _walkSpeed * Time.deltaTime;
        Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

        Vector3 targetPosition = transform.position + move * scaleMoveSpeed;

        transform.position = targetPosition;
    }
}
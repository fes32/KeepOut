using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _walkSpeed;

    public float MinimalValueForMove { get; private set; } = 0.01f;

    public void Look(Vector2 rotate)
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        float scaleRotateSpeed = _rotateSpeed * Time.deltaTime;
        rotation.y += rotate.x * scaleRotateSpeed;
        rotation.x = 0;
        transform.localEulerAngles = rotation;
    }

    public void Move(Vector2 direction)
    {
        float scaleMoveSpeed = _walkSpeed * Time.deltaTime;
        Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

        Vector3 targetPosition = transform.position + move * scaleMoveSpeed;

        transform.position = targetPosition;
    }
}
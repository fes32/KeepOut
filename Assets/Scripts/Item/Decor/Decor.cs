using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : MonoBehaviour
{
    private Transform _cell;

    public Transform Cell => _cell;

    public void SetParentCell(Transform cell)
    {
        _cell = cell;
    }
}

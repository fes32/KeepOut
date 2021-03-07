using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropItem : MonoBehaviour
{
    [SerializeField] private int _dropChance;

    public int DropChance => _dropChance;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : DropItem
{
    public void Taked()
    {
        gameObject.SetActive(false);
    }
}
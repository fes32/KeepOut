﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingTable : DropItem
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
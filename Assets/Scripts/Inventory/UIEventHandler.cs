﻿using UnityEngine;
using System.Collections;

public class UIEventHandler : MonoBehaviour {

    public delegate void ItemEventHandler(Item item);
    public static event ItemEventHandler OnItemAddedToInventory;

    public static void ItemAddedToInventory(Item item)
    {
        OnItemAddedToInventory(item);
    }
}

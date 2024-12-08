using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private bool isAvailable;
    public bool IsAvailable => isAvailable;
    public void ResetSlot()
    {
        isAvailable = true;
    }
    public Transform GetSlotPos()
    {
        isAvailable = false;
        return transform;
    }
}

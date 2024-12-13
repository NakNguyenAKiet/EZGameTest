using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventories : MonoBehaviour
{
    [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private void Awake()
    {
        MyGameEvent.Instance.OnPickUpAnimal += OnPickUpAnimal;
    }

    public void ResetInventory()
    {
        foreach (var slot in inventorySlots)
        {
            slot.ResetSlot();
        }
    }
    void OnPickUpAnimal(AnimalObstacle animal)
    {
        InventorySlot slot = inventorySlots.Find(x => x.IsAvailable);
        animal.transform.SetParent(slot.GetSlotPos());
        animal.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}

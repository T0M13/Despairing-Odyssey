using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemType type;
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.inventoryBehaviour.inventoryFull) return;
            player.inventoryBehaviour.AddItem(type, amount);
            player.UpdateInventory();
            DeactivateItem();
        }
    }

    private void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
    private void ActivateItem()
    {
        gameObject.SetActive(true);
    }

}

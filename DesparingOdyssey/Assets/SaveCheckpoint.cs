using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.inventoryBehaviour.inventoryItemSlots.Contains(InventoryItemType.SaveItem))
            {
                player.SavedPosition = new Vector3(transform.position.x, spawnPoint.position.y, transform.position.z);
                player.SavedPositionSaved = true;
                player.inventoryBehaviour.RemoveItem(InventoryItemType.SaveItem, 1);
                player.UpdateInventory();
            }
        }
    }
}

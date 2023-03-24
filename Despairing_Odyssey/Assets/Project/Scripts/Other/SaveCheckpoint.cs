using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float saveTime = 3;
    [SerializeField] private float saveTimer;
    [SerializeField] private EffectComponent effectBehaviour;

    private void Awake()
    {
        effectBehaviour.SpawnEffectClone(transform.position + new Vector3(0,0.1f,0));
        effectBehaviour.DeactivateEffectClone();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            saveTimer = saveTime;
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.inventoryBehaviour.inventoryItemSlots.Contains(InventoryItemType.SaveItem) && !player.SavedPositionSaved)
                effectBehaviour.ActivateEffectClone();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            saveTimer -= Time.deltaTime;
            if (saveTimer <= 0)
            {
                saveTimer = saveTime;

                PlayerController player = other.GetComponent<PlayerController>();
                if (player.inventoryBehaviour.inventoryItemSlots.Contains(InventoryItemType.SaveItem) && !player.SavedPositionSaved)
                {
                    player.SavedPosition = new Vector3(transform.position.x, spawnPoint.position.y, transform.position.z);
                    player.SavedPositionSaved = true;
                    player.inventoryBehaviour.RemoveItem(InventoryItemType.SaveItem, 1);
                    player.UpdateInventory();
                    effectBehaviour.DeactivateEffectClone();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        effectBehaviour.DeactivateEffectClone();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryBehaviour", menuName = "Behaviours/PlayerInventoryBehaviour")]
public class PlayerInvetoryComponent : ScriptableObject
{
    public InventoryItemType[] inventoryItemSlots;
    public int inventorySize = 3;
    public int inventorySlotsUsed = 0;
    public bool inventoryFull;


    public void InitiateInventory()
    {
        inventoryItemSlots = new InventoryItemType[inventorySize];
        RemoveAll();
    }

    public void AddItem(InventoryItemType inventoryItem, int amount)
    {
        for (int j = 0; j < amount; j++)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventoryItemSlots[i] == InventoryItemType.None)
                {
                    inventoryItemSlots[i] = inventoryItem;
                    inventorySlotsUsed++;
                    if (inventorySlotsUsed >= inventorySize)
                        inventoryFull = true;
                    break;
                }
            }
        }
    }

    public void RemoveItem(InventoryItemType inventoryItem, int amount)
    {
        for (int j = 0; j < amount; j++)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventoryItemSlots[i] == inventoryItem)
                {
                    inventoryItemSlots[i] = InventoryItemType.None;
                    inventorySlotsUsed--;
                    if (inventorySlotsUsed < inventorySize)
                        inventoryFull = false;
                    break;
                }
            }
        }
    }

    public void RemoveAll()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            inventoryItemSlots[i] = InventoryItemType.None;
        }

        if (inventorySlotsUsed < inventorySize)
            inventoryFull = false;
        inventorySlotsUsed = 0;
    }



}
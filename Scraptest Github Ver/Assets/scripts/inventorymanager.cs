using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class inventorymanager : NetworkBehaviour //InventoryItem = item
{
    public int maxStackedValue = 3;
    public invSlot[] invSlots;

    public invSlot[] equipedSlots;
    public GameObject itemPrefab;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.E) &&
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("inventory"))
        {
            SceneManager.LoadScene("openworld");
        }
    }
    public bool AddItem(Item item)
    {
        // check if slot has same item with lower count than max
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlot slot = invSlots[i];
            DragableItem itemInSlot = slot.GetComponentInChildren<DragableItem>();

            if (itemInSlot != null && 
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedValue &&
                itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // find empty slot
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlot slot = invSlots[i];
            DragableItem itemInSlot = slot.GetComponentInChildren<DragableItem>();

            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, invSlot slot)
    {
        GameObject newItemGo = Instantiate(itemPrefab, slot.transform);
        DragableItem inventoryItem = newItemGo.GetComponent<DragableItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetEquipedItem(int slotIndex)
    {

        invSlot slot = equipedSlots[slotIndex];
        DragableItem itemInSlot = slot.GetComponentInChildren<DragableItem>();
        if (itemInSlot != null)
        {
            return itemInSlot.item;
        }


        return null;
    }
}

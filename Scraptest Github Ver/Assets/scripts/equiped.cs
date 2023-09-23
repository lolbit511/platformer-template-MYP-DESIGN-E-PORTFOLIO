using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class equiped : MonoBehaviour
{
    public inventorymanager inventoryManager;

    public int healthTotal;
    public int defenseTotal;
    public int attackTotal;
    public int energyTotal;
    public int XPbonusTotal;

    public TextMeshProUGUI healthTotalDisplay;
    public TextMeshProUGUI defenseTotalDisplay;
    public TextMeshProUGUI attackTotalDisplay;
    public TextMeshProUGUI energyTotalDisplay;
    public TextMeshProUGUI XPbonusTotalDisplay;


    public void updateStats()
    {
        //DragableItem.GetEquipedItem();
    }

}
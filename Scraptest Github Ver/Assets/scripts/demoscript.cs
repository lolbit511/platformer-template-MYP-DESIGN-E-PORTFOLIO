using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class demoscript : MonoBehaviour
{
    public inventorymanager inventoryManager;
    public Item[] itemsToPickUp;

    public TextMeshProUGUI StatTotalDisplay;

    //bool picking_item = true;
    public void PickupItem(int id)
    {

        Debug.Log("item spawned: " + id);
        bool result = inventoryManager.AddItem(itemsToPickUp[id]);
        if (result)
        {
            Debug.Log("item added");
        }
        else
        {
            Debug.Log("item not added");
        }
    }


    static public bool checkStats;

    static public bool checkHealth;
    static public int maxHP = 5;

    static public bool checkEnergy;
    static public int maxEnergy = 15;

    void Update()
    {
        if (checkStats)
        {
            checkingFullStats();
        }

        if (checkHealth)
        {
            maxHP = checkingHealth();
        }
        if (checkEnergy)
        {
            maxEnergy = checkingEnergy();
        }

    }
    public int checkingHealth()
    {
        int healthTotal = 5;

        for (int i = 0; i < 9; i++)
        {
            Item recivedItem = inventoryManager.GetEquipedItem(i);

            if (recivedItem != null)
            {
                healthTotal = healthTotal + recivedItem.health;
            }

        }


        checkHealth = false;
        return healthTotal;
    }

    public int checkingEnergy()
    {
        int energyTotal = 15;

        for (int i = 0; i < 9; i++)
        {
            Item recivedItem = inventoryManager.GetEquipedItem(i);

            if (recivedItem != null)
            {
                energyTotal = energyTotal + recivedItem.health;
            }

        }

        checkHealth = false;
        return energyTotal;
    }

    void checkingFullStats()
    {
        int healthTotal = 5;
        int defenseTotal = 0;
        int attackTotal = 5;
        int energyTotal = 10;
        int XPbonusTotal = 0;

        for (int i = 0; i < 9; i++)
        {
            Item recivedItem = inventoryManager.GetEquipedItem(i);

            if (recivedItem != null)
            {
                healthTotal = healthTotal + recivedItem.health;
                defenseTotal = defenseTotal + recivedItem.defense;
                attackTotal = attackTotal + recivedItem.attack;
                energyTotal = energyTotal + recivedItem.energy;
                XPbonusTotal = XPbonusTotal + recivedItem.wisdom;
            }

        }

        StatTotalDisplay.text =
            "Full Stats" +
            "\n VITALITY: " + healthTotal +
            "\n DEFENSE: " + defenseTotal +
            "\n ATTACK: " + attackTotal +
            "\n ENERGY: " + energyTotal +
            "\n WISDOM: " + XPbonusTotal
            ;

        checkStats = false;
    }
}

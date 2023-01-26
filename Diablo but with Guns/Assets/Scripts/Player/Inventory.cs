using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private bool inventoryEnabled;


    private int allSlots;
    private int enabledSlotry;
    private GameObject[] slot; // Gameobject variable for all Slots 

    public GameObject slotHolder;

    void Start()
    {
        allSlots = 28;
        slot = new GameObject[allSlots];
        for (int i = 0; i < allSlots; i++) // read all Slots in the begin of the Game
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null) // check if Slot is empty
                slot[i].GetComponent<Slot>().empty = true; // if it is true
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryEnabled = !inventoryEnabled; // switching Inventory on off 

        if (inventoryEnabled == true)
        {
            inventory.SetActive(true); // Activate or deactivate the object true activates the GameObject and false deactivates it
        }
        else
        {
            inventory.SetActive(false);
        }
        }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject; // tempo. Item
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.ID, item.type, item.description, item.icon); // refere to ItemScript
        }
    }
    void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for(int i = 0; i< allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {// check every single Slot to see if there is a Item
                //add item to slot 
                itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().description = itemDescription;

                itemObject.transform.parent = slot[i].transform; // moves Item that got picked up into the slot too
                itemObject.SetActive(false);// Item gets not selected in Playermodel instantly by picking it up

                slot[i].GetComponent<Slot>().UpdateSlot();  // calling void from here
                slot[i].GetComponent<Slot>().empty = false;
                return;
            }

            
        }
    }
    }


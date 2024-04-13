using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject slotButton;
    public bool questItem = false;


    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(inventory.isFull == false)
            {
                inventory.isFull = true;
                questItem = true;
                Instantiate(slotButton, inventory.slot.transform);
                Destroy(gameObject);
            }
        }
    }
}

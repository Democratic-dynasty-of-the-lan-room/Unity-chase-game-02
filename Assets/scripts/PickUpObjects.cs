using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{

    public GameObject itemButton;

    private InventoryScript inventory;   

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function to be called in pickupscript. Instanciates button in inventory
    public void Instanciates()
    {
        
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            
            if (inventory.isFull[i] == false)
            {
                
                Instantiate(itemButton, inventory.slots[i].transform, false);
                Destroy(gameObject);

                inventory.isFull[i] = true;         

                break;
            }
        }
    }
}

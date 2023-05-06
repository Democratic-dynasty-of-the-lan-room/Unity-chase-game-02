using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    

    bool flash = false;

    private Light Torchlight;

    // Start is called before the first frame update
    void Start()
    {       
        Torchlight = this.GetComponent<Light>();
        Torchlight.enabled = false;
    }

    // Update is called once per frame
    //Turning torch on and off with two functions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Debug.Log("Hello from Flashlight");
            if(flash == false)
            {
                On();
            }
            else
            {
                off();
            }
        
        }
    }

    //Onfunction
    private void On()
    {
        flash = true;
        Torchlight.enabled = true;
    }
    //Off function
    private void off()
    {
        flash = false;
        Torchlight.enabled = false;

    } 
}

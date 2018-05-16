using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCIEL.Toolkit;
using UnityEngine.UI;

/// <summary>
/// Pairs an object with a location.
/// When the game object is released in contact with the location, 
/// it snaps into place.
/// </summary>
public class SnapObject : SimpleGrab {

    public GameObject objectLocation;
    [TextArea(10,15)]
    public string description;
    private bool snap = false;
    private bool placed = false; // Records whether the object has been placed or not

    public override void OnTriggerWasPressed(InteractionController controller)
    {
        if (!placed)
        {
            base.OnTriggerWasPressed(controller);
        }
        else
        {
            controller.GetComponent<ControllerUI>().ToggleCanvas();
            if (controller.GetComponentInChildren<Canvas>())
            {
                controller.GetComponentInChildren<Text>().text = description;
            }
        }
    }

    public override void OnTriggerWasReleased(InteractionController controller)
    {
        if(!placed)
        {
            base.OnTriggerWasReleased(controller);
            if (snap)
            {
                transform.position = objectLocation.transform.position;
                transform.rotation = objectLocation.transform.rotation;
                Destroy(GetComponent<Rigidbody>()); // there may be a more elegant way to lock the item in place.
                GetComponent<Collider>().isTrigger = true;
                objectLocation.SetActive(false);
                placed = true;
            }
        } 
    }



    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == objectLocation)
        {
            snap = true;
            Debug.Log("Snap: true");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        snap = false;   
    }
}

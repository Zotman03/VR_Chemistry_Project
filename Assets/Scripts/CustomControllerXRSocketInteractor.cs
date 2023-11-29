using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomControllerXRSocketInteractor : XRSocketInteractor
{
    public void Start()
    {
        Debug.Log("START CUSTOM XR SOCKET");
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        Debug.Log("checked canselect for... " + (interactable as XRBaseInteractor).name);
        if (interactable.isSelected)
        {
            Debug.Log("considered is alr selected");
            return true;
        }

        return base.CanSelect(interactable);
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        Debug.Log("CAN HOVER" + (interactable as XRBaseInteractor).name);
        return base.CanHover(interactable);
    }
}

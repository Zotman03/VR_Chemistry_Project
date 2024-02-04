using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomXRSocketInteractor : XRSocketInteractor
{
    public Transform smallAttachPoint;
    public Transform mediumAttachPoint;
    public Transform largeAttachPoint;
    Transform chosenAttach;

    //public override bool CanSelect(IXRSelectInteractable interactable)
    //{
    //    /*
    //    if ((interactable as XRBaseInteractor) != null) {
    //        Debug.Log("checked canselect for... " + (interactable as XRBaseInteractor).name);
    //    }
    //    */
    //    /*
    //    if (interactable.isSelected && interactable.selectingInteractor != this)
    //    {
    //        Debug.Log("considered is alr selected for " + this.name);
    //        return true;
    //    }*/

        
    //    if ((interactable != null) && ((interactable as XRBaseInteractable) != null) && (interactable as XRBaseInteractable).gameObject.CompareTag("GameController"))
    //    {
    //        // For demonstration: Always allow selection by gamecontroller.
    //        // Add more logic if needed.
    //        return true;
    //    }

    //    return base.CanSelect(interactable);
    //}

    /*

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if ((interactable as XRBaseInteractor) != null) {
            Debug.Log("CAN HOVER" + (interactable as XRBaseInteractor).name);
        }
        return base.CanHover(interactable);
    }

    */

    /*
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRBaseInteractable interactable = args.interactableObject as XRBaseInteractable;
        // If the interactable is currently selected by another interactor, force a deselection
        if (interactable && interactable.isSelected && interactable.selectingInteractor != this && interactable.selectingInteractor.gameObject.CompareTag("GameController"))
        {
            XRInteractionManager interactionManager = interactable.selectingInteractor.interactionManager;
            if (interactionManager)
            {
                interactionManager.Deselect(interactable.selectingInteractor, interactable);
            }
        }

        base.OnSelectEntered(args);
    }
    */

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (smallAttachPoint || mediumAttachPoint || largeAttachPoint)
        {
            XRBaseInteractable baseInteractable = args.interactableObject as XRBaseInteractable;
            if (baseInteractable)
                chosenAttach = SelectAttachPoint(baseInteractable.gameObject);
            else
                chosenAttach = mediumAttachPoint;
            SetAttachTransform(chosenAttach);
        }
        else
        {
            SetAttachTransform(attachTransform);
        }
        base.OnSelectEntering(args);
    }

    Transform SelectAttachPoint(GameObject obj)
    {
        float height = GetObjectHeight(obj);

        if (height <= .15)
            return smallAttachPoint;
        else if (height <= .6)
            return mediumAttachPoint;
        else
            return largeAttachPoint;
    }

    float GetObjectHeight(GameObject obj)
    {
        if (obj.GetComponent<Collider>())
            return obj.GetComponent<Collider>().bounds.size.y;
        else
            return 0f;
    }

    void SetAttachTransform(Transform newAttachPoint)
    {
        attachTransform = newAttachPoint;
    }
}

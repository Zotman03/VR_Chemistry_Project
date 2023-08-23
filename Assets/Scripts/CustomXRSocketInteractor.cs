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

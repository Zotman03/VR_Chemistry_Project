using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabActivityControl : MonoBehaviour
{
    private Rigidbody rb;
    private Collider[] colliders;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
            Debug.LogError("No XRGrabInteractable component found on object");
        if (rb == null)
            Debug.LogError("No Rigidbody component found on object");

        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(OnSelectExit);
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        rb.isKinematic = true;
        foreach (Collider collider in colliders)
            collider.enabled = false;
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        foreach (Collider collider in colliders)
            collider.enabled = true;
    }

    private void OnDestroy()
    {
        if (grabInteractable)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEnter);
            grabInteractable.selectExited.RemoveListener(OnSelectExit);
        }
    }
}
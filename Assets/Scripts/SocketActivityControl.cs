using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketActivityControl : MonoBehaviour
{
    XRSocketInteractor socketInteractor;
    XRBaseInteractable interactable;
    Rigidbody rb;
    Collider[] colliders;
    List<XRBaseInteractable> currentHoveredInteractables = new List<XRBaseInteractable>();

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        interactable = GetComponent<XRBaseInteractable>();
        rb = gameObject.GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        if (socketInteractor == null)
            Debug.LogError("No XRSocketInteractor component found on object " + gameObject.name);
        if (interactable == null)
            Debug.LogError("No XRBaseInteractable component found on object " + gameObject.name);
        if (rb == null)
            Debug.LogError("No Rigidbody component found on object " + gameObject.name);
    }

    void Start()
    {
        if (socketInteractor != null)
        {
            socketInteractor.hoverEntered.AddListener(SocketOnHoverEnter);
            socketInteractor.hoverExited.AddListener(SocketOnHoverExit);
        }
        if (interactable != null) {
            interactable.selectEntered.AddListener(OnSelectEnter);
            interactable.selectExited.AddListener(OnSelectExit);
        }
    }

    private void SocketOnHoverEnter(HoverEnterEventArgs args)
    {
        XRBaseInteractable interactableObj = args.interactableObject as XRBaseInteractable;
        if (!currentHoveredInteractables.Contains(interactableObj))
            currentHoveredInteractables.Add(interactableObj);
    }

    private void SocketOnHoverExit(HoverExitEventArgs args)
    {
        XRBaseInteractable interactableObj = args.interactableObject as XRBaseInteractable;
        currentHoveredInteractables.Remove(interactableObj);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        socketInteractor.socketActive = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        foreach (Collider collider in colliders)
            collider.enabled = false;
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        foreach (Collider collider in colliders)
            collider.enabled = true;

        XRSocketInteractor otherSocket = CheckHoveredSocket();
        IsSocketTriggered(otherSocket, (isActive) =>
        {
            if (!otherSocket || isActive)
                socketInteractor.socketActive = true;
        });
    }

    XRSocketInteractor CheckHoveredSocket()
    {
        foreach (var target in currentHoveredInteractables)
        {
            XRSocketInteractor otherSocket = target.GetComponent<XRSocketInteractor>();
            if (otherSocket)
                return otherSocket;
        }
        return null;
    }

    bool IsSocketTriggered(XRSocketInteractor socket, System.Action<bool> callback)
    {
        StartCoroutine(Delay(socket, callback));

        return socket && socket.isActiveAndEnabled;
    }

    IEnumerator Delay(XRSocketInteractor socket, System.Action<bool> callback)
    {
        yield return new WaitForSeconds(2f);
        callback.Invoke(socket && socket.isActiveAndEnabled);
    }

    private void OnDestroy()
    {
        socketInteractor.hoverEntered.RemoveListener(SocketOnHoverEnter);
        socketInteractor.hoverExited.RemoveListener(SocketOnHoverExit);

        interactable.selectEntered.RemoveListener(OnSelectEnter);
        interactable.selectExited.RemoveListener(OnSelectExit);
    }
}

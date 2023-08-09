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
            Debug.LogError("No XRSocketInteractor component found on object");
        if (interactable == null)
            Debug.LogError("No XRBaseInteractable component found on object");
        if (rb == null)
            Debug.LogError("No Rigidbody component found on object");
    }

    void Start()
    {
        if (socketInteractor != null && interactable != null)
        {
            socketInteractor.hoverEntered.AddListener(OnHoverEnter);
            socketInteractor.hoverExited.AddListener(OnHoverExit);

            interactable.selectEntered.AddListener((args) => {
                socketInteractor.socketActive = false;
                rb.isKinematic = true;
                foreach (Collider collider in colliders)
                    collider.enabled = false;
            });

            interactable.selectExited.AddListener((args) =>
            {
                rb.isKinematic = false;
                foreach (Collider collider in colliders)
                    collider.enabled = true;

                XRSocketInteractor otherSocket = CheckHoveredSocket();
                if (otherSocket && IsSocketTriggered(otherSocket))
                    socketInteractor.socketActive = true;
            });
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        XRBaseInteractable interactableObj = args.interactableObject as XRBaseInteractable;
        if (!currentHoveredInteractables.Contains(interactableObj))
            currentHoveredInteractables.Add(interactableObj);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        XRBaseInteractable interactableObj = args.interactableObject as XRBaseInteractable;
        currentHoveredInteractables.Remove(interactableObj);
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

    bool IsSocketTriggered(XRSocketInteractor socket)
    {
        StartCoroutine(Delay());
        return socket && socket.isActiveAndEnabled;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
    }

    private void OnDestroy()
    {
        socketInteractor.hoverEntered.RemoveListener(OnHoverEnter);
        socketInteractor.hoverExited.RemoveListener(OnHoverExit);
    }
}

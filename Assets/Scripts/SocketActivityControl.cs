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
    Liquid socketLiquid;
    bool currSocket = false;
    SceneControl sceneControl;

    //private IDataService DataService = new JsonDataService();
    //private LiquidState liquidState = new LiquidState();
    //private InteractableState interactableState = new InteractableState();

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        interactable = GetComponent<XRBaseInteractable>();
        rb = gameObject.GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        socketLiquid = GetComponentInChildren<Liquid>();

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
        ES3AutoSaveMgr.Current.Load();
    }

    private void Update()
    {
        OVRInput.Update();
        //if (currSocket &&
        if (Input.GetKeyDown(KeyCode.U) || OVRInput.GetDown(OVRInput.Button.One))
        //OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            //ES3AutoSaveMgr.Current.Save();
            Debug.Log("Change to bond scene");
            sceneControl.ToBondScene();
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
        interactableObj.GetComponent<XRSocketInteractor>().socketActive = true;
        currentHoveredInteractables.Remove(interactableObj);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        socketInteractor.socketActive = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.mass = 2f;
        foreach (Collider collider in colliders)
            collider.enabled = false;
        if (socketLiquid)
        {
            GlobalChemistryData.instance.mixedChemicalOne = socketLiquid.topSubstance;
            GlobalChemistryData.instance.mixedChemicalTwo = socketLiquid.foamSubstance;
            GlobalChemistryData.instance.mixedChemicalOneAmount = socketLiquid.topSubstanceAmount;
            GlobalChemistryData.instance.mixedChemicalTwoAmount = socketLiquid.foamSubstanceAmount;

            //liquidState.topSubstance = socketLiquid.topSubstance;
            //liquidState.topSubstanceAmount = socketLiquid.topSubstanceAmount;
            //liquidState.foamSubstance = socketLiquid.foamSubstance;
            //liquidState.foamSubstanceAmount = socketLiquid.foamSubstanceAmount;
            //liquidState.fillAmount = socketLiquid.fillAmount;
            //liquidState.scaledFillAmount = socketLiquid.scaledFillAmount;
            //liquidState.pos = socketLiquid.transform.position;
            //DataService.SaveData("/liquid-state-" + socketLiquid.name + ".json", liquidState, false);

            if (GlobalChemistryData.instance.gameStatus == "Incorrect")
            {
                GlobalChemistryData.instance.gameStatus = "Ongoing";
            }
            currSocket = true;
            //DataService.SaveData("/liquid-state-" + socketLiquid.name + ".json", liquidState, false);
        }
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
            if (otherSocket)
                otherSocket.socketActive = true;
        });
        currSocket = false;
        rb.mass = 10f;

        if (!socketLiquid)
        {
            socketInteractor.socketActive = true;
        }

        //liquidState.topSubstance = socketLiquid.topSubstance;
        //liquidState.topSubstanceAmount = socketLiquid.topSubstanceAmount;
        //liquidState.foamSubstance = socketLiquid.foamSubstance;
        //liquidState.foamSubstanceAmount = socketLiquid.foamSubstanceAmount;
        //liquidState.fillAmount = socketLiquid.fillAmount;
        //liquidState.scaledFillAmount = socketLiquid.scaledFillAmount;
        //liquidState.pos = socketLiquid.transform.position;
        ////liquidState.uniqueID = socketLiquid.uniqueID;
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
        if (socket)
            StartCoroutine(Delay(socket, callback));

        return true;
    }

    IEnumerator Delay(XRSocketInteractor socket, System.Action<bool> callback)
    {
        yield return new WaitForSeconds(2f);
        callback.Invoke(true);
    }

    private void OnDestroy()
    {
        socketInteractor.hoverEntered.RemoveListener(SocketOnHoverEnter);
        socketInteractor.hoverExited.RemoveListener(SocketOnHoverExit);

        interactable.selectEntered.RemoveListener(OnSelectEnter);
        interactable.selectExited.RemoveListener(OnSelectExit);
    }
}
